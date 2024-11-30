using System.Threading.Tasks;

namespace Core.Services
{
    public interface IImageGenerationService
    {
        Task<byte[]> GenerateImageAsync(string prompt);
    }
}
