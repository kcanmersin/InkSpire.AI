namespace Core.Features.StoryFeatures.Queries.GetAllStories
{
    public class GetAllStoriesResponse
    {
        public List<StoryDto> Stories { get; set; } = new();
    }

    public class StoryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int PageCount { get; set; }
        public bool IsPublic { get; set; }
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte[] CoverImage { get; set; }

        public List<PageDto> Pages { get; set; } = new();
    }

    public class PageDto
    {
        public Guid Id { get; set; }
        public int PageNumber { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
