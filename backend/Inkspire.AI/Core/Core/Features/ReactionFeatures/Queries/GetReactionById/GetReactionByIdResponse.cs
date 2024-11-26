using Core.Data.Entity;

namespace Core.Features.ReactionFeatures.Queries.GetReactionById
{
    public class GetReactionByIdResponse
    {
        public Guid Id { get; set; }
        public ReactionType Type { get; set; }
        public Guid? StoryId { get; set; }
        public Guid? CommentId { get; set; }
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
