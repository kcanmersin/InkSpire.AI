using Core.Data;
using Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ChatGroqService : IChatGroqService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
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
            "llama3-groq-8b-8192-tool-use-preview",
        };

        private readonly ApplicationDbContext _context;

        public ChatGroqService(HttpClient httpClient, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
            _apiKey = Environment.GetEnvironmentVariable("INKSPIRE_CHATGROQ_API_KEY");
            _baseUrl = "https://api.groq.com/openai/v1/chat/completions";
        }

        public async Task<object> FormatGeneratedStoryAsync(string title, string description, int pageCount = 5)
        {
            try
            {
                Guid userId = Guid.Parse("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7");

                var rawStory = await GenerateStoryAsyncFull(title, description, pageCount);

                var placeholderCoverImage = Encoding.UTF8.GetBytes("Placeholder cover image data");

                var placeholderStoryImages = new List<StoryImage>
        {
            new StoryImage
            {
                Id = Guid.NewGuid(),
                ImageData = Encoding.UTF8.GetBytes("Placeholder story image 1"),
                CreatedDate = DateTime.UtcNow
            },
            new StoryImage
            {
                Id = Guid.NewGuid(),
                ImageData = Encoding.UTF8.GetBytes("Placeholder story image 2"),
                CreatedDate = DateTime.UtcNow
            }
        };

                var story = new Story
                {
                    Title = title,
                    IsPublic = true,
                    PageCount = pageCount,
                    CreatedById = userId,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = null,
                    CoverImage = placeholderCoverImage,
                    StoryImages = placeholderStoryImages,
                    Comments = new List<Comment>(),
                    Reactions = new List<Reaction>(),
                    Pages = new List<Page>()
                };

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
                        story.Pages.Add(new Page
                        {
                            PageNumber = pageNumber,
                            Content = pageText,
                            Story = story,
                            CreatedDate = DateTime.UtcNow
                        });
                    }
                }

                await _context.Stories.AddAsync(story);
                await _context.SaveChangesAsync();

                return story;
            }
            catch (Exception ex)
            {
                return new
                {
                    Error = "An error occurred while formatting the story.",
                    Details = ex.InnerException?.Message ?? ex.Message
                };
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

                await Task.Delay(1000);
            }

            storyContent.AppendLine("The End.");
            return storyContent.ToString();
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
