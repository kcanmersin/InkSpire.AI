using MediatR;

public class BookCreatedEvent : INotification
{
    public Guid BookId { get; set; }  
    public string Title { get; set; } 

    public BookCreatedEvent() { } 
    public BookCreatedEvent(Guid bookId, string title)
    {
        BookId = bookId;
        Title = title;
    }
}
