using MediatR;
using System.Collections.Generic;

namespace Core.Features.ReactionFeatures.Queries.GetAllReaction
{
    public class GetAllReactionQuery : IRequest<IEnumerable<GetAllReactionQueryResponse>>
    {
    }
}
