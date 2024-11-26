using Core.Data.Entity;
using MediatR;

namespace Core.Features.ReactionFeatures.Commands.CreateReaction
{
    public class CreateReactionCommand : IRequest<CreateReactionResponse>
    {
        public ReactionType Type { get; set; }
        public Guid? StoryId { get; set; }
        public Guid? CommentId { get; set; }
        public Guid CreatedById { get; set; }
    }
}
