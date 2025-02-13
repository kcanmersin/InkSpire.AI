using Core.Data.Entity.Game;
using Core.Features.GameFeatures.CheckGameAnswer;
using Core.Features.GameFeatures.GenerateGameTopic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/game")]
public class GameController : ControllerBase
{
    private readonly IMediator _mediator;

    public GameController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("active-rooms")]
    public ActionResult<List<GameRoom>> GetActiveRooms()
    {
        return Ok(GameHub.ActiveRooms.Values.ToList());
    }

    [HttpGet("generate-topic")]
    public async Task<IActionResult> GenerateGameTopic([FromQuery] string language)
    {
        var result = await _mediator.Send(new GenerateGameTopicQuery { Language = language });

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("check-answer")]
    public async Task<IActionResult> CheckGameAnswer([FromBody] CheckGameAnswerQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
