using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.Limits;
using Messenger.BusinessLogic.Commands.Limits.LimitUser;
using Messenger.BusinessLogic.Commands.Limits.RemoveUserLimit;
using Messenger.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Messenger.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class LimitControllers : BaseApiController
{
    public LimitControllers(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }
    
    /// <summary>
    /// Ban user for some time
    /// </summary>
    /// <param name="request">Chat's ID, user's ID and time of unban</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Status codes: 200, 400, 404, 422</returns>
    [HttpPost("ban")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> BanUser([FromBody]LimitUserRequest request, CancellationToken cancellationToken)
    {
        var banUserCommand = new LimitUserCommand
        {
            ChatId = request.ChatId,
            LimitedAt = DateTime.Now,
            LimitedUserId = request.LimitedUserId,
            LimitType = (int)LimitTypes.Ban,
            UnLimitedAt = request.UnLimitedAt
        };

        return await Request(banUserCommand, cancellationToken);
    }

    /// <summary>
    /// Mute user for some time
    /// </summary>
    /// <param name="request">Chat's ID, user's ID and time of unmute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 400, 404, 422</returns>
    [HttpPost("mute")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> MuteUser([FromBody]LimitUserRequest request, CancellationToken cancellationToken)
    {
        var muteUserCommand = new LimitUserCommand
        {
            ChatId = request.ChatId,
            LimitedAt = DateTime.Now,
            LimitedUserId = request.LimitedUserId,
            LimitType = (int)LimitTypes.Mute,
            UnLimitedAt = request.UnLimitedAt
        };
        
        return await Request(muteUserCommand, cancellationToken);
    }

    /// <summary>
    /// Unban or unmute user
    /// </summary>
    /// <param name="request">Limited user's and chat's IDs</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 404, 400, 422</returns>
    [HttpDelete("remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RemoveUserLimit([FromBody] RemoveUserLimitRequest request, CancellationToken cancellationToken)
    {
        var removeUserLimitCommand = _mapper.Map<RemoveUserLimitCommand>(request);

        return await Request(removeUserLimitCommand, cancellationToken);
    }
}