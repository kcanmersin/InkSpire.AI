using Core.Data.Entity.User;
using Core.Service.JWT;
using Core.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.UserFeatures.Verify2FA
{
    public class Verify2FAHandler : IRequestHandler<Verify2FACommand, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<Verify2FAHandler> _logger;

        public Verify2FAHandler(UserManager<AppUser> userManager, IJwtService jwtService, ILogger<Verify2FAHandler> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(Verify2FACommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Result.Failure<string>(new Error("InvalidUser", "User not found."));
            }

            if (!user.TwoFactorEnabled)
            {
                return Result.Failure<string>(new Error("TwoFactorNotEnabled", "Two-factor authentication is not enabled for this user."));
            }

            if (user.TwoFactorCode != request.TwoFactorCode || user.TwoFactorExpiryTime < DateTime.UtcNow)
            {
                return Result.Failure<string>(new Error("InvalidCode", "Invalid or expired two-factor authentication code."));
            }

            user.TwoFactorCode = string.Empty;
            user.TwoFactorExpiryTime = null;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.Email, user.Id.ToString(), roles);

            return Result.Success(token);
        }
    }
}
