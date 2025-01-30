using Core.Data;
using InkSpire.Core.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace API.GraphQL
{
    public class BookQuery
    {
        public async Task<IEnumerable<Book>> GetBooks([Service] ApplicationDbContext context)
        {
            return await context.Books.Include(b => b.Images).ToListAsync();
        }

        public async Task<Book?> GetBookById(Guid id, [Service] ApplicationDbContext context)
        {
            return await context.Books.Include(b => b.Images).FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
