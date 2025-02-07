namespace API.Contracts.User
{
    public class UpdateUserRequest
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string NativeLanguage { get; set; } = string.Empty;
        public string TargetLanguage { get; set; } = string.Empty;
    }
}