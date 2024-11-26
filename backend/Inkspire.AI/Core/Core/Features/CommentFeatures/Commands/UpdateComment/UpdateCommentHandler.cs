using Core.Data;
using FluentValidation;
using MediatR;

namespace Core.Features.CommentFeatures.Commands.UpdateComment
{
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, UpdateCommentResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<UpdateCommentCommand> _validator;

        public UpdateCommentHandler(ApplicationDbContext context, IValidator<UpdateCommentCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<UpdateCommentResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            // Validate the request
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new Exception(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var comment = await _context.Comments.FindAsync(request.Id);

            if (comment == null)
            {
                throw new Exception("Comment not found.");
            }

            comment.Content = request.Content;
            comment.ModifiedBy = request.UpdatedById.ToString();
            comment.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateCommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                ModifiedById = request.UpdatedById,
                ModifiedDate = comment.ModifiedDate
            };
        }
    }
}
