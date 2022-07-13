using System.Net;
using MediatR;
using Messenger.BusinessLogic.Hubs;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Data.Database;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Chats.CreateDirectChat;

public class CreateDirectChatHandler : IRequestHandler<CreateDirectChatCommand, Response<Chat>>
{
    private readonly DatabaseContext _context;
    private readonly IHubContext<NotifyHub, IHubClient> _hubContext;

    public CreateDirectChatHandler(DatabaseContext context, IHubContext<NotifyHub, IHubClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }
    
    public async Task<Response<Chat>> Handle(CreateDirectChatCommand request, CancellationToken cancellationToken)
    {
        var partner = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.PartnerId,cancellationToken);

        if (partner == null)
        {
            return Response.Fail<Chat>("Partner not found", HttpStatusCode.NotFound);
        }

        if (request.UserId == request.PartnerId)
        {
            return Response.Fail<Chat>("You can't add yourself", HttpStatusCode.Conflict);
        }

        var userDirectChats = await _context.Chats
            .Where(x => x.ChatType == (int)ChatTypes.DirectChat)
            .Include(x => x.ChatUsers)
            .Where(x => x.ChatUsers.Any(y => y.UserId == request.UserId))
            .AnyAsync(x => x.ChatUsers.Any(y => y.UserId == request.PartnerId), cancellationToken);
                

        var currentUsername = await _context.Users
            .Where(x => x.Id == request.UserId)
            .Select(x => x.Username)
            .FirstOrDefaultAsync(cancellationToken);
        

        var chatEntity = new ChatEntity
        {
            Id = Guid.NewGuid(),
            ChatType = (int)ChatTypes.DirectChat,
            MembersCount = 2,
            Title = $"{currentUsername}/{partner.Username}"
        };

        var userChats = new []
        {
            new UserChatEntity { ChatId = chatEntity.Id, RoleId = (int)UserChatRoles.User, UserId = request.UserId },
            new UserChatEntity { ChatId = chatEntity.Id, RoleId = (int)UserChatRoles.User, UserId = request.PartnerId }
        };

        _context.Chats.Add(chatEntity);
        _context.UserChats.AddRange(userChats);

        await _context.SaveChangesAsync(cancellationToken);

        var chat = new Chat
        {
            Id = chatEntity.Id,
            ChatType = chatEntity.ChatType,
            MembersCount = chatEntity.MembersCount,
            Title = chatEntity.Title
        };

        await _hubContext.Clients.User(request.UserId.ToString()).UpdateUserChatsAsync(chat);
        await _hubContext.Clients.User(request.PartnerId.ToString()).UpdateUserChatsAsync(chat);

        
        return Response.Ok<Chat>("Ok",chat);

    }
}