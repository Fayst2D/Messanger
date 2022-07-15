using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.Messages;


public class SearchMessagesQuery : BaseRequest, IRequest<Response<IEnumerable<Message>>>
{
    public string MessageText { get; set; }
    public Guid ChatId { get; set; }  
}

public class SearchMessagesHandler : IRequestHandler<SearchMessagesQuery, Response<IEnumerable<Message>>>
{
    private readonly DatabaseContext _context;

    public SearchMessagesHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<IEnumerable<Message>>> Handle(SearchMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _context.Messages
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.ChatId == request.ChatId && EF.Functions.Like(x.MessageText, $"%{request.MessageText}%"))
            .OrderBy(x => x.CreatedAt)
            .Select(x => new Message
            {
                Id = x.Id,
                ChatId = x.ChatId,
                UserId = x.UserId,
                Attachment = x.Attachment,
                CreatedAt = x.CreatedAt.ToShortTimeString(),
                MessageText = x.MessageText,
                UserAvatar = x.User.Avatar
            }).Take(100).ToListAsync(cancellationToken);


        return Response.Ok<IEnumerable<Message>>("Ok", messages);
    }
}