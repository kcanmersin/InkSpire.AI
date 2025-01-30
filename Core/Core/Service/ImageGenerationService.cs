using InkSpire.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace InkSpire.Infrastructure.Services
{
    public class ImageGenerationService : IImageGenerationService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://api.stability.ai/v1/generation/stable-diffusion-v1-6/text-to-image";
        private const string ApiKey = "sk-vhuGGBpx5dJCAlxkvdJCSXNAsUuyMriaDaiYg6l88RI47HVE"; 

        public ImageGenerationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> GenerateImageAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                throw new ArgumentException("Prompt cannot be null or empty.", nameof(prompt));

            var payload = new
            {
                text_prompts = new[] { new { text = prompt, weight = 1.0 } },
                cfg_scale = 7,
                height = 512,
                width = 512,
                samples = 1,
                steps = 30
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl)
            {
                Content = content
            };
            request.Headers.Add("Authorization", $"Bearer {ApiKey}");
            request.Headers.Add("Accept", "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Image generation failed: {response.StatusCode} - {errorMessage}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<ImageResponse>(responseContent);

            var base64Image = responseObject?.Artifacts?[0]?.Base64;
            if (string.IsNullOrEmpty(base64Image))
                throw new Exception("Failed to generate image: No image data returned.");

            return Convert.FromBase64String(base64Image);
        }

        private class ImageResponse
        {
            [JsonPropertyName("artifacts")]
            public List<Artifact> Artifacts { get; set; }
        }

        private class Artifact
        {
            [JsonPropertyName("base64")]
            public string Base64 { get; set; }

            [JsonPropertyName("seed")]
            public long Seed { get; set; }

            [JsonPropertyName("finishReason")]
            public string FinishReason { get; set; }
        }
    }
}
