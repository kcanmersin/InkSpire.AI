using Core.Data.Entity.EntityBases;
using Core.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entity
{
    public class Story : EntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = true; 
        public int PageCount { get; set; }
        public Guid CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }

        public byte[] CoverImage { get; set; }

        public ICollection<StoryImage> StoryImages { get; set; } = new List<StoryImage>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    }
}
