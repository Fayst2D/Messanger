using System.Net;
using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.Messages.Delete;
using Messenger.BusinessLogic.Commands.Messages.Edit;
using Messenger.BusinessLogic.Commands.Messages.Send;
using Messenger.BusinessLogic.Queries.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// ReSharper disable once CheckNamespace
namespace Messenger.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MessageController : BaseApiController
    {
        public MessageController(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }
        
        [HttpGet("get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMessages([FromQuery]GetMessagesRequest getMessagesRequest)
        {
            var getMessagesQuery = _mapper.Map<GetMessagesQuery>(getMessagesRequest);

            return await Request(getMessagesQuery);
        }

        [HttpPost("send")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest sendMessageRequest)
        {
            var sendMessageCommand = _mapper.Map<SendMessageCommand>(sendMessageRequest);

            return await Request(sendMessageCommand);
        }

        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMessage([FromBody] DeleteMessageRequest deleteMessageRequest)
        {
            var deleteMessageCommand = _mapper.Map<DeleteMessageCommand>(deleteMessageRequest);

            return await Request(deleteMessageCommand);
        }

        [HttpPut("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> EditMessage([FromBody] EditMessageRequest editMessageRequest)
        {
            var editMessageCommand = _mapper.Map<EditMessageCommand>(editMessageRequest);

            return await Request(editMessageCommand);
        }

    }
}
