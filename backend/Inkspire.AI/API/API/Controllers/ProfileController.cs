using Core.Service.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly LocalStorageService _localStorageService;
        private readonly CloudinaryStorageService _cloudinaryStorageService;

        public ProfileController(LocalStorageService localStorageService, CloudinaryStorageService cloudinaryStorageService)
        {
            _localStorageService = localStorageService;
            _cloudinaryStorageService = cloudinaryStorageService;
        }

        [HttpPost("upload-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfileImage([FromForm] UploadProfileImageRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("Dosya boş olamaz.");
            }

            try
            {
                var localFilePath = await _localStorageService.UploadFileAsync(request.UserId, request.File);

                var cloudinaryUrl = await _cloudinaryStorageService.UploadFileFromLocalAsync(request.UserId, localFilePath);

                return Ok(new { Url = cloudinaryUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Dosya yüklenirken bir hata oluştu.", Error = ex.Message });
            }
        }
    }
}
