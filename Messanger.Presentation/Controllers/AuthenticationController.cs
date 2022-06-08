using MediatR;
using Messenger.BusinessLogic.Commands.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messanger.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public AuthenticationController(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand authenticateUserCommand)
        {
            return Ok(await _mediatr.Send(authenticateUserCommand));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokensCommand refreshTokensCommand)
        {
            return Ok(await _mediatr.Send(refreshTokensCommand));
        }
    }
}
