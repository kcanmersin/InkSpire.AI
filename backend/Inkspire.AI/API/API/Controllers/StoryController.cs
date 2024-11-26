using API.Contracts.Story;
using Core.Features.StoryFeatures.Commands.CreateStory;
using Core.Features.StoryFeatures.Queries.GetAllStories;
using Core.Features.StoryFeatures.Queries.GetStoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStoryRequest request)
        {
            var command = new CreateStoryCommand
            {
                Title = request.Title,
                Description = request.Description,
                PageCount = request.PageCount,
                UserId = request.UserId
            };

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Error = result.Error.Message });
            }

            var response = new CreateStoryResponse
            {
                Id = result.Value.Id,
                Title = result.Value.Title,
                PageCount = result.Value.PageCount,
                IsPublic = result.Value.IsPublic,
                CreatedDate = result.Value.CreatedDate
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllStoriesQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Error = result.Error.Message });
            }

            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetStoryByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return NotFound(new { Error = result.Error.Message });
            }

            return Ok(result.Value);
        }
    }
}
