using System.Net;
using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace Messenger.BusinessLogic.Commands.UserChats.JoinChannel;


public class JoinChannelHandler : IRequestHandler<JoinChannelCommand, Response<Chat>>
{
    private readonly DatabaseContext _context;

    public JoinChannelHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<Chat>> Handle(JoinChannelCommand request, CancellationToken cancellationToken)
    {
        var banEntity = await _context.UserLimits
            .Where(x => x.ChatId == request.ChatId && x.UserId == request.UserId &&
                        x.LimitType == (int)LimitTypes.Ban)
            .FirstOrDefaultAsync(cancellationToken);

        if (banEntity != null)
        {

            if (DateTime.Compare(banEntity.LimitedAt,banEntity.UnLimitedAt) >= 0)
            {
                return Response.Fail<Chat>($"you will unban on {banEntity.UnLimitedAt}", HttpStatusCode.BadRequest);
            }
            
            _context.UserLimits.Remove(banEntity);
        }
        
        var isJoined = await _context.UserChats
            .AnyAsync(entity => entity.UserId == request.UserId && entity.ChatId == request.ChatId,cancellationToken);

        if (isJoined)
        {
            return Response.Fail<Chat>("AlreadyJoined", HttpStatusCode.Conflict);
        }
        
        var chatEntity = await _context.Chats
            .Where(x => x.ChatType == (int)ChatTypes.Channel)
            .FirstOrDefaultAsync(x => x.Id == request.ChatId, cancellationToken);

        if (chatEntity == null)
        {
            return Response.Fail<Chat>("ChatNotFound",HttpStatusCode.NotFound);
        }

        chatEntity.MembersCount++;
        _context.Chats.Update(chatEntity);

        _context.UserChats.Add(new UserChatEntity
        {
            Id = Guid.NewGuid(),
            ChatId = request.ChatId,
            RoleId = (int)UserRoles.User,
            UserId = request.UserId
        });

        await _context.SaveChangesAsync(cancellationToken);

        var chat = new Chat()
        {
            Id = chatEntity.Id,
            MembersCount = chatEntity.MembersCount,
            Title = chatEntity.Title
        };

        return Response.Ok("Ok", chat);
    }
}



