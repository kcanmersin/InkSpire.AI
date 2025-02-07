namespace Core.Features.BookFeatures.Commands.CreateBook
{
    public class CreateBookResponse
    {
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
