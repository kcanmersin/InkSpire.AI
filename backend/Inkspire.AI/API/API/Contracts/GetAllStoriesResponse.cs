using System;
using System.Collections.Generic;

namespace API.Contracts.Story
{
    public class GetAllStoriesResponse
    {
        public List<StoryDto> Stories { get; set; } = new();

        public class StoryDto
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public int PageCount { get; set; }
            public bool IsPublic { get; set; }
            public Guid CreatedById { get; set; }
            public string CreatedByName { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}
