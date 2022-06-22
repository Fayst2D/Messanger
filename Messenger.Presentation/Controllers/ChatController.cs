using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.Chats.CreateChannel;
using Messenger.BusinessLogic.Commands.Chats.CreateDirectChat;
using Messenger.BusinessLogic.Queries.Chats;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Presentation.Controllers;

public class ChatController : BaseApiController
{
    public ChatController(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

    /// <summary>
    /// Search chats by title
    /// </summary>
    /// <param name="title">Chat's title</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string title, CancellationToken cancellationToken)
    {
        var searchChatQuery = new SearchChatsQuery
        {
            Title = title
        };

        return await Request(searchChatQuery, cancellationToken);
    }
    
    /// <summary>
    /// Create public channel
    /// </summary>
    /// <param name="request">CreateChannelRequest</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 422</returns>
    [HttpPost("channel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateChannel([FromBody]CreateChannelRequest request, CancellationToken cancellationToken)
    {
        var createChannelCommand = _mapper.Map<CreateChannelCommand>(request);
        return await Request(createChannelCommand, cancellationToken);
    }

    [HttpPost("chat")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateDirectChat([FromQuery] Guid partnerId, CancellationToken cancellationToken)
    {
        var createDirectChatCommand = new CreateDirectChatCommand
        {
            PartnerId = partnerId
        };

        return await Request(createDirectChatCommand, cancellationToken);
    }
    
}