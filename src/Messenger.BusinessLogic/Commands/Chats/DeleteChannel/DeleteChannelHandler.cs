using System.Net;
using MediatR;
using Messenger.BusinessLogic.Hubs;
using Messenger.BusinessLogic.Models;
using Messenger.Data.Database;
using Messenger.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Chats.DeleteChannel;

public class DeleteChannelHandler : IRequestHandler<DeleteChannelCommand, Response<Chat>>
{
    private readonly DatabaseContext _context;
    private readonly IHubContext<NotifyHub, IHubClient> _hubContext;

    public DeleteChannelHandler(DatabaseContext context, IHubContext<NotifyHub, IHubClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<Response<Chat>> Handle(DeleteChannelCommand request, CancellationToken cancellationToken)
    {
        var userChatEntity = await _context.UserChats
            .Include(x => x.Chat)
            .ThenInclude(x => x.ChatUsers)
            .Where(x => 
                x.ChatId == request.ChatId && 
                x.UserId == request.UserId &&
                x.Chat.ChatType == (int)ChatTypes.Channel)
            .FirstOrDefaultAsync(cancellationToken);

        if (userChatEntity == null)
        {
            return Response.Fail<Chat>("Chat not found", HttpStatusCode.NotFound);
        }

        if (userChatEntity.RoleId != (int)UserChatRoles.Owner)
        {
            return Response.Fail<Chat>("You don't have enough rights", HttpStatusCode.BadRequest);
        }
        
        _context.UserChats.RemoveRange(userChatEntity.Chat.ChatUsers);
        _context.Chats.Remove(userChatEntity.Chat);

        await _context.SaveChangesAsync(cancellationToken);

        var chat = new Chat
        {
            Id = request.ChatId,
            ChatType = userChatEntity.Chat.ChatType,
            Image = userChatEntity.Chat.Image,
            MembersCount = userChatEntity.Chat.MembersCount,
            Title = userChatEntity.Chat.Title
        };

        await _hubContext.Clients.Group(request.ChatId.ToString()).UpdateUserChatsAsync(chat);
        
        return Response.Ok("Ok", chat);
    }
}