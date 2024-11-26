using MediatR;

namespace Core.Features.CommentFeatures.Queries.GetCommentById
{
    public class GetCommentByIdQuery : IRequest<GetCommentByIdResponse>
    {
        public Guid Id { get; set; }
    }
}
