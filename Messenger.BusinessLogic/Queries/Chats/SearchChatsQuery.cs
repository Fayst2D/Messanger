using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.Chats;

public class SearchChatsQuery : IRequest<Response<IEnumerable<Chat>>>
{
    public string Title { get; set; }
}

public class SearchChatsHandler : IRequestHandler<SearchChatsQuery, Response<IEnumerable<Chat>>>
{
    private readonly DatabaseContext _context;

    public SearchChatsHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<IEnumerable<Chat>>> Handle(SearchChatsQuery request, CancellationToken cancellationToken)
    {
        var chats = await _context.Chats
            .Where(x => x.ChatType == (int)ChatTypes.Channel)
            .Where(x => EF.Functions.Like(x.Title, $"%{request.Title}%"))
            .Select(x => new Chat
            {
                Id = x.Id,
                Title = x.Title,
                MembersCount = x.MembersCount,
                ChatType = x.ChatType
            }).Take(100).ToListAsync(cancellationToken);
        
        return Response.Ok<IEnumerable<Chat>>("Ok", chats);
    }
}