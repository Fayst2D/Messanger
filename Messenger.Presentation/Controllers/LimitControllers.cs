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
public class LimitControllers : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public LimitControllers(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost("ban")]
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

        return Ok(await _mediator.Send(banUserCommand));
    }

    [HttpPost("mute")]
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
        
        return Ok(await _mediator.Send(muteUserCommand));
    }
}