using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Service.Storage
{
    public class WasabiS3StorageService
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<WasabiS3StorageService> _logger;

        public WasabiS3StorageService(IConfiguration configuration, ILogger<WasabiS3StorageService> logger)
        {
            _bucketName = configuration["WasabiS3:BucketName"];
            var accessKey = configuration["WasabiS3:AccessKey"];
            var secretKey = configuration["WasabiS3:SecretKey"];
            var serviceUrl = configuration["WasabiS3:ServiceUrl"];

            _s3Client = new AmazonS3Client(accessKey, secretKey, new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true
            });

            _logger = logger;
        }

        public async Task<string> UploadFileFromLocalAsync(Guid userId, string localFilePath)
        {
            if (!File.Exists(localFilePath))
            {
                throw new FileNotFoundException("Dosya bulunamadı", localFilePath);
            }

            var fileName = Path.GetFileName(localFilePath);
            var s3FilePath = $"profile-images/{userId}/{fileName}";

            try
            {
                using (var stream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = s3FilePath,
                        InputStream = stream,
                        ContentType = "image/jpeg",
                        CannedACL = S3CannedACL.PublicRead
                    };

                    await _s3Client.PutObjectAsync(request);
                }

                var fileUrl = $"{_s3Client.Config.ServiceURL}/{_bucketName}/{s3FilePath}";
                _logger.LogInformation("Dosya Wasabi'ye yüklendi: {FileUrl}", fileUrl);

                return fileUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wasabi S3 yükleme hatası");
                throw;
            }
        }
    }
}
