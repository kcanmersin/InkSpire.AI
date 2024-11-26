using Core.Data;
using Core.Features.CommentFeatures.Queries.GetAllComments;
using Core.Features.StoryFeatures.Queries.GetStoryById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.CommentFeatures.Queries.GetAllComments
{
    public class GetAllCommentsHandler : IRequestHandler<GetAllCommentsQuery, GetAllCommentsResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetAllCommentsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetAllCommentsResponse> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            var commentsQuery = _context.Comments.AsQueryable();

            if (request.StoryId.HasValue)
            {
                commentsQuery = commentsQuery.Where(c => c.StoryId == request.StoryId.Value);
            }

            var comments = await commentsQuery
                .Include(c => c.CreatedBy) 
                .Where(c => !c.IsDeleted) 
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    StoryId = c.StoryId,
                    CreatedById = c.CreatedById,
                    CreatedByName = $"{c.CreatedBy.Name} {c.CreatedBy.Surname}",
                    CreatedDate = c.CreatedDate
                })
                .ToListAsync(cancellationToken);

            return new GetAllCommentsResponse
            {
                Comments = comments
            };
        }
    }
}
