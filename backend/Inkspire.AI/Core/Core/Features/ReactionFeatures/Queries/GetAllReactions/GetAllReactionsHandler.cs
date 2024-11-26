using Core.Data;
using Core.Features.ReactionFeatures.Queries.GetAllReactions;
using Core.Features.StoryFeatures.Queries.GetStoryById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.ReactionFeatures.Queries.GetAllReactions
{
    public class GetAllReactionsHandler : IRequestHandler<GetAllReactionsQuery, GetAllReactionsResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetAllReactionsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetAllReactionsResponse> Handle(GetAllReactionsQuery request, CancellationToken cancellationToken)
        {
            var reactionsQuery = _context.Reactions.AsQueryable();

            if (request.StoryId.HasValue)
                reactionsQuery = reactionsQuery.Where(r => r.StoryId == request.StoryId.Value);
            if (request.CommentId.HasValue)
                reactionsQuery = reactionsQuery.Where(r => r.CommentId == request.CommentId.Value);

            var reactions = await reactionsQuery
                .Include(r => r.CreatedBy)
                .Select(r => new ReactionDto
                {
                    Id = r.Id,
                    Type = r.Type,
                    StoryId = r.StoryId,
                    CommentId = r.CommentId,
                    CreatedById = r.CreatedById,
                    CreatedByName = $"{r.CreatedBy.Name} {r.CreatedBy.Surname}",
                    CreatedDate = r.CreatedDate
                })
                .ToListAsync(cancellationToken);

            return new GetAllReactionsResponse { Reactions = reactions };
        }
    }
}
