using System.Net;
using MediatR;
using Messenger.BusinessLogic.Hubs;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Data.Database;
using Messenger.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.UserChats.LeaveChat;

public class LeaveChatHandler : IRequestHandler<LeaveChatCommand, Response<Chat>>
{
    private readonly DatabaseContext _context;
    private readonly IHubContext<NotifyHub, IHubClient> _hubContext;

    public LeaveChatHandler(DatabaseContext context, IHubContext<NotifyHub, IHubClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<Response<Chat>> Handle(LeaveChatCommand request, CancellationToken cancellationToken)
    {
        
        var userChatEntity = await _context.UserChats
            .Include(x => x.Chat)
            .Where(x => x.ChatId == request.ChatId)
            .Where(x => x.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        
        if (userChatEntity == null)
        {
            return Response.Fail<Chat>("Chat not found", HttpStatusCode.NotFound);
        }

        if (userChatEntity.RoleId == (int)UserChatRoles.Owner)
        {
            return Response.Fail<Chat>("Owner can't leave chat", HttpStatusCode.BadRequest);
        }

        var chatEntity = userChatEntity.Chat;

        var chat = new Chat
        {
            ChatType = chatEntity.ChatType,
            Id = chatEntity.Id,
            Image = chatEntity.Image,
            MembersCount = chatEntity.MembersCount--,
            Title = chatEntity.Title
        };

        if (chatEntity.ChatType == (int)ChatTypes.DirectChat)
        {
            var messages = await _context.Messages
                .Where(x => x.ChatId == request.ChatId)
                .ToListAsync(cancellationToken);

            _context.RemoveRange(messages);
            _context.RemoveRange(chatEntity.ChatUsers);
            _context.Remove(chatEntity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Response.Ok<Chat>("Ok", chat);
        }

        _context.UserChats.Remove(userChatEntity);
        
        chatEntity.MembersCount--;
        _context.Chats.Update(chatEntity);

        await _context.SaveChangesAsync(cancellationToken);
        
        await _hubContext.Clients.User(request.UserId.ToString()).UpdateUserChatsAsync(chat);
        
        return Response.Ok<Chat>("Ok", chat);
    }
}