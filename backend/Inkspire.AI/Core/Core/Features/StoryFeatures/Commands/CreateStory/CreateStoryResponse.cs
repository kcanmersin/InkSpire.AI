using System;

namespace Core.Features.StoryFeatures.Commands.CreateStory
{
    public class CreateStoryResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int PageCount { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
