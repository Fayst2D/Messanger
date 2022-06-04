using MediatR;
using Messanger.BusinessLogic.Models;
using Messanger.DataAccess;
using Messanger.Domain.Constants;
using Messanger.Domain.Entities;
using Messanger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messanger.BusinessLogic.Commands.UserChats;

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
        var IsJoined = await _context.UserChats
            .AnyAsync(entity => entity.UserId == request.UserId && entity.ChatId == request.ChatId);

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