using Core.Data;
using Core.Features.StoryFeatures.Queries.GetStoryById;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.StoryFeatures.Queries.GetAllStories
{
    public class GetAllStoriesHandler : IRequestHandler<GetAllStoriesQuery, Result<GetAllStoriesResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllStoriesHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAllStoriesResponse>> Handle(GetAllStoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Stories.AsQueryable();

            if (request.IsPublic.HasValue)
                query = query.Where(s => s.IsPublic == request.IsPublic.Value);

            if (request.CreatedById.HasValue)
                query = query.Where(s => s.CreatedById == request.CreatedById.Value);

            if (!string.IsNullOrWhiteSpace(request.TitleContains))
                query = query.Where(s => s.Title.Contains(request.TitleContains));

            var stories = await query
                .Include(s => s.CreatedBy)
                .Include(s => s.Pages)  
                .Select(s => new StoryDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    PageCount = s.PageCount,
                    IsPublic = s.IsPublic,
                    CreatedById = s.CreatedById,
                    CreatedByName = $"{s.CreatedBy.Name} {s.CreatedBy.Surname}",
                    CreatedDate = s.CreatedDate,
                    CoverImage = s.CoverImage,
                    //Pages = s.Pages.Select(p => new PageDto 
                    //{
                    //    Id = p.Id,
                    //    PageNumber = p.PageNumber,
                    //    Content = p.Content
                    //}).ToList()
                })
                .ToListAsync(cancellationToken);

            return Result.Success(new GetAllStoriesResponse { Stories = stories });
        }
    }
}
