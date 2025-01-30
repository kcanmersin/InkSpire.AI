using Core.Shared;
using MediatR;
using System;

namespace Core.Features.UserFeatures.Verify2FA
{
    public class Verify2FACommand : IRequest<Result<string>> 
    {
        public Guid UserId { get; set; }
        public string TwoFactorCode { get; set; }
    }
}
