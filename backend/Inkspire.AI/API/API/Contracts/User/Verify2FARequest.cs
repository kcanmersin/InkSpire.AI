using System.ComponentModel;

namespace API.Contracts.User
{
    public class Verify2FARequest
    {
        [System.ComponentModel.DefaultValue("6c9f755a-de96-4e9e-991a-22b15a003572")]
        public Guid UserId { get; set; }
        public string TwoFactorCode { get; set; }
    }
}
