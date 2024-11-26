using Core.Data.Entity;

namespace Core.Features.ReactionFeatures.Commands.CreateReaction
{
    public class CreateReactionResponse
    {
        public Guid Id { get; set; }
        public ReactionType Type { get; set; }
        public Guid? StoryId { get; set; }
        public Guid? CommentId { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
