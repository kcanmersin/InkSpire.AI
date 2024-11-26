namespace Core.Features.CommentFeatures.Queries.GetAllComments
{
    public class GetAllCommentsResponse
    {
        public List<CommentDto> Comments { get; set; } = new();
    }

    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid StoryId { get; set; }
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
