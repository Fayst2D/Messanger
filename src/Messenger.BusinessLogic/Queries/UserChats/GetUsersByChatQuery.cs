using System.Net;
using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.UserChats;

public class GetUsersByChatQuery : BaseRequest, IRequest<Response<IEnumerable<User>>>
{
    public Guid ChatId { get; init; }
}

public class GetUsersByChatHandler : IRequestHandler<GetUsersByChatQuery, Response<IEnumerable<User>>>
{
    private readonly DatabaseContext _context;
    
    public GetUsersByChatHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<IEnumerable<User>>> Handle(GetUsersByChatQuery request, CancellationToken cancellationToken)
    {
        var userChatEntity = await _context
            .UserChats
            .AsNoTracking()
            .Where(x => x.ChatId == request.ChatId && x.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (userChatEntity == null)
        {
            return Response.Fail<IEnumerable<User>>("chat not found", HttpStatusCode.NotFound);
        }

        var users = _context.UserChats
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.ChatId == request.ChatId)
            .Select(x => new User
            {
                UserId = x.UserId,
                Avatar = x.User.Avatar,
                Email = x.User.Email,
                Username = x.User.Username,
            });
        
        return Response.Ok<IEnumerable<User>>("Ok", users);
    }
}
