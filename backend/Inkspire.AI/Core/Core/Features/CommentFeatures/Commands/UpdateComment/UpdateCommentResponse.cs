namespace Core.Features.CommentFeatures.Commands.UpdateComment
{
    public class UpdateCommentResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
