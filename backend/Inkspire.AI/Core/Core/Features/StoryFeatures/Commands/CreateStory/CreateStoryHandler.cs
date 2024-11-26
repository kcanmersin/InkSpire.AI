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
        private readonly IValidator<CreateStoryCommand> _validator;

        public CreateStoryHandler(
            ApplicationDbContext context,
            IChatGroqService chatGroqService,
            IValidator<CreateStoryCommand> validator)
        {
            _context = context;
            _chatGroqService = chatGroqService;
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
                // Generate the story using ChatGroqService
                var storyData = await _chatGroqService.FormatGeneratedStoryAsync(request.Title, request.Description, request.PageCount);

                var formattedStory = JsonSerializer.Deserialize<GeneratedStoryResponse>(storyData.ToString());
                if (formattedStory == null)
                {
                    return Result.Failure<CreateStoryResponse>(
                        new Error("FormattingFailed", "Failed to parse the story formatting response."));
                }

                var story = new Story
                {
                    Title = formattedStory.Title,
                    IsPublic = true, 
                    PageCount = request.PageCount,
                    CreatedById = request.UserId,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Stories.Add(story);
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
