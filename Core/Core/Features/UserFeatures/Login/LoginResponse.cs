namespace Core.Features.UserFeatures.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public bool RequiresTwoFactor { get; set; } = false;
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Guid UserId { get; set; } 
    }
}
