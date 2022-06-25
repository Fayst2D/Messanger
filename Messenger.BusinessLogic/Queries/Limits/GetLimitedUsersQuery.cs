using System.Net;
using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.Limits;

public class GetLimitedUsersQuery : BaseRequest, IRequest<Response<IEnumerable<LimitedUser>>>
{
    public Guid ChatId { get; set; }
}

public class GetLimitedUsersHandler : IRequestHandler<GetLimitedUsersQuery, Response<IEnumerable<LimitedUser>>>
{
    private readonly DatabaseContext _context;

    public GetLimitedUsersHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<IEnumerable<LimitedUser>>> Handle(GetLimitedUsersQuery request, CancellationToken cancellationToken)
    {
        var userChatEntity = await _context.UserChats
            .AsNoTracking()
            .Where(x => x.ChatId == request.ChatId)
            .Where(x => x.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        

        if (userChatEntity == null)
        {
            return Response.Fail<IEnumerable<LimitedUser>>("Chat not found", HttpStatusCode.NotFound);
        }

        if (userChatEntity.RoleId < (int)UserChatRoles.Administrator)
        {
            return Response.Fail<IEnumerable<LimitedUser>>("You don't have enough rights", HttpStatusCode.BadRequest);
        }
        
        var limitedUsers = await _context.UserLimits
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.ChatId == request.ChatId)
            .Select(x => new LimitedUser
            {
                UserId = x.User.Id,
                Email = x.User.Email,
                Username = x.User.Username,
                LimitType = x.LimitType
            }).Take(100).ToListAsync(cancellationToken);
        
        return Response.Ok<IEnumerable<LimitedUser>>("Ok", limitedUsers);
    }
}
