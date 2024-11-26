using System;
using System.Collections.Generic;

namespace API.Contracts.Story
{
    public class GetStoryByIdResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<string> Content { get; set; } = new();
        public bool IsPublic { get; set; }
        public int PageCount { get; set; }
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public byte[] CoverImage { get; set; }
        public List<CommentDto> Comments { get; set; } = new();
        public List<ReactionDto> Reactions { get; set; } = new();

        public class CommentDto
        {
            public Guid Id { get; set; }
            public string Content { get; set; }
            public string CreatedByName { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public class ReactionDto
        {
            public string Type { get; set; }
            public string CreatedByName { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}
