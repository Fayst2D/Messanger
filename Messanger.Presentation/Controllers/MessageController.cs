using System.Security.Claims;
using MediatR;
using Messanger.BusinessLogic.Commands.Messages;
using Messanger.BusinessLogic.Queries.Messages.GetMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Messanger.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] Guid ChatId)
        {
            var getMessagesQuery = new GetMessagesQuery
            {
                ChatId = ChatId
            };
            
            return Ok(await _mediator.Send(getMessagesQuery));
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest sendMessageRequest)
        {
            var sendMessageCommand = new SendMessageCommand
            {
                ChatId = sendMessageRequest.ChatId,
                Message = sendMessageRequest.Message
            };
            
            return Ok(await _mediator.Send(sendMessageCommand));
        }
        

    }
}
