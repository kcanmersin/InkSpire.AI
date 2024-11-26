using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Comment
{
    public class UpdateCommentRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public Guid StoryId { get; set; }

        [Required]
        public Guid CreatedById { get; set; }
    }
}
