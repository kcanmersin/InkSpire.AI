using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Comment
{
    public class CreateCommentRequest
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public Guid StoryId { get; set; }

        [Required]
        public Guid CreatedById { get; set; }
    }
}
