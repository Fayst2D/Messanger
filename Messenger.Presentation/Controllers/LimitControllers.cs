using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.Limits;
using Messenger.BusinessLogic.Commands.Limits.LimitUser;
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
    
    [HttpPost("ban")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BanUser([FromBody]LimitUserRequest limitRequest)
    {
        var banUserCommand = new LimitUserCommand
        {
            ChatId = limitRequest.ChatId,
            LimitedAt = DateTime.Now,
            LimitedUserId = limitRequest.LimitedUserId,
            LimitType = (int)LimitTypes.Ban,
            UnLimitedAt = limitRequest.UnLimitedAt
        };

        return await Request(banUserCommand);
    }

    [HttpPost("mute")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MuteUser([FromBody]LimitUserRequest limitRequest)
    {
        var muteUserCommand = new LimitUserCommand
        {
            ChatId = limitRequest.ChatId,
            LimitedAt = DateTime.Now,
            LimitedUserId = limitRequest.LimitedUserId,
            LimitType = (int)LimitTypes.Mute,
            UnLimitedAt = limitRequest.UnLimitedAt
        };
        
        return await Request(muteUserCommand);
    }
}