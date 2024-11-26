using MediatR;

namespace Core.Features.CommentFeatures.Commands.UpdateComment
{
    public class UpdateCommentCommand : IRequest<UpdateCommentResponse>
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid UpdatedById { get; set; }
    }
}
