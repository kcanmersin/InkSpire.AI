using InkSpire.Core.Data.Entity;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Language { get; set; }
    public string Level { get; set; }
    public string Content { get; set; }
    public List<BookImageDto> Images { get; set; } = new();
    public List<CommentDto> Comments { get; set; } = new();
    public List<ReactionDto> Reactions { get; set; } = new();
    public UserDto Author { get; set; }
}

public class CommentDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public List<ReactionDto> Reactions { get; set; } = new();
    public UserDto User { get; set; } 
}

public class ReactionDto
{
    public Guid UserId { get; set; }
    public ReactionType ReactionType { get; set; }
}

public class BookImageDto
{
    public string ImageData { get; set; }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}
