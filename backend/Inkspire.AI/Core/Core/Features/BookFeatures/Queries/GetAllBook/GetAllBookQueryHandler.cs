using Core.Data;
using InkSpire.Core.Data.Entity;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.BookFeatures.Queries.GetAllBook
{
    public class GetAllBookQueryHandler : IRequestHandler<GetAllBookQuery, IEnumerable<GetAllBookQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllBookQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllBookQueryResponse>> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
        {
            var books = await _context.Books
                .Include(b => b.Images)
                .Select(b => new GetAllBookQueryResponse
                {
                    BookId = b.Id,
                    Title = b.Title,
                    AuthorId = b.AuthorId,
                    Language = b.Language,
                    Level = b.Level,
                    Content = b.Content,
                    images = b.Images.FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            return books;
        }
    }
}
