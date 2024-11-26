using Core.Data;
using Core.Features.ReactionFeatures.Queries.GetReactionById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.ReactionFeatures.Queries.GetReactionById
{
    public class GetReactionByIdHandler : IRequestHandler<GetReactionByIdQuery, GetReactionByIdResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetReactionByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetReactionByIdResponse> Handle(GetReactionByIdQuery request, CancellationToken cancellationToken)
        {
            var reaction = await _context.Reactions
                .Include(r => r.CreatedBy)
                .Where(r => r.Id == request.Id)
                .Select(r => new GetReactionByIdResponse
                {
                    Id = r.Id,
                    Type = r.Type,
                    StoryId = r.StoryId,
                    CommentId = r.CommentId,
                    CreatedById = r.CreatedById,
                    CreatedByName = $"{r.CreatedBy.Name} {r.CreatedBy.Surname}",
                    CreatedDate = r.CreatedDate
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (reaction == null) throw new Exception("Reaction not found.");

            return reaction;
        }
    }
}
