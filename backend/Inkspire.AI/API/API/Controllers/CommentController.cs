using API.Contracts.Comment;
using Core.Features.CommentFeatures.Commands.CreateComment;
using Core.Features.CommentFeatures.Commands.UpdateComment;
using Core.Features.CommentFeatures.Commands.DeleteComment;
using Core.Features.CommentFeatures.Queries.GetAllComments;
using Core.Features.CommentFeatures.Queries.GetCommentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            var command = new CreateCommentCommand
            {
                Content = request.Content,
                StoryId = request.StoryId,
                CreatedById = request.CreatedById
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentRequest request)
        {
            var command = new UpdateCommentCommand
            {
                Id = request.Id,
                Content = request.Content,
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
        {
            var command = new DeleteCommentCommand { Id = id };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCommentById([FromRoute] Guid id)
        {
            var query = new GetCommentByIdQuery { Id = id };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments([FromQuery] Guid? storyId = null)
        {
            var query = new GetAllCommentsQuery { StoryId = storyId };
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
