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

        /// <summary>
        /// Get all user's chats
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Status codes: 200</returns>
        [HttpGet("chats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserChats(CancellationToken cancellationToken)
        {
            return await Request(new GetUserChatsQuery(), cancellationToken);
        }

        /// <summary>
        /// Create public channel
        /// </summary>
        /// <param name="request">CreateChannelRequest</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Status codes: 200, 422</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateChannel([FromBody]CreateChannelRequest request, CancellationToken cancellationToken)
        {
            var createChannelCommand = _mapper.Map<CreateChannelCommand>(request);
            return await Request(createChannelCommand, cancellationToken);
        }

        /// <summary>
        /// Joins to channel by chat's ID
        /// </summary>
        /// <param name="chatId">Chat's ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Status codes: 200, 400, 422, 404</returns>
        [HttpPost("join")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> JoinChannel([FromQuery]Guid chatId, CancellationToken cancellationToken)
        {
            var joinChannelCommand = new JoinChannelCommand
            {
                ChatId = chatId
            }; 
            
            return await Request(joinChannelCommand, cancellationToken);
        }
        
    }
}