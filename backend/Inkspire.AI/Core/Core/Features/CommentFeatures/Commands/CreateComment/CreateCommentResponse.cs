namespace Core.Features.CommentFeatures.Commands.CreateComment
{
    public class CreateCommentResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid StoryId { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
