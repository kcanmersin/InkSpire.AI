using Core.Data;
using InkSpire.Core.Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReactionFeatures.Queries.GetAllReaction
{
    public class GetAllReactionQueryHandler : IRequestHandler<GetAllReactionQuery, IEnumerable<GetAllReactionQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllReactionQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllReactionQueryResponse>> Handle(GetAllReactionQuery request, CancellationToken cancellationToken)
        {
            var reactions = await _context.Reactions
                .Select(r => new GetAllReactionQueryResponse
                {
                    ReactionId = r.Id,
                    UserId = r.UserId,
                    ReactionType = r.ReactionType,
                    BookId = r.BookId,
                    CommentId = r.CommentId
                })
                .ToListAsync(cancellationToken);

            return reactions;
        }
    }
}
