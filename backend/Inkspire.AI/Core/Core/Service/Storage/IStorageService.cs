using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Core.Service.Storage
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(Guid userId, IFormFile file);
    }
}
