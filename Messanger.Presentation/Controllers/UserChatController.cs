using System.Security.Claims;
using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.UserChats;
using Messenger.BusinessLogic.Queries.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Messanger.Presentation.Controllers
{
    [Authorize]
    [Route("chat")]
    [ApiController]
    public class UserChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        

        public UserChatController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("chats")]
        public async Task<IActionResult> GetUserChats()
        {
            return Ok(await _mediator.Send(new GetUserChatsQuery()));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChannel([FromBody]CreateChannelRequest createChannelRequest)
        {
            var createChannelCommand = _mapper.Map<CreateChannelCommand>(createChannelRequest);
            return Ok(await _mediator.Send(createChannelCommand));
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinChannel([FromBody]JoinChannelRequest joinChannelRequest)
        {
            var joinChannelCommand = _mapper.Map<JoinChannelRequest>(joinChannelRequest);       
            return Ok(await _mediator.Send(joinChannelCommand));
        }

       

    }
}
