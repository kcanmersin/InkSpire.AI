using API.Contracts.Reaction;
using Core.Features.ReactionFeatures.Commands.CreateReaction;
using Core.Features.ReactionFeatures.Commands.DeleteReaction;
using Core.Features.ReactionFeatures.Queries.GetAllReactions;
using Core.Features.ReactionFeatures.Queries.GetReactionById;
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

        [HttpPost]
        public async Task<IActionResult> CreateReaction([FromBody] CreateReactionRequest request)
        {
            var command = new CreateReactionCommand
            {
                Type = request.Type,
                StoryId = request.StoryId,
                CommentId = request.CommentId,
                CreatedById = request.CreatedById
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteReaction([FromRoute] Guid id)
        {
            var command = new DeleteReactionCommand { Id = id };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetReactionById([FromRoute] Guid id)
        {
            var query = new GetReactionByIdQuery { Id = id };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReactions([FromQuery] Guid? storyId = null, [FromQuery] Guid? commentId = null)
        {
            var query = new GetAllReactionsQuery
            {
                StoryId = storyId,
                CommentId = commentId
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
