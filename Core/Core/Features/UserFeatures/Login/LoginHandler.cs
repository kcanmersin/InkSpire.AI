using Core.Data.Entity.User;
using Core.Service.Email;
using Core.Service.JWT;
using Core.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.UserFeatures.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IValidator<LoginCommand> _validator;
        private readonly IEmailService _emailService;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IJwtService jwtService,
            IValidator<LoginCommand> validator,
            IEmailService emailService,
            ILogger<LoginHandler> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _validator = validator;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<LoginResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure<LoginResponse>(new Error("InvalidCredentials", "Invalid email or password."));
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded)
            {
                return Result.Failure<LoginResponse>(new Error("InvalidCredentials", "Invalid email or password."));
            }

            if (user.TwoFactorEnabled)
            {
                var twoFactorCode = GenerateTwoFactorCode();
                user.TwoFactorCode = twoFactorCode;
                user.TwoFactorExpiryTime = DateTime.UtcNow.AddMinutes(10);
                await _userManager.UpdateAsync(user);

                var subject = "Your 2FA Code";
                var body = $"Your two-factor authentication code is: {twoFactorCode}";

                try
                {
                    await _emailService.SendEmailAsync(user.Email, subject, body);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending 2FA email to user {UserId}", user.Id);
                    return Result.Failure<LoginResponse>(new Error("EmailError", "Failed to send 2FA code."));
                }

                return Result.Success(new LoginResponse
                {
                    RequiresTwoFactor = true,
                    Email = user.Email,
                    Name = user.Name,
                    Surname = user.Surname,
                    UserId = user.Id
                });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.Email, user.Id.ToString(), roles);

            return Result.Success(new LoginResponse
            {
                Token = token,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname
            });
        }

        private string GenerateTwoFactorCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
