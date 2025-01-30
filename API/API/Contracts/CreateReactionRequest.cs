using InkSpire.Core.Data.Entity;
using System;

namespace API.Contracts.Reaction
{
    public class CreateReactionRequest
    {
        public Guid UserId { get; set; }
        public ReactionType ReactionType { get; set; }
        public Guid? BookId { get; set; }
        public Guid? CommentId { get; set; }
    }
}
