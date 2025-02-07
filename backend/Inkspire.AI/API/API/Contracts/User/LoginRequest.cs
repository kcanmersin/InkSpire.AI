using System.ComponentModel;

namespace API.Contracts.User
{
    public class LoginRequest
    {
        [System.ComponentModel.DefaultValue("kcanmersin@gmail.com")]
        public string Email { get; set; } = string.Empty;
        [System.ComponentModel.DefaultValue("19071907")]
        public string Password { get; set; } = string.Empty;
    }

    
}
