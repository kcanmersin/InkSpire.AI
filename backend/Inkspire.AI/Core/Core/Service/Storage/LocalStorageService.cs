using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Service.Storage
{
    public class LocalStorageService
    {
        private readonly ILogger<LocalStorageService> _logger;

        public LocalStorageService(ILogger<LocalStorageService> logger)
        {
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(Guid userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("Dosya boş olamaz.");
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile-images", userId.ToString());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation("Dosya yerel olarak kaydedildi: {FilePath}", filePath);

            return filePath; 
        }
    }
}
