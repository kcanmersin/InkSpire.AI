using Core.Data;
using Core.Shared;
using InkSpire.Core.Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.CommentFeatures.Queries.GetCommentById
{
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, Result<GetCommentByIdQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetCommentByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetCommentByIdQueryResponse>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments
                .Include(c => c.Reactions)
                .Where(c => c.Id == request.CommentId)
                .Select(c => new GetCommentByIdQueryResponse
                {
                    CommentId = c.Id,
                    UserId = c.UserId,
                    Text = c.Text,
                    BookId = c.BookId,
                    Reactions = c.Reactions.ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);


            return Result<GetCommentByIdQueryResponse>.Success(comment);
        }
    }
}
