using Core.Data;
using Core.Data.Entity;
using Core.Services;
using Core.Shared;
using FluentValidation;
using MediatR;
using System.Text.Json;

namespace Core.Features.StoryFeatures.Commands.CreateStory
{
    public class CreateStoryHandler : IRequestHandler<CreateStoryCommand, Result<CreateStoryResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IChatGroqService _chatGroqService;
        private readonly IImageGenerationService _imageGenerationService;
        private readonly IValidator<CreateStoryCommand> _validator;

        public CreateStoryHandler(
            ApplicationDbContext context,
            IChatGroqService chatGroqService,
            IImageGenerationService imageGenerationService,
            IValidator<CreateStoryCommand> validator)
        {
            _context = context;
            _chatGroqService = chatGroqService;
            _imageGenerationService = imageGenerationService;
            _validator = validator;
        }

        public async Task<Result<CreateStoryResponse>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
        {
            // Validate the request
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<CreateStoryResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            try
            {
                // Format the story text into pages
                var formattedPages = await _chatGroqService.FormatGeneratedStoryAsync(request.Title, request.Description, request.PageCount);

                var story = new Story
                {
                    Title = request.Title,
                    IsPublic = true,
                    PageCount = request.PageCount,
                    CreatedById = request.UserId,
                    CreatedDate = DateTime.UtcNow,
                    CoverImage = null, // Will be set later if images are generated
                    StoryImages = new List<StoryImage>(),
                    Comments = new List<Comment>(),
                    Reactions = new List<Reaction>(),
                    Pages = new List<Page>(formattedPages)
                };
                int count = 0;

                // Generate images for every 4th page
                foreach (var page in formattedPages.Where(p => p.PageNumber % 4 == 0))
                {
                    var trimmedText = page.Content.Length > 1900 ? page.Content.Substring(0, 1900) : page.Content;
                    var imageBytes = await _imageGenerationService.GenerateImageAsync(trimmedText);

                    var storyImage = new StoryImage
                    {
                        Id = Guid.NewGuid(),
                        ImageData = imageBytes,
                        Story = story,
                        CreatedDate = DateTime.UtcNow,
                        //page number
                        Page=count++,

                    };

                    story.StoryImages.Add(storyImage);
                }

                // Assign the first image as the cover image
                if (story.StoryImages.Any())
                {
                    story.CoverImage = story.StoryImages.First().ImageData;
                }

                // Save the story to the database
                await _context.Stories.AddAsync(story, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var response = new CreateStoryResponse
                {
                    Id = story.Id,
                    Title = story.Title,
                    PageCount = story.PageCount,
                    IsPublic = story.IsPublic,
                    CreatedDate = story.CreatedDate
                };

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<CreateStoryResponse>(
                    new Error("StoryCreationFailed", $"An error occurred while creating the story: {ex.Message}"));
            }
        }


        private class GeneratedStoryResponse
        {
            public string Title { get; set; }
            public List<string> Pages { get; set; } = new();
        }
    }
}
