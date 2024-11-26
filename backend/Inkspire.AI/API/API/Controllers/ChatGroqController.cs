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
    [HttpPost("generate-formatted-story")]
    public async Task<IActionResult> GenerateFormattedStory(
     [FromQuery] string title = "animal",
     [FromQuery] string description = "fable",
     [FromQuery] int pageCount = 5)
    {
        if (pageCount <= 0)
        {
            return BadRequest(new { error = "Page count must be greater than 0." });
        }

        try
        {
            var formattedStory = await _chatGroqService.FormatGeneratedStoryAsync(title, description, pageCount);
            return Ok(formattedStory);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpPost("generate-story-full")]
    public async Task<IActionResult> GenerateStoryFull(
        [FromQuery] string title = "animal",
        [FromQuery] string description = "fable",
        [FromQuery] int pageCount = 5) 
    {
        if (pageCount <= 0)
        {
            return BadRequest(new { error = "Page count must be greater than 0." });
        }

        try
        {
            var story = await _chatGroqService.GenerateStoryAsyncFull(title, description, pageCount);
            return Ok(new { title, story });
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    //[HttpPost("generate-story")]
    //public async Task<IActionResult> GenerateStory(string title, string description, int pageCount)
    //{
    //    try
    //    {
    //        var response = await _chatGroqService.GenerateStoryAsync(title, description, pageCount);
    //        return Ok(response);
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(new { error = ex.Message });
    //    }
    //}

}
