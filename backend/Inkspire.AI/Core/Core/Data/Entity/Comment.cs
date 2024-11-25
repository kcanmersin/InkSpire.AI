using Core.Data.Entity.EntityBases;
using Core.Data.Entity.User;
using System;
using System.Collections.Generic;

namespace Core.Data.Entity
{
    public class Comment : EntityBase
    {
        public string Content { get; set; } = string.Empty;
        public Guid StoryId { get; set; }
        public Story Story { get; set; }
        public Guid CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    }
}
