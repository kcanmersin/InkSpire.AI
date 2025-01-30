using API.Contracts;
using API.Contracts.Book;
using API.Helper;
using Core.Features.BookFeatures.Commands.CreateBook;
using Core.Features.BookFeatures.Queries.GetAllBook;
using Core.Features.BookFeatures.Queries.GetBookById;
using Core.Features.TestFeatures.Commands.CreateTest;
using Core.Features.TestFeatures.Commands.SolveTest;
using Core.Features.TestFeatures.Queries.GetTestByBookId;
using Core.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request)
        {
            var command = new CreateBookCommand
            {
                AuthorId = request.AuthorId,
                Title = request.Title,
                Language = request.Language,
                Level = request.Level
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return CreatedAtAction(nameof(CreateBook),
                new { id = result.Value.BookId },
                new
                {
                    BookId = result.Value.BookId,
                    Title = result.Value.Title
                });
        }

        [HttpGet]
        //[ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client, NoStore = false)]
        [Cache(600)]
        public async Task<IActionResult> GetAllBooks()
        {
            var query = new GetAllBookQuery();
            var books = await _mediator.Send(query);
            return Ok(books);
        }

        [HttpGet("{id}")]
        //[ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client, NoStore = false)]
        [Cache(600)]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var query = new GetBookByIdQuery { BookId = id };
            var book = await _mediator.Send(query);
            return Ok(book);
        }

        [HttpPost("test")]
        //create test
        public async Task<IActionResult> CreateTest([FromBody] CreateTestRequest request)
        {
            var command = new CreateTestCommand
            {
                BookId = request.BookId,
                UserId = request.UserId
            };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return CreatedAtAction(nameof(CreateTest),
                new { id = result.Value.TestId },
                new
                {
                    TestId = result.Value.TestId,
                });
        }
        //gettest by bookid
        [HttpGet("test")]
        public async Task<IActionResult> GetTestByBookId([FromQuery] GetTestByBookIdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var query = new GetTestByBookIdQuery { BookId = request.BookId, UserId = request.UserId };
            var test = await _mediator.Send(query);
            return Ok(test);
        }
        //solve test
        [HttpPost("solve-test")]
        public async Task<IActionResult> SolveTest([FromBody] SolveTestRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new SolveTestCommand
            {
                TestId = request.TestId,
                Answers = request.Answers
            };

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return Ok(result.Value);
        }
    }
}
