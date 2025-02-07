using API.Contracts.User;
using Core.Features.UserFeatures.Register;
using Core.Features.UserFeatures.Login;
using Core.Features.UserFeatures.Update;
using Core.Features.UserFeatures.GetUserDetails;
using Core.Features.UserFeatures.Verify2FA;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var command = new RegisterCommand
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.Name,
                Surname = request.Surname,
                Roles = request.Roles,
                NativeLanguage = request.NativeLanguage,
                TargetLanguage = request.TargetLanguage
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return CreatedAtAction(nameof(Register), new { id = result.Value.Id }, new
            {
                Id = result.Value.Id,
                Email = result.Value.Email,
                Name = result.Value.Name,
                Surname = result.Value.Surname
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return Ok(new
            {
                Token = result.Value.Token,
                Email = result.Value.Email,
                Name = result.Value.Name,
                Surname = result.Value.Surname,
                UserId = result.Value.UserId
            });
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] Verify2FARequest request)
        {
            var command = new Verify2FACommand
            {
                UserId = request.UserId,
                TwoFactorCode = request.TwoFactorCode
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return Ok(new { Token = result.Value });
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var command = new UpdateUserCommand
            {
                UserId = request.UserId,
                Name = request.Name,
                Surname = request.Surname,
                NativeLanguage = request.NativeLanguage,
                TargetLanguage = request.TargetLanguage
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return Ok(new { Message = "User updated successfully." });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserDetail(Guid userId)
        {
            var query = new GetUserDetailByIdQuery { UserId = userId };
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(new { Error = result.Error.Message });

            return Ok(result.Value);
        }
    }
}
