using Core.Data;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.StoryFeatures.Queries.GetStoryById
{
    public class GetStoryByIdHandler : IRequestHandler<GetStoryByIdQuery, Result<GetStoryByIdResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetStoryByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetStoryByIdResponse>> Handle(GetStoryByIdQuery request, CancellationToken cancellationToken)
        {
            var story = await _context.Stories
                .Include(s => s.Comments)
                    .ThenInclude(c => c.CreatedBy)
                .Include(s => s.Reactions)
                    .ThenInclude(r => r.CreatedBy)
                .Include(s => s.StoryImages)
                .Include(s => s.Pages)  
                .Include(s => s.CreatedBy)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (story == null)
                return Result.Failure<GetStoryByIdResponse>(new Error("StoryNotFound", "The story could not be found."));

            var response = new GetStoryByIdResponse
            {
                Id = story.Id,
                Title = story.Title,
                IsPublic = story.IsPublic,
                PageCount = story.PageCount,
                CreatedById = story.CreatedById,
                CreatedByName = $"{story.CreatedBy.Name} {story.CreatedBy.Surname}",
                CoverImage = story.CoverImage,
                Pages = story.Pages.Select(p => new PageDto  
                {
                    Id = p.Id,
                    PageNumber = p.PageNumber,
                    Content = p.Content
                }).ToList(),
                Comments = story.Comments.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedByName = $"{c.CreatedBy.Name} {c.CreatedBy.Surname}",
                    CreatedDate = c.CreatedDate
                }).ToList(),
                Reactions = story.Reactions.Select(r => new ReactionDto
                {
                    Type = r.Type,
                    CreatedByName = $"{r.CreatedBy.Name} {r.CreatedBy.Surname}",
                    CreatedDate = r.CreatedDate
                }).ToList(),
                StoryImages = story.StoryImages.Select(si => new StoryImageDto
                {
                    Id = si.Id,
                    ImageData = si.ImageData,
                    Page = si.Page
                }).ToList() // Map StoryImages
            };

            return Result.Success(response);
        }
    }
}
