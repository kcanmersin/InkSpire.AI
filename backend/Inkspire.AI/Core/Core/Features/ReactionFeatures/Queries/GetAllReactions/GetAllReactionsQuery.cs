using MediatR;

namespace Core.Features.ReactionFeatures.Queries.GetAllReactions
{
    public class GetAllReactionsQuery : IRequest<GetAllReactionsResponse>
    {
        public Guid? StoryId { get; set; }
        public Guid? CommentId { get; set; }
    }
}
