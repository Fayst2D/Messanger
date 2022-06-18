using AutoMapper;
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
    public class UserController : BaseApiController
    {
        public UserController(IMediator mediator,IMapper mapper) : base(mediator,mapper) { }

        /// <summary>Register a new user.</summary>
        /// <param name="registerUserCommand">User registration information.</param>
        /// <response code="200">Newly registered user.</response>
        /// <response code="409">Failed to register a user: email already taken.</response>
        /// <response code="422">Validation error</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand registerUserCommand)
        {
            return await Request(registerUserCommand);
        }
        
        [HttpGet("get")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById([FromBody] GetUserByIdQuery getUserByIdQuery)
        {
            return await Request(getUserByIdQuery);
        }
    }
}
