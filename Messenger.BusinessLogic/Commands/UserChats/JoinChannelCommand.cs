using MediatR;
using Messenger.Data;
using Messenger.Domain.Constants;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Messenger.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.UserChats;

public class JoinChannelRequest
{
    public Guid ChatId { get; set; }
}
public class JoinChannelCommand : BaseRequest, IRequest<Response<Chat>>
{
    public Guid ChatId { get; set; }
}

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
                return Response.Fail<Chat>($"you will unban on {banEntity.UnLimitedAt}");
            }
            
            _context.UserLimits.Remove(banEntity);
        }
        
        var IsJoined = await _context.UserChats
            .AnyAsync(entity => entity.UserId == request.UserId && entity.ChatId == request.ChatId,cancellationToken);

        if (IsJoined)
        {
            return Response.Fail<Chat>("AlreadyJoined");
        }
        
        var chatEntity = await _context.Chats.Where(x => x.ChatType == (int)ChatTypes.Channel).FirstOrDefaultAsync(x => x.Id == request.ChatId);

        if (chatEntity == null)
        {
            return Response.Fail<Chat>("ChatNotFound");
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

        await _context.SaveChangesAsync();

        var chat = new Chat()
        {
            Id = chatEntity.Id,
            MembersCount = chatEntity.MembersCount,
            Title = chatEntity.Title
        };

        return Response.Ok("Ok", chat);
    }
}



