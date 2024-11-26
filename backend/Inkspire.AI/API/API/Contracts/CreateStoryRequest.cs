using System;
using System.ComponentModel;

namespace API.Contracts.Story
{
    public class CreateStoryRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; } = 5;
        [DefaultValue("F7F7F7F7-F7F7-F7F7-F7F7-F7F7F7F7F7F7")]
        public Guid UserId { get; set; }
    }
}
