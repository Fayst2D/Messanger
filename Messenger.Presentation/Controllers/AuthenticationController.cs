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

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="authenticateUserCommand">Email and password</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Status codes: 200, 404, 400, 422</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Login([FromBody] LoginCommand authenticateUserCommand, CancellationToken cancellationToken)
        {
            return await Request(authenticateUserCommand, cancellationToken);
        }

        /// <summary>
        /// Refresh JWT tokens
        /// </summary>
        /// <param name="refreshTokensCommand">Refresh token</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Status codes: 200, 404, 400, 422</returns>
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokensCommand refreshTokensCommand, CancellationToken cancellationToken)
        {
            return await Request(refreshTokensCommand, cancellationToken);
        }
    }
}
