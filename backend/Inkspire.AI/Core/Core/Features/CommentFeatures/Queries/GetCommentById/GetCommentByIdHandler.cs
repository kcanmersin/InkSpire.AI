using Core.Data;
using Core.Features.CommentFeatures.Queries.GetCommentById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.CommentFeatures.Queries.GetCommentById
{
    public class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, GetCommentByIdResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetCommentByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetCommentByIdResponse> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments
                .Include(c => c.CreatedBy) 
                .AsNoTracking()
                .Where(c => !c.IsDeleted && c.Id == request.Id) 
                .Select(c => new GetCommentByIdResponse
                {
                    Id = c.Id,
                    Content = c.Content,
                    StoryId = c.StoryId,
                    CreatedById = c.CreatedById,
                    CreatedByName = $"{c.CreatedBy.Name} {c.CreatedBy.Surname}",
                    CreatedDate = c.CreatedDate
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (comment == null)
            {
                throw new Exception("Comment not found.");
            }

            return comment;
        }
    }
}
