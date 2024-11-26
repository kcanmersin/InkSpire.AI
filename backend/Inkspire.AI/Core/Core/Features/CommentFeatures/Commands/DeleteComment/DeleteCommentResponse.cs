namespace Core.Features.CommentFeatures.Commands.DeleteComment
{
    public class DeleteCommentResponse
    {
        public Guid Id { get; set; }
        public Guid DeletedById { get; set; }
    }
}
