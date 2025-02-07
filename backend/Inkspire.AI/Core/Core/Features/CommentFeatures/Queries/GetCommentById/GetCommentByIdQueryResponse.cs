using InkSpire.Core.Data.Entity;
using System;
using System.Collections.Generic;

namespace Core.Features.CommentFeatures.Queries.GetCommentById
{
    public class GetCommentByIdQueryResponse
    {
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public Guid BookId { get; set; }
        public List<Reaction> Reactions { get; set; } = new();
    }
}
