using MediatR;
using Messenger.BusinessLogic.Commands.Authentication;
using Messenger.BusinessLogic.Commands.Authentication.Login;
using Messenger.BusinessLogic.Commands.Authentication.RefreshTokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Messenger.Presentation.Controllers
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
