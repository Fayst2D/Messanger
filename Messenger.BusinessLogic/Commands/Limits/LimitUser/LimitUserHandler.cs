using System.Net;
using Messenger.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Messenger.Data.Database;

namespace Messenger.BusinessLogic.Commands.Limits.LimitUser;

public class LimitUserHandler : IRequestHandler<LimitUserCommand, Response<string>>
{
    private readonly DatabaseContext _context;

    public LimitUserHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<string>> Handle(LimitUserCommand request, CancellationToken cancellationToken)
    {
        var limitedUser = await _context.UserChats //entity of banned user
            .AsNoTracking()
            .Where(x => x.ChatId == request.ChatId && x.UserId == request.LimitedUserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (limitedUser == null)
        {
            return Response.Fail<string>("banned user not found", HttpStatusCode.NotFound);
        }
        
        
        var currentUser = await _context.UserChats //entity of admin user
            .AsNoTracking()
            .Where(x => x.ChatId == request.ChatId && x.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (currentUser.RoleId <= limitedUser.RoleId)
        {
            return Response.Fail<string>("you don't have enough rights", HttpStatusCode.BadRequest);
        }
        
        
        var chatEntity = await _context.Chats
            .Where(x => x.Id == request.ChatId)
            .FirstOrDefaultAsync(cancellationToken);

        if (chatEntity == null)
        {
            return Response.Fail<string>("chat not found", HttpStatusCode.NotFound);
        }
        

        var userLimitEntity = new UserLimitEntity
        {
            Id = Guid.NewGuid(),
            ChatId = request.ChatId,
            UserId = request.LimitedUserId,
            LimitType = request.LimitType,
            LimitedAt = request.LimitedAt,
            UnLimitedAt = request.UnLimitedAt
        };
        
        await _context.UserLimits.AddAsync(userLimitEntity, cancellationToken);
        
        if (request.LimitType == (int)LimitTypes.Ban)
        {

            chatEntity.MembersCount--;
            _context.Chats.Update(chatEntity);

            _context.UserChats.Remove(limitedUser);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Response.Ok<string>("Ok", "user successfully muted");
        }
        
        await _context.SaveChangesAsync(cancellationToken);

        return Response.Ok<string>("Ok","user successfully muted");
    }
}