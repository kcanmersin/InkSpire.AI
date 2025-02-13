using Core.Data.Entity.User;
using Core.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.UserFeatures.GetUserDetails
{
    public class GetUserDetailByIdQueryResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public bool IsTwoFactorEnabled { get; set; } = false;
        public string NativeLanguage { get; set; } = string.Empty;
        public string TargetLanguage { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
    }

    public class GetUserDetailByIdQuery : IRequest<Result<GetUserDetailByIdQueryResponse>>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserDetailByIdHandler : IRequestHandler<GetUserDetailByIdQuery, Result<GetUserDetailByIdQueryResponse>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserDetailByIdHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<GetUserDetailByIdQueryResponse>> Handle(GetUserDetailByIdQuery request, CancellationToken cancellationToken)
        { 

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                return Result.Failure<GetUserDetailByIdQueryResponse>(
                    new Error("UserNotFound", "User not found."));
            }

            return Result.Success(new GetUserDetailByIdQueryResponse
            {
                Name = user.Name,
                Surname = user.Surname,
                IsTwoFactorEnabled = user.IsTwoFactorEnabled,
                NativeLanguage = user.NativeLanguage,
                TargetLanguage = user.TargetLanguage,
                ProfileImageUrl = user.ProfileImageUrl
            });
        }
    }
}
