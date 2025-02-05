using API.Contracts.Reaction;
using Core.Features.ReactionFeatures.Commands.CreateReaction;
using Core.Features.ReactionFeatures.Queries.GetAllReaction;
using Core.Features.ReactionFeatures.Queries.GetReactionById;
using Core.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateReaction([FromBody] CreateReactionRequest request)
        {
            var command = new CreateReactionCommand
            {
                CommentId = request.CommentId,
                UserId = request.UserId,
                ReactionType = request.ReactionType,
                BookId = request.BookId

            };
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            //return CreatedAtAction(nameof(CreateReaction),
            //    new { id = result.Value.ReactionId },
            //    new
            //    {
            //        ReactionId = result.Value.ReactionId,
            //        UserId = result.Value.UserId,
            //        ReactionType = result.Value.ReactionType
            //    });
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReactions()
        {
            var query = new GetAllReactionQuery();
            var reactions = await _mediator.Send(query);
            return Ok(reactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReactionById(Guid id)
        {
            var query = new GetReactionByIdQuery { ReactionId = id };
            var reaction = await _mediator.Send(query);
            return Ok(reaction);
        }
    }
}
