using Core.Data.Entity.EntityBases;
using System;
using System.Collections.Generic;

namespace InkSpire.Core.Data.Entity
{
    public class Comment : EntityBase
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public List<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}
