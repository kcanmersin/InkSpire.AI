using Core.Data;
using Core.Data.Entity;
using FluentValidation;
using MediatR;

namespace Core.Features.CommentFeatures.Commands.CreateComment
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CreateCommentResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreateCommentCommand> _validator;

        public CreateCommentHandler(ApplicationDbContext context, IValidator<CreateCommentCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<CreateCommentResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            // Validate the request
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new Exception(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var comment = new Comment
            {
                Content = request.Content,
                StoryId = request.StoryId,
                CreatedById = request.CreatedById,
                CreatedDate = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateCommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                StoryId = comment.StoryId,
                CreatedById = comment.CreatedById,
                CreatedDate = comment.CreatedDate
            };
        }
    }
}
