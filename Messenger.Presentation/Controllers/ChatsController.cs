using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.Chats.CreateChannel;
using Messenger.BusinessLogic.Commands.Chats.CreateDirectChat;
using Messenger.BusinessLogic.Commands.Chats.UploadImage;
using Messenger.BusinessLogic.Queries.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Messenger.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/chats")]
public class ChatsController : BaseApiController
{
    public ChatsController(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

    /// <summary>
    /// Get all user's chats
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserChats(CancellationToken cancellationToken)
    {
        return await Request(new GetUserChatsQuery(), cancellationToken);
    }
    
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
    [HttpPost("create-channel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateChannel([FromBody]CreateChannelRequest request, CancellationToken cancellationToken)
    {
        var createChannelCommand = _mapper.Map<CreateChannelCommand>(request);
        return await Request(createChannelCommand, cancellationToken);
    }

    /// <summary>
    /// Create direct chat
    /// </summary>
    /// <param name="partnerId">Partner's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200</returns>
    [HttpPost("{partnerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateDirectChat([FromRoute] Guid partnerId, CancellationToken cancellationToken)
    {
        var createDirectChatCommand = new CreateDirectChatCommand
        {
            PartnerId = partnerId
        };

        return await Request(createDirectChatCommand, cancellationToken);
    }

    /// <summary>
    /// Change chat's image
    /// </summary>
    /// <param name="chatId">Chat's ID</param>
    /// <param name="image">Image</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 400, 404</returns>
    [HttpPost("{chatId:guid}/upload-image")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadImage([FromRoute] Guid chatId, IFormFile image,
        CancellationToken cancellationToken)
    {
        var uploadImageCommand = new UploadImageCommand
        {
            ChatId = chatId,
            Image = image
        };

        return await Request(uploadImageCommand, cancellationToken);
    }

}