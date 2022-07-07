using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.UserChats.JoinChannel;
using Messenger.BusinessLogic.Commands.UserChats.LeaveChat;
using Messenger.BusinessLogic.Queries.UserChats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Messenger.Presentation.Controllers;

[Authorize]
[Route("api/user-chats/{chatId:guid}")]
[ApiController]
public class UserChatsController : BaseApiController
{
    public UserChatsController(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

        
        

    /// <summary>
    /// Joins to channel by chat's ID
    /// </summary>
    /// <param name="chatId">Chat's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 400, 422, 404</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> JoinChannel([FromRoute]Guid chatId, CancellationToken cancellationToken)
    {
        var joinChannelCommand = new JoinChannelCommand
        {
            ChatId = chatId
        }; 
            
        return await Request(joinChannelCommand, cancellationToken);
    }
        
    /// <summary>
    /// Get users by chat
    /// </summary>
    /// <param name="chatId">Chat's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200</returns>
    [HttpGet("users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersByChat([FromRoute] Guid chatId, CancellationToken cancellationToken)
    {
        var getUsersChatByChatQuery = new GetUsersByChatQuery
        {
            ChatId = chatId
        };

        return await Request(getUsersChatByChatQuery, cancellationToken);
    }

    /// <summary>
    /// Leave channel or direct chat
    /// </summary>
    /// <param name="chatId">Chat's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 404, 400</returns>
    [HttpDelete("leave")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LeaveChat([FromRoute] Guid chatId, CancellationToken cancellationToken)
    {
        var leaveChatCommand = new LeaveChatCommand
        {
            ChatId = chatId
        };

        return await Request(leaveChatCommand, cancellationToken);
    }
}