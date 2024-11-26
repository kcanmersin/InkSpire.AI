using Core.Shared;
using MediatR;

namespace Core.Features.StoryFeatures.Commands.CreateStory
{
    public class CreateStoryCommand : IRequest<Result<CreateStoryResponse>>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; } = 5; 
        public Guid UserId { get; set; }
    }
}
