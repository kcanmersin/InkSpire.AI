using FluentValidation;

namespace Core.Features.ReactionFeatures.Commands.CreateReaction
{
    public class CreateReactionValidator : AbstractValidator<CreateReactionCommand>
    {
        public CreateReactionValidator()
        {
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.CreatedById).NotEmpty();
            RuleFor(x => x).Must(x => x.StoryId.HasValue || x.CommentId.HasValue)
                .WithMessage("A reaction must be associated with either a story or a comment.");
        }
    }
}
