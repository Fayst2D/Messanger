using System.Net;
using MediatR;
using Messenger.Data;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.UserChats.LeaveChat;

public class LeaveChatHandler : IRequestHandler<LeaveChatCommand, Response<string>>
{
    private readonly DatabaseContext _context;

    public LeaveChatHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response<string>> Handle(LeaveChatCommand request, CancellationToken cancellationToken)
    {
        
        var userChatEntity = await _context.UserChats
            .Include(x => x.Chat)
            .Where(x => x.ChatId == request.ChatId)
            .Where(x => x.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        
        if (userChatEntity == null)
        {
            return Response.Fail<string>("Chat not found", HttpStatusCode.NotFound);
        }

        if (userChatEntity.RoleId == (int)UserRoles.Owner)
        {
            return Response.Fail<string>("Owner can't leave chat", HttpStatusCode.BadRequest);
        }

        var chatEntity = userChatEntity.Chat;

        if (chatEntity.ChatType == (int)ChatTypes.DirectChat)
        {
            var messages = await _context.Messages
                .Where(x => x.ChatId == request.ChatId)
                .ToListAsync(cancellationToken);

            _context.RemoveRange(messages);
            _context.RemoveRange(chatEntity.ChatUsers);
            _context.Remove(chatEntity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Response.Ok<string>("Ok", "Direct chat deleted");
        }

        _context.UserChats.Remove(userChatEntity);
        
        chatEntity.MembersCount--;
        _context.Chats.Update(chatEntity);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Response.Ok<string>("Ok", "You leaved the channel");
    }
}