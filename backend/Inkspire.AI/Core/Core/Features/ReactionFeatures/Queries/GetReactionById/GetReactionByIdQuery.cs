using MediatR;

namespace Core.Features.ReactionFeatures.Queries.GetReactionById
{
    public class GetReactionByIdQuery : IRequest<GetReactionByIdResponse>
    {
        public Guid Id { get; set; }
    }
}
