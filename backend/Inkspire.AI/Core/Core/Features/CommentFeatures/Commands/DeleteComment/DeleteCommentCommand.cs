using MediatR;

namespace Core.Features.CommentFeatures.Commands.DeleteComment
{
    public class DeleteCommentCommand : IRequest<DeleteCommentResponse>
    {
        public Guid Id { get; set; }
        public Guid DeletedById { get; set; }
    }
}
