using Microsoft.AspNetCore.Mvc;
using MediatR;
using Messenger.BusinessLogic.Commands.Users.Register;
using Messenger.BusinessLogic.Queries.Users;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable once CheckNamespace
namespace Messenger.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Register a new user.</summary>
        /// <param name="registerUserCommand">User registration information.</param>
        /// <response code="200">Newly registered user.</response>
        /// <response code="409">Failed to register a user: username already taken.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand registerUserCommand)
        {
            return Ok(await _mediator.Send(registerUserCommand));
        }
        
        [HttpGet("get")]
        public async Task<IActionResult> GetUserById([FromBody] GetUserByIdQuery getUserByIdQuery)
        {
            return Ok(await _mediator.Send(getUserByIdQuery));
        }
    }
}
