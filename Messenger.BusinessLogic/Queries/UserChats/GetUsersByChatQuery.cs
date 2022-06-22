using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.UserChats;

public class GetUsersByChatQuery : BaseRequest, IRequest<Response<IEnumerable<User>>>
{
    public Guid ChatId { get; set; }
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
        var users = await _context
            .UserChats
            .AsNoTracking()
            .Where(x => x.ChatId == request.ChatId && x.UserId == request.UserId)
            .Include(x => x.User)
            .OrderByDescending(x => x.RoleId)
            .Select(x => new User
            {
                UserId = x.User.Id,
                Email = x.User.Email,
                Username = x.User.Username,
            }).Take(100).ToListAsync(cancellationToken);
        
        return Response.Ok<IEnumerable<User>>("Ok", users);
    }
}
