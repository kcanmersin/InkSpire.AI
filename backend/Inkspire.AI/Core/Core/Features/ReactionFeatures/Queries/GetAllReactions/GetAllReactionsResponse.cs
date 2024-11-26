using Core.Data.Entity;

namespace Core.Features.ReactionFeatures.Queries.GetAllReactions
{
    public class GetAllReactionsResponse
    {
        public List<ReactionDto> Reactions { get; set; } = new();
    }

    public class ReactionDto
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
