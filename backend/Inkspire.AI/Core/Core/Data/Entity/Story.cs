using Core.Data.Entity.EntityBases;
using Core.Data.Entity.User;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace Core.Data.Entity
{
    public class Story : EntityBase
    {
        public string Title { get; set; } = string.Empty;

        public ICollection<Page> Pages { get; set; } = new List<Page>();

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
