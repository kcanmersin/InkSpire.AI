using MediatR;

namespace Core.Features.ReactionFeatures.Commands.DeleteReaction
{
    public class DeleteReactionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
