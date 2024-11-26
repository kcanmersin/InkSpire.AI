using MediatR;

namespace Core.Features.CommentFeatures.Commands.CreateComment
{
    public class CreateCommentCommand : IRequest<CreateCommentResponse>
    {
        public string Content { get; set; }
        public Guid StoryId { get; set; }
        public Guid CreatedById { get; set; }
    }
}
