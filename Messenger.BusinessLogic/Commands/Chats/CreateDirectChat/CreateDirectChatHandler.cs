using System.Net;
using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Chats.CreateDirectChat;

public class CreateDirectChatHandler : IRequestHandler<CreateDirectChatCommand, Response<Chat>>
{
    private readonly DatabaseContext _context;

    public CreateDirectChatHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<Chat>> Handle(CreateDirectChatCommand request, CancellationToken cancellationToken)
    {
        var partner = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.PartnerId,cancellationToken);

        if (partner == null)
        {
            return Response.Fail<Chat>("partner not found", HttpStatusCode.NotFound);
        }

        if (request.UserId == request.PartnerId)
        {
            return Response.Fail<Chat>("you can't add yourself", HttpStatusCode.Conflict);
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
            new UserChatEntity { ChatId = chatEntity.Id, RoleId = (int)UserRoles.User, UserId = request.UserId },
            new UserChatEntity { ChatId = chatEntity.Id, RoleId = (int)UserRoles.User, UserId = request.PartnerId }
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
        
        return Response.Ok<Chat>("Ok",chat);

    }
}