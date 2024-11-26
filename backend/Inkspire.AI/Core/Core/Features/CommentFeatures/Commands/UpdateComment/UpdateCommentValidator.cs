using FluentValidation;

namespace Core.Features.CommentFeatures.Commands.UpdateComment
{
    public class UpdateCommentValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(500).WithMessage("Content must be at most 500 characters.");

            RuleFor(x => x.UpdatedById)
                .NotEmpty().WithMessage("UpdatedById is required.")
                .NotEqual(Guid.Empty).WithMessage("UpdatedById must be a valid GUID.");
        }
    }
}
