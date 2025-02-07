using InkSpire.Core.Data.Entity;
using System;

namespace Core.Features.ReactionFeatures.Queries.GetAllReaction
{
    public class GetAllReactionQueryResponse
    {
        public Guid ReactionId { get; set; }
        public Guid UserId { get; set; }
        public ReactionType ReactionType { get; set; }
        public Guid? BookId { get; set; }
        public Guid? CommentId { get; set; }
    }
}
