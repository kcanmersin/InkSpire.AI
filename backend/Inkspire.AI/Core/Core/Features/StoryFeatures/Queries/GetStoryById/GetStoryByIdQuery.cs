using Core.Shared;
using MediatR;

namespace Core.Features.StoryFeatures.Queries.GetStoryById
{
    public class GetStoryByIdQuery : IRequest<Result<GetStoryByIdResponse>>
    {
        public Guid Id { get; set; }
    }
}
