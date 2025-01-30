using Core.Data;
using Core.Shared;
using InkSpire.Core.Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReactionFeatures.Queries.GetReactionById
{
    public class GetReactionByIdQueryHandler : IRequestHandler<GetReactionByIdQuery, Result<GetReactionByIdQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetReactionByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetReactionByIdQueryResponse>> Handle(GetReactionByIdQuery request, CancellationToken cancellationToken)
        {
            var reaction = await _context.Reactions
                .Where(r => r.Id == request.ReactionId)
                .Select(r => new GetReactionByIdQueryResponse
                {
                    ReactionId = r.Id,
                    UserId = r.UserId,
                    ReactionType = r.ReactionType,
                    BookId = r.BookId,
                    CommentId = r.CommentId
                })
                .FirstOrDefaultAsync(cancellationToken);

           var response = new GetReactionByIdQueryResponse
           {
               UserId = reaction.UserId,
               ReactionType = reaction.ReactionType,
               BookId = reaction.BookId,
               CommentId = reaction.CommentId
           };
            return Result.Success(response);


        }
    }
}
