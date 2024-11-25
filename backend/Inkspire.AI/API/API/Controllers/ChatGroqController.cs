using Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/groq")]
public class GroqController : ControllerBase
{
    private readonly IChatGroqService _chatGroqService;

    public GroqController(IChatGroqService chatGroqService)
    {
        _chatGroqService = chatGroqService;
    }

    [HttpPost("generate-story")]
    public async Task<IActionResult> GenerateStory(string title, string description, int pageCount)
    {
        try
        {
            var response = await _chatGroqService.GenerateStoryAsync(title, description, pageCount);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
