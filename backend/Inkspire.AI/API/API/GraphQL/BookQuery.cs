using Core.Data;
using InkSpire.Core.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace API.GraphQL
{
    public class BookQuery
    {
        public async Task<IEnumerable<BookDto>> GetBooks(
            [Service] ApplicationDbContext context,
            int limit = 10,
            int offset = 0,
            List<string>? languages = null,
            List<string>? levels = null)
        {
            var query = context.Books.Include(b => b.Images).AsQueryable();

            if (languages != null && languages.Any())
                query = query.Where(b => languages.Contains(b.Language));

            if (levels != null && levels.Any())
                query = query.Where(b => levels.Contains(b.Level));

            return await query
                .Skip(offset)
                .Take(limit)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Language = b.Language,
                    Level = b.Level,
                    Images = b.Images.Take(1)
                        .Select(i => new BookImageDto { ImageData = Convert.ToBase64String(i.ImageData) })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<BookDto?> GetBookById(
            [Service] ApplicationDbContext context,
            Guid id)
        {
            var book = await context.Books
                .Include(b => b.Images)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return null;

            var author = await context.Users
                .Where(u => u.Id == book.AuthorId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname
                })
                .FirstOrDefaultAsync();

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Language = book.Language,
                Level = book.Level,
                Content = book.Content,
                Author = author,
                Images = book.Images
                    .Select(i => new BookImageDto { ImageData = Convert.ToBase64String(i.ImageData) })
                    .ToList()
            };
        }

        public async Task<List<CommentDto>> GetCommentsByBookId(
            [Service] ApplicationDbContext context,
            Guid bookId)
        {
            var comments = await context.Comments
                .Where(c => c.BookId == bookId)
                .Include(c => c.Reactions)
                .ToListAsync();

            if (!comments.Any()) return new List<CommentDto>();

            var userDictionary = await context.Users
                .Where(u => comments.Select(c => c.UserId).Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname
                });

            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Text = c.Text,
                User = userDictionary.ContainsKey(c.UserId) ? userDictionary[c.UserId] : null,
                Reactions = c.Reactions.Select(r => new ReactionDto
                {
                    UserId = r.UserId,
                    ReactionType = r.ReactionType
                }).ToList()
            }).ToList();
        }

        public async Task<List<ReactionDto>> GetReactionsByBookId(
            [Service] ApplicationDbContext context,
            Guid bookId)
        {
            var reactions = await context.Reactions
                .Where(r => r.BookId == bookId)
                .ToListAsync();

            return reactions.Select(r => new ReactionDto
            {
                UserId = r.UserId,
                ReactionType = r.ReactionType
            }).ToList();
        }
    }
}
