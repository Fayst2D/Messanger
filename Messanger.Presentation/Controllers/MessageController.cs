using System.Security.Claims;
using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.Messages;
using Messenger.BusinessLogic.Queries.Messages.GetMessages;
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
        private readonly IMapper _mapper;

        public MessageController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        
        [HttpGet("get")]
        public async Task<IActionResult> GetMessages([FromQuery]GetMessagesRequest getMessagesRequest)
        {
            var getMessagesQuery = _mapper.Map<GetMessagesQuery>(getMessagesRequest);
            
            return Ok(await _mediator.Send(getMessagesQuery));
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest sendMessageRequest)
        {
            var sendMessageCommand = _mapper.Map<SendMessageCommand>(sendMessageRequest);

            return Ok(await _mediator.Send(sendMessageCommand));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMessage([FromBody] DeleteMessageRequest deleteMessageRequest)
        {
            var deleteMessageCommand = _mapper.Map<DeleteMessageCommand>(deleteMessageRequest);

            return Ok(await _mediator.Send(deleteMessageCommand));
        }

    }
}
