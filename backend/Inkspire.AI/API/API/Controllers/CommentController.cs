using API.Contracts.Comment;
using Core.Features.CommentFeatures.Commands.CreateComment;
using Core.Features.CommentFeatures.Queries.GetAllComment;
using Core.Features.CommentFeatures.Queries.GetCommentById;
using Core.Shared;
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            var command = new CreateCommentCommand
            {
                BookId = request.BookId,
                Text = request.Text,
                UserId = request.UserId
            };
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return CreatedAtAction(nameof(CreateComment),
                new { id = result.Value.Id },
                new
                {
                    CommentId = result.Value.Id,
                    Text = result.Value.Text
                });


        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllComments()
        {
            var query = new GetAllCommentQuery();
            var comments = await _mediator.Send(query);
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(Guid id)
        {
            var query = new GetCommentByIdQuery { CommentId = id };
            var comment = await _mediator.Send(query);
            return Ok(comment);

        }
    }
}
