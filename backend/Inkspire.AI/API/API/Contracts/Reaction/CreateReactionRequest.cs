using Core.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Reaction
{
    public class CreateReactionRequest
    {
        [Required]
        public ReactionType Type { get; set; }

        public Guid? StoryId { get; set; }

        public Guid? CommentId { get; set; }

        [Required]
        public Guid CreatedById { get; set; }
    }
}
