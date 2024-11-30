using Core.Data;
using Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ChatGroqService : IChatGroqService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly IImageGenerationService _imageGenerationService;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.groq.com/openai/v1/chat/completions";

        private readonly List<string> _models = new()
        {
            "llama-3.1-70b-versatile",
            "llama-3.1-8b-instant",
            "llama-3.2-3b-preview",
            "llama-3.2-11b-vision-preview",
            "llama-3.2-90b-vision-preview",
            "llama-guard-3-8b",
            "llama3-70b-8192",
            "llama3-8b-8192",
            "gemma2-9b-it",
            "gemma-7b-it",
            "llama3-groq-70b-8192-tool-use-preview",
            "llama3-groq-8b-8192-tool-use-preview"
        };

        public ChatGroqService(HttpClient httpClient, ApplicationDbContext context, IImageGenerationService imageGenerationService)
        {
            _httpClient = httpClient;
            _context = context;
            _imageGenerationService = imageGenerationService;
            _apiKey = Environment.GetEnvironmentVariable("INKSPIRE_CHATGROQ_API_KEY");
        }

        public async Task<List<Page>> FormatGeneratedStoryAsync(string title, string description, int pageCount = 5)
        {
            try
            {
                // Generate raw story content
                var rawStory = await GenerateStoryAsyncFull(title, description, pageCount);

                var pages = new List<Page>();
                string[] pageContents = rawStory.Split(new[] { "\nPage " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var pageContent in pageContents)
                {
                    int pageNumber = 0;
                    string pageText = string.Empty;

                    var firstColonIndex = pageContent.IndexOf(":");
                    if (firstColonIndex >= 0 && int.TryParse(pageContent.Substring(0, firstColonIndex).Replace("Page ", "").Trim(), out pageNumber))
                    {
                        pageText = pageContent.Substring(firstColonIndex + 1).Trim();
                    }

                    if (pageNumber > 0 && !string.IsNullOrWhiteSpace(pageText))
                    {
                        pages.Add(new Page
                        {
                            PageNumber = pageNumber,
                            Content = pageText,
                            CreatedDate = DateTime.UtcNow
                        });
                    }
                }

                return pages;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while formatting the story.", ex);
            }
        }



        public async Task<string> GenerateStoryAsyncFull(string title, string description, int pageCount = 5)
        {
            var systemMessage = $@"
You are a professional storyteller AI.
Generate a complete story based on the given details:
- Title: {title}
- Description: {description}
- Number of Pages: {pageCount}
- Each page should be approximately 300-500 words.
- The story must flow logically across all pages and end conclusively.
- Avoid phrases like 'to be continued.'";

            var storyContent = new StringBuilder();
            string previousPageContent = string.Empty;

            for (int page = 1; page <= pageCount; page++)
            {
                var model = _models[(page - 1) % _models.Count];

                var pagePrompt = $@"
Write content for page {page} of the story.
The story so far:
{previousPageContent}

Page {page}:";

                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new { role = "system", content = systemMessage },
                        new { role = "user", content = $"Title: {title}\nDescription: {description}\n{pagePrompt}" }
                    },
                    max_tokens = 1000,
                    temperature = 0.7,
                    top_p = 0.95
                };

                var json = JsonSerializer.Serialize(requestBody);
                var contentMessage = new StringContent(json, Encoding.UTF8, "application/json");

                using var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl)
                {
                    Content = contentMessage
                };
                request.Headers.Add("Authorization", $"Bearer {_apiKey}");

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error: {response.StatusCode}, Details: {errorDetails}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<ChatCompletionResponse>(
                    responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                string pageContent = responseObject?.Choices?.FirstOrDefault()?.Message?.Content ?? string.Empty;

                if (string.IsNullOrEmpty(pageContent))
                {
                    throw new Exception($"Page {page} content generation failed.");
                }

                storyContent.AppendLine($"Page {page}:\n{pageContent}\n");
                previousPageContent += $"Page {page}:\n{pageContent}\n";
            }

            return storyContent.ToString().Trim();
        }
    }

    public class ChatCompletionResponse
    {
        public List<ChatChoice> Choices { get; set; }
    }

    public class ChatChoice
    {
        public ChatMessage Message { get; set; }
    }

    public class ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
