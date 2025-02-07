using System;

namespace API.Contracts.Comment
{
    public class CreateCommentRequest
    {
        public Guid UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public Guid BookId { get; set; }
    }
}
