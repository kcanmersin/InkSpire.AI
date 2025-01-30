using System.Security.Claims;

namespace Core.Service.JWT
{
    public interface IJwtService
    {
        //string GenerateToken(string email, Guid userId, IEnumerable<string> roles);
        string GenerateToken(string email, string userId, IList<string> roles);

        ClaimsPrincipal? ValidateToken(string token);
    }
}
