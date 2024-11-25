using Core.Data.Entity.EntityBases;
using Core.Data.Entity.User;
using System;

namespace Core.Data.Entity
{
    public class Reaction : EntityBase
    {
        public ReactionType Type { get; set; } 
        public Guid? StoryId { get; set; }
        public Story Story { get; set; }
        public Guid? CommentId { get; set; }
        public Comment Comment { get; set; }
        public Guid CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }
    }
}
