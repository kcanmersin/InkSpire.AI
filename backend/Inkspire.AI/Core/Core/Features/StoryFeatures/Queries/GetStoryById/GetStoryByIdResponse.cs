using Core.Data.Entity;

namespace Core.Features.StoryFeatures.Queries.GetStoryById
{
    public class GetStoryByIdResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsPublic { get; set; }
        public int PageCount { get; set; }
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public byte[] CoverImage { get; set; }
        public List<PageDto> Pages { get; set; } = new();  
        public List<CommentDto> Comments { get; set; } = new();
        public List<ReactionDto> Reactions { get; set; } = new();
        public List<StoryImageDto> StoryImages { get; set; } = new();
    }

    public class PageDto  // New DTO for Page
    {
        public Guid Id { get; set; }
        public int PageNumber { get; set; }
        public string Content { get; set; }
    }

    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ReactionDto
    {
        public ReactionType Type { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class StoryImageDto
    {
        public Guid Id { get; set; }
        public byte[] ImageData { get; set; }
        public int Page { get; set; }
    }
}
