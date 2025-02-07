using Core.Data.Entity.EntityBases;
using System;

namespace InkSpire.Core.Data.Entity
{
    public class Reaction : EntityBase
    {
        public Guid Id { get;  set; }
        public Guid UserId { get;  set; }
        public ReactionType ReactionType { get;  set; }

        public Guid? BookId { get;  set; }
        public Book Book { get;  set; }

        public Guid? CommentId { get;  set; }
        public Comment Comment { get;  set; }


    }
}
