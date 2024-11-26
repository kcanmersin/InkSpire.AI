using MediatR;

namespace Core.Features.CommentFeatures.Queries.GetAllComments
{
    public class GetAllCommentsQuery : IRequest<GetAllCommentsResponse>
    {
        public Guid? StoryId { get; set; } 
    }
}
