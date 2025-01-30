using Core.Data;
using InkSpire.Core.Data.Entity;

namespace API.GraphQL
{
    public class BookMutation
    {
        public async Task<Book> CreateBook(
            string title,
            string content,
            string language,
            string level,
            Guid authorId,
            [Service] ApplicationDbContext context)
        {
            var book = new Book(authorId, title, content, language, level);
            context.Books.Add(book);
            await context.SaveChangesAsync();
            return book;
        }
    }
}
