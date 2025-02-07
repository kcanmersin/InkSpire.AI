using System.ComponentModel;

namespace API.Contracts.Book
{
    public class CreateBookRequest
    {
        [System.ComponentModel.DefaultValue("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7")]
        public Guid AuthorId { get; set; }
        [System.ComponentModel.DefaultValue("Rabbit and car")]
        public string Title { get; set; }
        [System.ComponentModel.DefaultValue("English")]
        public string Language { get; set; } 
        [System.ComponentModel.DefaultValue("Beginner")]
        public string Level { get; set; } 
    }
}
