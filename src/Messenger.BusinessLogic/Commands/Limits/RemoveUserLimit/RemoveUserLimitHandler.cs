using System.Net;
using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Data.Database;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Limits.RemoveUserLimit;

public class RemoveUserLimitHandler : IRequestHandler<RemoveUserLimitCommand, Response<string>>
{
    private readonly DatabaseContext _context;

    public RemoveUserLimitHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<string>> Handle(RemoveUserLimitCommand request, CancellationToken cancellationToken)
    {
        var userChat = await _context
            .UserChats
            .Where(x => x.UserId == request.UserId && x.ChatId == request.ChatId)
            .FirstOrDefaultAsync(cancellationToken);

        if (userChat == null)
        {
            return Response.Fail<string>("Chat not found", HttpStatusCode.NotFound);
        }
        
        if (userChat.RoleId < (int)UserChatRoles.Administrator)
        {
            return Response.Fail<string>("You don't have enough rights", HttpStatusCode.BadRequest);
        }

        
        var userLimit = await _context
            .UserLimits
            .Where(x => x.ChatId == request.ChatId && x.UserId == request.LimitedUserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (userLimit == null)
        {
            return Response.Fail<string>("Limited user not found",HttpStatusCode.NotFound);
        }

        _context.UserLimits.Remove(userLimit);
        await _context.SaveChangesAsync(cancellationToken);

        if (userLimit.LimitType == (int)LimitTypes.Ban)
        {
            return Response.Ok<string>("Ok", "User unbanned");
        }

        return Response.Ok<string>("Ok", "User unmuted");
    }
}

