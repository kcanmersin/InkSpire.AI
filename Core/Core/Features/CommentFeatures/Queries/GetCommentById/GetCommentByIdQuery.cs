using Core.Shared;
using MediatR;
using System;

namespace Core.Features.CommentFeatures.Queries.GetCommentById
{
    public class GetCommentByIdQuery : IRequest<Result<GetCommentByIdQueryResponse>>
    {
        public Guid CommentId { get; set; }
    }
}
