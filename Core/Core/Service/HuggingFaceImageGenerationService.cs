using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using InkSpire.Application.Abstractions;

namespace Core.Services
{
    public class HuggingFaceSettings
    {
        public string ApiKey { get; set; }
        public string Model { get; set; } = "stabilityai/stable-diffusion-3.5-large"; //dahaa hızlı olanı bul

    }

    public class HuggingFaceImageGenerationService : IImageGenerationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HuggingFaceImageGenerationService> _logger;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly string _apiUrl;

        public HuggingFaceImageGenerationService(
            HttpClient httpClient,
            ILogger<HuggingFaceImageGenerationService> logger,
            IOptions<HuggingFaceSettings> settings)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = settings.Value.ApiKey;
            _model = settings.Value.Model;
            _apiUrl = $"models/{_model}";
        }

        public async Task<byte[]> GenerateImageAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                throw new ArgumentException("Prompt cannot be null or empty.", nameof(prompt));

            try
            {
                var requestBody = new { inputs = prompt };
                var jsonPayload = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl)
                {
                    Content = content
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Image generation failed: {response.StatusCode} - {errorMessage}");
                }

                var responseContent = await response.Content.ReadAsByteArrayAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GenerateImageAsync");
                throw;
            }
        }
    }
}

