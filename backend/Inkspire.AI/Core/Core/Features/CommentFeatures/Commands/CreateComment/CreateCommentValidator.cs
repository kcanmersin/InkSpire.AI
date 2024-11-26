using FluentValidation;

namespace Core.Features.CommentFeatures.Commands.CreateComment
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(500).WithMessage("Content must be at most 500 characters.");

            RuleFor(x => x.StoryId)
                .NotEmpty().WithMessage("StoryId is required.")
                .NotEqual(Guid.Empty).WithMessage("StoryId must be a valid GUID.");

            RuleFor(x => x.CreatedById)
                .NotEmpty().WithMessage("CreatedById is required.")
                .NotEqual(Guid.Empty).WithMessage("CreatedById must be a valid GUID.");
        }
    }
}
