using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Messenger.BusinessLogic.Commands.Users.Register;
using Messenger.BusinessLogic.Queries.Users;
using Microsoft.AspNetCore.Authorization;


namespace Messenger.Presentation.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : BaseApiController
{
    public UsersController(IMediator mediator,IMapper mapper) : base(mediator,mapper) { }

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="registerUserCommand">User registration information.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 409, 422</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand registerUserCommand, CancellationToken cancellationToken)
    {
        return await Request(registerUserCommand, cancellationToken);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="userId">User's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 404, 200</returns>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserById([FromRoute]Guid userId, CancellationToken cancellationToken)
    {
        var getUserByIdQuery = new GetUserByIdQuery
        {
            UserId = userId
        };
            
        return await Request(getUserByIdQuery, cancellationToken);
    }
}