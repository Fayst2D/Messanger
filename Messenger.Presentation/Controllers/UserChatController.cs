using System.Net;
using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.UserChats.CreateChannel;
using Messenger.BusinessLogic.Commands.UserChats.JoinChannel;
using Messenger.BusinessLogic.Queries.UserChats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// ReSharper disable once CheckNamespace
namespace Messenger.Presentation.Controllers
{
    [Authorize]
    [Route("chat")]
    [ApiController]
    public class UserChatController : BaseApiController
    {
        public UserChatController(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

        [HttpGet("chats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserChats()
        {
            return await Request(new GetUserChatsQuery());
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateChannel([FromBody]CreateChannelRequest createChannelRequest)
        {
            var createChannelCommand = _mapper.Map<CreateChannelCommand>(createChannelRequest);
            return await Request(createChannelCommand);
        }

        [HttpPost("join")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> JoinChannel([FromBody]JoinChannelRequest joinChannelRequest)
        {
            var joinChannelCommand = _mapper.Map<JoinChannelCommand>(joinChannelRequest);       
            return await Request(joinChannelCommand);
        }
        
    }
}