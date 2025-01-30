using FluentValidation;

namespace Core.Features.BookFeatures.Commands.CreateBook
{
    public class CreateBookValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookValidator()
        {
            RuleFor(x => x.AuthorId)
                .NotEmpty().WithMessage("AuthorId is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Language is required.");

            RuleFor(x => x.Level)
                .NotEmpty().WithMessage("Level is required.");
        }
    }
}
