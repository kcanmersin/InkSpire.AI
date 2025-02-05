using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.Extensions.Logging;
using InkSpire.Application.Abstractions;
using Core.Data.Entity;
using InkSpire.Core.Data.Entity;
using Core.Data.Entity.User;
using System.Text.Json;
using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Xamarin.UITest.Utils;
using System.Text.RegularExpressions;

namespace InkSpire.Infrastructure.Services
{
    public class GroqLLMSettings
    {
        public string BaseUrl { get; set; } = "https://api.groq.com/openai/v1/chat/completions";
        public string ApiKey { get; set; }
        public string Model { get; set; } = "mixtral-8x7b-32768";
            //"llama-3.1-8b-instant";
        //"llama-3.3-70b-versatile";
    }

    public record GroqChatRequest(
        string Model,
        List<GroqMessage> Messages
    );

    public record GroqMessage(
        string Role,
        string Content
    );

    public class GroqChatResponse
    {
        [JsonPropertyName("choices")]
        public List<GroqChoice> Choices { get; set; }
    }

    public class GroqChoice
    {
        [JsonPropertyName("message")]
        public GroqMessageResponse Message { get; set; }
    }

    public class GroqMessageResponse
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    public class GroqLLMService : IGroqLLM
    {
        private readonly GroqLLMSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<GroqLLMService> _logger;

        private readonly IAsyncPolicy<HttpResponseMessage> _finalPolicy;

        public GroqLLMService(
            GroqLLMSettings settings,
            HttpClient httpClient,
            ILogger<GroqLLMService> logger)
        {
            _settings = settings;
            _httpClient = httpClient;
            _logger = logger;

            var retryPolicy = Policy<HttpResponseMessage>
                .HandleResult(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(3, attempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        _logger.LogWarning($"[Polly] Retry #{retryCount} after {outcome.Result?.StatusCode}. Waiting {timespan.TotalSeconds}s");
                    });

            var circuitBreakerPolicy = Policy<HttpResponseMessage>
                .HandleResult(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(1),
                    onBreak: (outcome, breakDelay) =>
                    {
                        _logger.LogWarning($"[Polly] Circuit OPEN for {breakDelay.TotalSeconds} seconds. StatusCode: {outcome.Result?.StatusCode}");
                    },
                    onReset: () =>
                    {
                        _logger.LogInformation("[Polly] Circuit RESET, calls allowed again.");
                    });

            var fallbackPolicy = Policy<HttpResponseMessage>
                .Handle<BrokenCircuitException>()
                .OrResult(r => r.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .FallbackAsync(
                    fallbackAction: (_, ct) =>
                    {
                        _logger.LogError("[Polly] Fallback triggered! Returning empty JSON []");
                        var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                        {
                            Content = new StringContent("[]", Encoding.UTF8, "application/json")
                        };
                        return Task.FromResult(response);
                    },
                    onFallbackAsync: (res, ct) =>
                    {
                        _logger.LogWarning("[Polly] onFallbackAsync: Fallback response used.");
                        return Task.CompletedTask;
                    }
                );

            _finalPolicy = fallbackPolicy
                .WrapAsync(retryPolicy)
                .WrapAsync(circuitBreakerPolicy);


        }

        public async Task<string> GenerateContentAsync(string title, string language, string level)
        {
            var userMessage = $"Write a very very long story titled '{title}' in {language} language at a {level} level. Return only the story text.";

            var requestBody = new GroqChatRequest(
                Model: _settings.Model,
                Messages: new List<GroqMessage> { new GroqMessage("user", userMessage) }
            );

            using var request = new HttpRequestMessage(HttpMethod.Post, _settings.BaseUrl);
            request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");
            request.Content = JsonContent.Create(requestBody);

            try
            {
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var chatResponse = await response.Content.ReadFromJsonAsync<GroqChatResponse>();
                if (chatResponse?.Choices?.Count > 0)
                {
                    var generated = chatResponse.Choices[0].Message.Content;
                    return generated?.Trim() ?? string.Empty;
                }
                else
                {
                    _logger.LogWarning("GroqLLM boş bir yanıt döndü (GenerateContentAsync)");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GroqLLM isteği sırasında hata oluştu (GenerateContentAsync)");
                return "Error generating content.";
            }
        }

        public async Task<List<string>> GetImageIdeasAsync(string content)
        {
            var prompt =
            $@"You are a helpful assistant. The user has written a story:
            --------------------------------
            {content}
            --------------------------------
            Based on this story, generate 3 to 5 bullet points describing possible images for the story. 
            The first bullet point is the cover image idea, and the rest are additional illustrations. 
            Return ONLY bullet points in the format:
            - idea1
            - idea2
            - idea3
            No other text.";

            var requestBody = new GroqChatRequest(
                Model: _settings.Model,
                Messages: new List<GroqMessage> { new GroqMessage("user", prompt) }
            );

            using var request = new HttpRequestMessage(HttpMethod.Post, _settings.BaseUrl);
            request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");
            request.Content = JsonContent.Create(requestBody);

            try
            {
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var chatResponse = await response.Content.ReadFromJsonAsync<GroqChatResponse>();
                if (chatResponse?.Choices?.Count > 0)
                {
                    var generated = chatResponse.Choices[0].Message.Content;
                    if (string.IsNullOrWhiteSpace(generated))
                        return new List<string>();

                    var lines = generated
                        .Split('\n')
                        .Select(l => l.Trim())
                        .Where(l => l.StartsWith("-"))
                        .Select(l => l.TrimStart('-').Trim())
                        .ToList();

                    return lines;
                }
                else
                {
                    _logger.LogWarning("GroqLLM boş bir yanıt döndü (GetImageIdeasAsync)");
                    return new List<string>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GroqLLM isteği sırasında hata oluştu (GetImageIdeasAsync)");
                return new List<string>();
            }
        }

        public class GeneratedQuestionDto
        {
            public string QuestionType { get; set; }
            public string QuestionText { get; set; }
            public int Score { get; set; }
            public string Answer { get; set; }
            public List<string> Choices { get; set; }
        }

        private async Task<List<Question>> GenerateAllQuestionsAsync(
    int multipleCount,
    int voiceCount,
    int questionCount,
    int wordCount,
    string content,
    string language,
    string level)
        {
            var total = multipleCount + voiceCount + questionCount + wordCount;
            var prompt = $@"
You are an AI teacher.
The user has a story in {language} at {level} level:
------------------------------------------------
{content}
------------------------------------------------
Generate {total} questions in total with these exact counts:
- {multipleCount} of type 'MultipleChoice'
- {voiceCount} of type 'Voice'
- {questionCount} of type 'Question'
- {wordCount} of type 'Word'
Return ONLY a valid JSON array. Each object must have:
- questionType (one of [MultipleChoice, Voice, Question, Word])
- questionText
- score=10
- answer="" (empty)
- choices (array of strings; at least 4 items if MultipleChoice, else empty)

Example:
[
  {{
    ""questionType"": ""MultipleChoice"",
    ""questionText"": ""..."",
    ""score"": 10,
    ""answer"": """",
    ""choices"": [""A"", ""B"", ""C"", ""D""]
  }}
]
No extra text. JSON array only.
";

            var requestBody = new GroqChatRequest(
                Model: _settings.Model,
                Messages: new List<GroqMessage> { new GroqMessage("user", prompt) }
            );

            var questions = new List<Question>();

            using var request = new HttpRequestMessage(HttpMethod.Post, _settings.BaseUrl);
            request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");
            request.Content = JsonContent.Create(requestBody);

            try
            {
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var chatResponse = await response.Content.ReadFromJsonAsync<GroqChatResponse>();
                if (chatResponse?.Choices?.Count > 0)
                {
                    var jsonString = chatResponse.Choices[0].Message.Content;
                    if (!string.IsNullOrWhiteSpace(jsonString))
                    {
                        List<GeneratedQuestionDto>? dtos;
                        try
                        {
                            dtos = JsonSerializer.Deserialize<List<GeneratedQuestionDto>>(
                                jsonString,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        }
                        catch
                        {
                            return questions;
                        }
                        if (dtos != null)
                        {
                            foreach (var dto in dtos)
                            {
                                var t = QuestionTypes.Question;
                                try
                                {
                                    t = (QuestionTypes)Enum.Parse(typeof(QuestionTypes), dto.QuestionType, true);
                                }
                                catch { }
                                questions.Add(new Question
                                {
                                    QuestionText = dto.QuestionText,
                                    QuestionType = t,
                                    Score = dto.Score,
                                    Answer = "",
                                    Choices = dto.Choices ?? new List<string>()
                                });
                            }
                        }
                    }
                }
            }
            catch { }

            return questions;
        }

        public async Task<Test> GenerateTestAsync(Book? book, AppUser? user, string level, string content, string language)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));

            var allQuestions = await GenerateAllQuestionsAsync(
                multipleCount: 2,
                voiceCount: 2,
                questionCount: 3,
                wordCount: 5,
                content: content,
                language: language,
                level: level
            );

            return new Test
            {
                BookId = book.Id,
                UserId = user?.Id,
                Questions = allQuestions,
                TotalScore = 0,
                GeneralFeedback = null 
            };
        }

        //        public async Task<Test> CheckTestAsync(Test test, string content)
        //        {
        //            var questionsText = new StringBuilder();
        //            questionsText.AppendLine("Questions and User Answers:");
        //            foreach (var q in test.Questions)
        //            {
        //                questionsText.AppendLine($"QuestionId={q.Id} Type={q.QuestionType}, Text={q.QuestionText}, UserAnswer={q.Answer}");
        //            }

        //            var prompt = $@"
        //You are an AI teacher.
        //We have a story:
        //-------------------------------------
        //{content}
        //-------------------------------------
        //We have a set of questions with user answers:
        //{questionsText}
        //Please evaluate each question's answer.
        //Rules:
        //1) Score each question on a scale of 0 or 10 (integer).
        //2) Provide a short feedback string for each question (like 'Correct word', 'Wrong word', etc.).
        //3) Also provide a 'generalFeedback' string for the entire test, summarizing performance.
        //4) Return ONLY valid JSON with this format:

        //{{
        //  ""generalFeedback"": ""..."",
        //  ""questions"": [
        //    {{
        //      ""questionId"": ""{Guid.Empty}"",
        //      ""score"": 10,
        //      ""feedback"": ""...""
        //    }},
        //    ...
        //  ]
        //}}

        //No other text. No explanation.
        //";

        //            var requestBody = new GroqChatRequest(
        //                Model: _settings.Model,
        //                Messages: new List<GroqMessage> { new GroqMessage("user", prompt) }
        //            );

        //            var questions = test.Questions;

        //            using var request = new HttpRequestMessage(HttpMethod.Post, _settings.BaseUrl);
        //            request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");
        //            request.Content = JsonContent.Create(requestBody);

        //            try
        //            {
        //                var response = await _httpClient.SendAsync(request);
        //                response.EnsureSuccessStatusCode();

        //                var rawJson = await response.Content.ReadAsStringAsync();
        //                if (string.IsNullOrWhiteSpace(rawJson))
        //                {
        //                    _logger.LogWarning("CheckTestAsync: empty JSON from LLM");
        //                    return test;
        //                }

        //                // Remove backticks and any code block indicators if present
        //                rawJson = rawJson.Trim();
        //                if (rawJson.StartsWith("```"))
        //                {
        //                    var startIdx = rawJson.IndexOf('{');
        //                    if (startIdx >= 0)
        //                    {
        //                        rawJson = rawJson.Substring(startIdx).Trim();
        //                    }
        //                }
        //                while (rawJson.EndsWith("```"))
        //                {
        //                    rawJson = rawJson.Substring(0, rawJson.Length - 3).Trim();
        //                }

        //                // Deserialize to CheckTestResultDto
        //                var dto = JsonSerializer.Deserialize<CheckTestResultDto>(rawJson,
        //                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //                if (dto == null)
        //                {
        //                    _logger.LogWarning("CheckTestAsync: cannot parse LLM JSON");
        //                    return test;
        //                }

        //                test.GeneralFeedback = dto.GeneralFeedback ?? "";

        //                if (dto.Questions != null)
        //                {
        //                    int totalScore = 0;
        //                    foreach (var qRes in dto.Questions)
        //                    {
        //                        // Ensure questionId is a valid GUID
        //                        if (!Guid.TryParse(qRes.QuestionId.ToString(), out Guid parsedQuestionId))
        //                        {
        //                            _logger.LogWarning($"Invalid questionId format: {qRes.QuestionId}");
        //                            continue;
        //                        }

        //                        var q = questions.FirstOrDefault(x => x.Id == parsedQuestionId);
        //                        if (q != null)
        //                        {
        //                            q.Score = qRes.Score;
        //                            q.Feedback = qRes.Feedback ?? "";
        //                            totalScore += q.Score;
        //                        }
        //                        else
        //                        {
        //                            _logger.LogWarning($"Question with ID {parsedQuestionId} not found in test.");
        //                        }
        //                    }
        //                    test.TotalScore = totalScore;
        //                }
        //                else
        //                {
        //                    _logger.LogWarning("CheckTestAsync: 'questions' field is null in LLM response.");
        //                }
        //            }
        //            catch (JsonException ex)
        //            {
        //                _logger.LogError(ex, $"JSON Parse Error: {ex.Message}.");
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error calling LLM in CheckTestAsync");
        //            }

        //            return test;
        //        }
        public async Task<Test> CheckTestAsync(Test test, string content)
        {
            var questionsText = new StringBuilder();
            questionsText.AppendLine("Questions and User Answers:");
            foreach (var q in test.Questions)
            {
                questionsText.AppendLine($"QuestionId={q.Id} Type={q.QuestionType}, Text={q.QuestionText}, UserAnswer={q.Answer}");
            }

            var prompt = $@"
            You are an AI teacher.
            We have a story:
            -------------------------------------
            {content}
            -------------------------------------
            We have a set of questions with user answers:
            {questionsText}
            Please evaluate each question's answer.
            Rules:
            1) Score each question on a scale of 0 or 10 (integer only).
            2) Provide a short feedback string for each question (e.g., 'Correct word', 'Incorrect answer', etc.).
            3) Also provide a 'general_feedback' string summarizing overall performance.
            4) Return ONLY valid JSON in the following format:

            {{
              ""general_feedback"": ""Your overall feedback here."",
              ""questions"": [
                {{ ""questionId"": ""{Guid.Empty}"", ""score"": 10, ""feedback"": ""Your feedback here."" }},
                {{ ""questionId"": ""{Guid.Empty}"", ""score"": 0, ""feedback"": ""Your feedback here."" }},
                {{ ""questionId"": ""{Guid.Empty}"", ""score"": 10, ""feedback"": ""Your feedback here."" }},
                {{ ""questionId"": ""{Guid.Empty}"", ""score"": 10, ""feedback"": ""Your feedback here."" }}
              ]
            }}

            Do not return any additional text, explanations, or formatting outside the JSON object.
            ";

            var requestBody = new GroqChatRequest(
                Model: _settings.Model,
                Messages: new List<GroqMessage> { new GroqMessage("user", prompt) }
            );

            using var request = new HttpRequestMessage(HttpMethod.Post, _settings.BaseUrl);
            request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");
            request.Content = JsonContent.Create(requestBody);

            try
            {
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseContent);
                var root = doc.RootElement;

                var choices = root.GetProperty("choices");
                var messageContent = choices[0].GetProperty("message").GetProperty("content").GetString();

                if (string.IsNullOrWhiteSpace(messageContent))
                    throw new Exception("Invalid response: messageContent is empty.");

                messageContent = messageContent.Trim();

                if (messageContent.StartsWith("```json"))
                    messageContent = messageContent.Substring(7); 

                if (messageContent.EndsWith("```"))
                    messageContent = messageContent.Substring(0, messageContent.Length - 3);
                messageContent = messageContent.Replace("\n", "").Replace("\r", "").Replace("\t", "").Trim();
                messageContent = messageContent.Substring(3);

                string generalFeedbackKey = "\"general_feedback\":";
                int feedbackStartIndex = messageContent.IndexOf(generalFeedbackKey);
                string generalFeedback = "";

                if (feedbackStartIndex != -1)
                {
                    feedbackStartIndex += generalFeedbackKey.Length;
                    int start = messageContent.IndexOf("\"", feedbackStartIndex) + 1;
                    int end = messageContent.IndexOf("\"", start);
                    generalFeedback = messageContent.Substring(start, end - start);
                }

                Console.WriteLine($"General Feedback: {generalFeedback}");

                List<(string questionId, int score, string feedback)> questionsList = new();
                string questionsKey = "\"questions\": [";
                int questionsStartIndex = messageContent.IndexOf(questionsKey);

                if (questionsStartIndex != -1)
                {
                    int currentIndex = questionsStartIndex + questionsKey.Length;

                    while (true)
                    {
                        string questionIdKey = "\"questionId\": \"";
                        string scoreKey = "\"score\": ";
                        string feedbackKey = "\"feedback\": \"";

                        int questionIdStart = messageContent.IndexOf(questionIdKey, currentIndex);
                        if (questionIdStart == -1) break;

                        questionIdStart += questionIdKey.Length;
                        int questionIdEnd = messageContent.IndexOf("\"", questionIdStart);
                        string questionId = messageContent.Substring(questionIdStart, questionIdEnd - questionIdStart);

                        int scoreStart = messageContent.IndexOf(scoreKey, questionIdEnd) + scoreKey.Length;
                        int scoreEnd = messageContent.IndexOf(",", scoreStart);
                        int score = int.Parse(messageContent.Substring(scoreStart, scoreEnd - scoreStart).Trim());

                        int feedbackStart = messageContent.IndexOf(feedbackKey, scoreEnd) + feedbackKey.Length;
                        int feedbackEnd = messageContent.IndexOf("\"", feedbackStart);
                        string feedback = messageContent.Substring(feedbackStart, feedbackEnd - feedbackStart);

                        questionsList.Add((questionId, score, feedback));
                        currentIndex = feedbackEnd;
                    }
                }
                test.GeneralFeedback = generalFeedback;
                test.TotalScore = questionsList.Sum(q => q.score);

                foreach (var question in test.Questions)
                {
                    var evaluatedQuestion = questionsList.FirstOrDefault(q => q.questionId == question.Id.ToString());
                    if (evaluatedQuestion != default)
                    {
                        question.Score = evaluatedQuestion.score;
                        question.Feedback = evaluatedQuestion.feedback;
                    }
                }


                foreach (var (questionId, score, feedback) in questionsList)
                {
                    Console.WriteLine($"QuestionId: {questionId}, Score: {score}, Feedback: {feedback}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing response: {ex.Message}");
            }

            return test;


        }

        public class EvaluationResponse
        {
            [JsonPropertyName("general_feedback")]
            public string GeneralFeedback { get; set; }

            [JsonPropertyName("questions")]
            public List<EvaluatedQuestion> Questions { get; set; }
        }

        public class EvaluatedQuestion
        {
            [JsonPropertyName("questionId")]
            public Guid QuestionId { get; set; }

            [JsonPropertyName("score")]
            public int Score { get; set; }

            [JsonPropertyName("feedback")]
            public string Feedback { get; set; }
        }





    }
}
