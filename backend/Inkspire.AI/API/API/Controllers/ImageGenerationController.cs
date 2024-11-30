using Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageGenerationController : ControllerBase
    {
        private readonly IImageGenerationService _imageGenerationService;

        public ImageGenerationController(IImageGenerationService imageGenerationService)
        {
            _imageGenerationService = imageGenerationService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateImage([FromBody] ImageGenerationRequest request)
        {
            try
            {
                // Default prompt if not provided
                var prompt = string.IsNullOrWhiteSpace(request.Prompt)
                    ? "A cat"
                    : request.Prompt;

                var imageBytes = await _imageGenerationService.GenerateImageAsync(prompt);

                var base64Image = Convert.ToBase64String(imageBytes);

                return Ok(new
                {
                    Success = true,
                    Prompt = prompt,
                    Base64Image = base64Image
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = ex.Message
                });
            }
        }
    }

    public class ImageGenerationRequest
    {
        public string Prompt { get; set; }
    }
}
