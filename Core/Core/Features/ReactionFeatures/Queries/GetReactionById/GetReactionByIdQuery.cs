using Core.Shared; 
using MediatR;
using System;

namespace Core.Features.ReactionFeatures.Queries.GetReactionById
{
    public class GetReactionByIdQuery : IRequest<Result<GetReactionByIdQueryResponse>>
    {
        public Guid ReactionId { get; set; }
    }
}
