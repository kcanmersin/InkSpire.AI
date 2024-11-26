using Core.Data;
using FluentValidation;
using MediatR;

namespace Core.Features.CommentFeatures.Commands.DeleteComment
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, DeleteCommentResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<DeleteCommentCommand> _validator;

        public DeleteCommentHandler(ApplicationDbContext context, IValidator<DeleteCommentCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<DeleteCommentResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
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

            comment.IsDeleted = true;
            comment.DeletedBy = request.DeletedById.ToString();
            comment.DeletedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteCommentResponse
            {
                Id = comment.Id,
                DeletedById = request.DeletedById
            };
        }
    }
}
