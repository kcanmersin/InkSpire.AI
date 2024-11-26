namespace Core.Features.CommentFeatures.Queries.GetCommentById
{
    public class GetCommentByIdResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid StoryId { get; set; }
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
