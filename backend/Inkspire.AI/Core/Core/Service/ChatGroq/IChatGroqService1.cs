namespace Core.Services
{
    public interface IChatGroqService
    {
        //Task<string> GenerateStoryAsync(string title, string description, int pageCount);
        Task<string> GenerateStoryAsyncFull(string title, string description, int pageCount = 5);
        Task<object> FormatGeneratedStoryAsync(string title, string description, int pageCount = 5);

    }

}
