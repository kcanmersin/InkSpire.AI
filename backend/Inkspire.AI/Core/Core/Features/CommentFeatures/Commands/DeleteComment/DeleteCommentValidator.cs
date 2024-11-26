using FluentValidation;

namespace Core.Features.CommentFeatures.Commands.DeleteComment
{
    public class DeleteCommentValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID.");

            RuleFor(x => x.DeletedById)
                .NotEmpty().WithMessage("DeletedById is required.")
                .NotEqual(Guid.Empty).WithMessage("DeletedById must be a valid GUID.");
        }
    }
}
