using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Data.Entity.User;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Service.Storage
{
    public class CloudinaryStorageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryStorageService> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly HttpClient _httpClient;
        private readonly string _cloudName;
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public CloudinaryStorageService(IConfiguration configuration, ILogger<CloudinaryStorageService> logger, UserManager<AppUser> userManager, IServiceScopeFactory serviceScopeFactory)
        {
            _cloudName = configuration["Cloudinary:CloudName"];
            _apiKey = configuration["Cloudinary:ApiKey"];
            _apiSecret = configuration["Cloudinary:ApiSecret"];

            Account account = new Account(_cloudName, _apiKey, _apiSecret);
            _cloudinary = new Cloudinary(account);

            _logger = logger;
            _userManager = userManager;
            _serviceScopeFactory = serviceScopeFactory;
            _httpClient = new HttpClient();
        }

        public async Task<string> UploadFileFromLocalAsync(Guid userId, string localFilePath)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }

                var fileName = Path.GetFileName(localFilePath);
                var cloudinaryFolder = $"profile-images/{userId}";

                try
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(localFilePath),
                        Folder = cloudinaryFolder,
                        PublicId = Path.GetFileNameWithoutExtension(fileName),
                        UseFilename = true,
                        UniqueFilename = false,
                        Overwrite = true
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    var newCloudinaryUrl = uploadResult.SecureUrl.AbsoluteUri;

                    if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                    {
                        await DeleteOldImagesAsync(userId, newCloudinaryUrl);
                    }

                    user.ProfileImageUrl = newCloudinaryUrl;
                    user.SecurityStamp = Guid.NewGuid().ToString();
                    await userManager.UpdateAsync(user);

                    _logger.LogInformation("Yeni resim yüklendi ve eski resimler silindi: {FileUrl}", newCloudinaryUrl);
                    return newCloudinaryUrl;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Cloudinary yükleme hatası");
                    throw;
                }
            }
        }

        public async Task DeleteOldImagesAsync(Guid userId, string newImageUrl)
        {
            try
            {
                var searchUrl = $"https://api.cloudinary.com/v1_1/{_cloudName}/resources/search";
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_apiKey}:{_apiSecret}"));

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                var requestBody = new
                {
                    expression = $"folder:profile-images/{userId}",
                    max_results = 100
                };

                var jsonBody = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(searchUrl, jsonBody);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                var searchResult = JsonSerializer.Deserialize<CloudinarySearchResponse>(responseString);
                var publicIds = searchResult.Resources
                    .Where(r => r.SecureUrl != newImageUrl)
                    .Select(r => r.PublicId)
                    .ToList();

                if (publicIds.Any())
                {
                    var deleteUrl = $"https://api.cloudinary.com/v1_1/{_cloudName}/resources/image/upload";
                    var deleteBody = new
                    {
                        public_ids = publicIds
                    };

                    var deleteJson = new StringContent(JsonSerializer.Serialize(deleteBody), Encoding.UTF8, "application/json");
                    var deleteResponse = await _httpClient.PostAsync(deleteUrl, deleteJson);
                    var deleteResponseString = await deleteResponse.Content.ReadAsStringAsync();

                    if (!deleteResponse.IsSuccessStatusCode)
                    {
                        _logger.LogError("Cloudinary silme işlemi başarısız: {Response}", deleteResponseString);
                    }
                    else
                    {
                        _logger.LogInformation("Eski profil resimleri silindi: {DeletedFiles}", string.Join(", ", publicIds));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eski Cloudinary resimleri silerken hata oluştu.");
            }
        }
    }

    public class CloudinarySearchResponse
    {
        public List<CloudinaryResource> Resources { get; set; }
    }

    public class CloudinaryResource
    {
        public string PublicId { get; set; }
        public string SecureUrl { get; set; }
    }
}
