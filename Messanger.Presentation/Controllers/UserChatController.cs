using System.Security.Claims;
using MediatR;
using Messanger.BusinessLogic.Commands.UserChats;
using Messanger.BusinessLogic.Queries.Chats;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserChatController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("chats")]
        public async Task<IActionResult> GetUserChats()
        {
            return Ok(await _mediator.Send(new GetUserChatsQuery()));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChannel([FromBody]string Title)
        {
            var createChannelCommand = new CreateChannelCommand
            {
                Title = Title
            };
            
            return Ok(await _mediator.Send(createChannelCommand));
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinChannel([FromBody] Guid ChatId)
        {
            var joinChannelCommand = new JoinChannelCommand
            {
                ChatId = ChatId
            };
            
            return Ok(await _mediator.Send(joinChannelCommand));
        }

       

    }
}
