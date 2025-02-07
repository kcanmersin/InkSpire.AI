using Core.Data.Entity.User;
using Core.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.UserFeatures.Update
{
    public class UpdateUserCommand : IRequest<Result<Unit>>
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? NativeLanguage { get; set; }
        public string? TargetLanguage { get; set; }
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<Unit>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<UpdateUserCommand> _validator;

        public UpdateUserHandler(UserManager<AppUser> userManager, IValidator<UpdateUserCommand> validator)
        {
            _userManager = userManager;
            _validator = validator;
        }

        public async Task<Result<Unit>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<Unit>(new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null)
            {
                return Result.Failure<Unit>(new Error("UserNotFound", "User not found."));
            }

            bool isUpdated = false;

            if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != user.Name)
            {
                user.Name = request.Name;
                isUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Surname) && request.Surname != user.Surname)
            {
                user.Surname = request.Surname;
                isUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(request.NativeLanguage) && request.NativeLanguage != user.NativeLanguage)
            {
                user.NativeLanguage = request.NativeLanguage;
                isUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(request.TargetLanguage) && request.TargetLanguage != user.TargetLanguage)
            {
                user.TargetLanguage = request.TargetLanguage;
                isUpdated = true;
            }

            if (!isUpdated)
            {
                return Result.Success(Unit.Value); // Değişiklik yapılmadığı için direkt başarı döndür
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                var errors = updateResult.Errors.Select(e => e.Description).ToList();
                return Result.Failure<Unit>(new Error("UpdateFailed", string.Join("; ", errors)));
            }

            return Result.Success(Unit.Value);
        }
    }

    public class UpdateUserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
    }

    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId cannot be empty.");

            RuleFor(x => x.Name)
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters.");

            RuleFor(x => x.Surname)
                .MaximumLength(50)
                .WithMessage("Surname must not exceed 50 characters.");

            RuleFor(x => x.NativeLanguage)
                .MaximumLength(50)
                .WithMessage("Native language must not exceed 50 characters.");

            RuleFor(x => x.TargetLanguage)
                .MaximumLength(50)
                .WithMessage("Target language must not exceed 50 characters.");
        }
    }
}
