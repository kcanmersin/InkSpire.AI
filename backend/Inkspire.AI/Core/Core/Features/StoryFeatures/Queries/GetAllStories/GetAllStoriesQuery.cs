using Core.Shared;
using MediatR;

namespace Core.Features.StoryFeatures.Queries.GetAllStories
{
    public class GetAllStoriesQuery : IRequest<Result<GetAllStoriesResponse>>
    {
        public bool? IsPublic { get; set; }
        public Guid? CreatedById { get; set; }
        public string? TitleContains { get; set; }
    }
}
