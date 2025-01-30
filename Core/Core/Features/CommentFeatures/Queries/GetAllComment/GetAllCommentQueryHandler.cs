using Core.Data;
using InkSpire.Core.Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.CommentFeatures.Queries.GetAllComment
{
    public class GetAllCommentQueryHandler : IRequestHandler<GetAllCommentQuery, IEnumerable<GetAllCommentQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllCommentQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllCommentQueryResponse>> Handle(GetAllCommentQuery request, CancellationToken cancellationToken)
        {
            var comments = await _context.Comments
                .Include(c => c.Reactions)    
                .Select(c => new GetAllCommentQueryResponse
                {
                    CommentId = c.Id,
                    UserId = c.UserId,
                    Text = c.Text,
                    BookId = c.BookId,
                    Reactions = c.Reactions.ToList()
                })
                .ToListAsync(cancellationToken);

            return comments;
        }
    }
}
