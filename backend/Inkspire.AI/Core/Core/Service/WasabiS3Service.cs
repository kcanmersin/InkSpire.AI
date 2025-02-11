using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

public class WasabiS3Service
{
    private readonly string _bucketName;
    private readonly string _accessKey;
    private readonly string _secretKey;
    private readonly string _serviceUrl;
    private readonly IAmazonS3 _s3Client;

    public WasabiS3Service(IConfiguration configuration)
    {
        _bucketName = configuration["WasabiS3:BucketName"];
        _accessKey = configuration["WasabiS3:AccessKey"];
        _secretKey = configuration["WasabiS3:SecretKey"];
        _serviceUrl = configuration["WasabiS3:ServiceUrl"];

        _s3Client = new AmazonS3Client(_accessKey, _secretKey, new AmazonS3Config
        {
            ServiceURL = _serviceUrl,
            ForcePathStyle = true
        });
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Dosya boş olamaz.");

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = $"profile-images/{fileName}";

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = filePath,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead
            };

            await _s3Client.PutObjectAsync(request);
        }

        return $"{_serviceUrl}/{_bucketName}/{filePath}";
    }
}
