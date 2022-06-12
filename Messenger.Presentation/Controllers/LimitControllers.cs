using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.Limits;
using Messenger.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messanger.Presentation.Controllers;

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
    public async Task<IActionResult> BanUser([FromBody]LimitRequest limitRequest)
    {
        var banUserCommand = _mapper.Map<LimitUserCommand>(limitRequest);
        banUserCommand.LimitType = (int)LimitTypes.Ban;
        banUserCommand.LimitedAt = DateTime.Now;
        banUserCommand.UnLimitedAt = limitRequest.UnLimitedOAt;

        return Ok(await _mediator.Send(banUserCommand));
    }

    [HttpPost("mute")]
    public async Task<IActionResult> MuteUser([FromBody]LimitRequest limitRequest)
    {
        var muteUserCommand = _mapper.Map<LimitUserCommand>(limitRequest);
        muteUserCommand.LimitType = (int)LimitTypes.Mute;
        muteUserCommand.LimitedAt = DateTime.Now;
        muteUserCommand.UnLimitedAt = limitRequest.UnLimitedOAt;
        

        return Ok(await _mediator.Send(muteUserCommand));
    }
}