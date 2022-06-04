using Messanger.BusinessLogic.Commands.Users;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Messanger.BusinessLogic;
using Messanger.BusinessLogic.Queries.Users;
using Microsoft.AspNetCore.Authorization;

namespace Messanger.Presentation.Controllers
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

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand registerUserCommand)
        {
            return Ok(await _mediator.Send(registerUserCommand));
        }
        
        [HttpGet("getUserById")]
        public async Task<IActionResult> GetUserById([FromBody] GetUserByIdQuery getUserByIdQuery)
        {
            return Ok(await _mediator.Send(getUserByIdQuery));
        }
    }
}
