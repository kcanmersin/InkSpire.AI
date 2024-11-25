namespace Core.Services
{
    public interface IChatGroqService
    {
        Task<string> GenerateStoryAsync(string title, string description, int pageCount);
    }
}
