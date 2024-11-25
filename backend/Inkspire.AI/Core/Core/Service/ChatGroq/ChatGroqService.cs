using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ChatGroqService : IChatGroqService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public ChatGroqService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("INKSPIRE_CHATGROQ_API_KEY");
            _baseUrl = "https://api.groq.com/openai/v1/chat/completions";
        }

        public async Task<string> GenerateStoryAsync(string title, string description, int pageCount)
        {
            var systemMessage = @"
                You are a professional storyteller AI.
                Generate a story with the following details:
                - Title: {title}
                - Description: {description}
                - Number of Pages: {pageCount}
                - Each page should be approximately 300-500 words.
                - The story must flow logically across all pages.
                - The response should be formatted as follows:

                Title: {title}

                Page 1:
                [Content for page 1]

                Page 2:
                [Content for page 2]

                ...
            ";

            var userMessage = $"Title: {title}\nDescription: {description}\nNumber of Pages: {pageCount}";

            var requestBody = new
            {
                model = "llama-3.1-70b-versatile",
                messages = new[]
                {
                    new { role = "system", content = systemMessage },
                    new { role = "user", content = userMessage }
                },
                max_tokens = 2000,
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

            return await response.Content.ReadAsStringAsync();
        }
    }
}
