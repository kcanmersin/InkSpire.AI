using Core.Data;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.BookFeatures.Queries.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Result<GetBookByIdQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetBookByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetBookByIdQueryResponse>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _context.Books
                .Include(b => b.Images)
                .Where(b => b.Id == request.BookId)
                .Select(b => new GetBookByIdQueryResponse
                {
                    BookId = b.Id,
                    Title = b.Title,
                    AuthorId = b.AuthorId,
                    Language = b.Language,
                    Level = b.Level,
                    Content = b.Content,
                    images = b.Images.ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);


            return book;
        }
    }
}
