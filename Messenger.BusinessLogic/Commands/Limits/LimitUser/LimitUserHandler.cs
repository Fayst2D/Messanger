using Messenger.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using MediatR;

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


        var currentUser = await _context.UserChats //entity of admin user
            .AsNoTracking()
            .Where(x => x.ChatId == request.ChatId && x.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        
        if (limitedUser == null)
        {
            return Response.Fail<string>("banned user not found");
        }

        if (currentUser == null)
        {
            return Response.Fail<string>("you didn't authenticated");
        }

        if (currentUser.RoleId <= limitedUser.RoleId)
        {
            return Response.Fail<string>("you don't have enough rights");
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

        
        
        await _context.UserLimits.AddAsync(userLimitEntity);
        
        if (request.LimitType == (int)LimitTypes.Ban)
        {
            var chatEntity = await _context.Chats
                .Where(x => x.Id == request.ChatId)
                .FirstOrDefaultAsync(cancellationToken);
            
            chatEntity.MembersCount--;
            _context.Chats.Update(chatEntity);

            _context.UserChats.Remove(limitedUser);
        }
        
        await _context.SaveChangesAsync(cancellationToken);

        return Response.Ok<string>("Ok","user successfully banned");
    }
}