using MediatR;
using Messenger.Data;
using Messenger.Domain.Constants;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Limits;

public class LimitRequest
{
    public Guid ChatId { get; set; }
    public Guid LimitedUserId { get; set; }
    public DateTime UnLimitedOAt{ get; set; }
}

public class LimitUserCommand : BaseRequest, IRequest<Response<string>>
{
    public Guid ChatId { get; set; }
    public Guid LimitedUserId { get; set; }
    public DateTime LimitedAt { get; set; }
    public DateTime UnLimitedAt { get; set; }
    public int LimitType { get; set; }
}

public class BanUserHandler : IRequestHandler<LimitUserCommand, Response<string>>
{
    private readonly DatabaseContext _context;

    public BanUserHandler(DatabaseContext context)
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