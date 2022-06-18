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
    public class AuthenticationController : BaseApiController
    {
        public AuthenticationController(IMediator mediator) : base(mediator,null) { }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Login([FromBody] LoginCommand authenticateUserCommand)
        {
            return await Request(authenticateUserCommand);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokensCommand refreshTokensCommand)
        {
            return await Request(refreshTokensCommand);
        }
    }
}
