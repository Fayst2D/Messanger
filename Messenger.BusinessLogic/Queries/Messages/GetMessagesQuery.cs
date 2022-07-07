using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data.Database;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.Messages;

public class GetMessagesQuery : BaseRequest, IRequest<Response<IEnumerable<Message>>>
{
    public Guid ChatId { get; init; }
}

public class GetMessagesHandler : IRequestHandler<GetMessagesQuery, Response<IEnumerable<Message>>>
{
    private readonly DatabaseContext _context;

    public GetMessagesHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response<IEnumerable<Message>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _context
            .Messages
            .AsNoTracking()
            .Where(x => x.ChatId == request.ChatId)
            .OrderBy(x => x.CreatedAt)
            .Select(messageEntity => new Message
            {
                Id = messageEntity.Id,
                ChatId = messageEntity.ChatId,
                UserId = messageEntity.UserId,
                MessageText = messageEntity.MessageText,
                CreatedAt = messageEntity.CreatedAt.ToShortTimeString(),
                Attachment = messageEntity.Attachment  
            }).Take(100).ToListAsync(cancellationToken);

        
        return Response.Ok<IEnumerable<Message>>("Ok",messages);
    }
}