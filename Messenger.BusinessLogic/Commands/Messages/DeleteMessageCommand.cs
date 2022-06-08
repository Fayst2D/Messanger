using MediatR;
using Messenger.Data;
using Messenger.Domain.Entities;
using Messenger.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Messages;

public class DeleteMessageRequest
{
    public Guid MessageId { get; set; }
    public Guid ChatId { get; set; }
}

public class DeleteMessageCommand : BaseRequest, IRequest<Response<Message>>
{
    public Guid MessageId { get; set; }
    public Guid ChatId { get; set; }
}


public class DeleteMessageHandler : IRequestHandler<DeleteMessageCommand, Response<Message>>
{
    private readonly DatabaseContext _context;

    public DeleteMessageHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response<Message>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {

        var chatEntity = await _context.UserChats
            .Where(x => x.UserId == request.UserId && x.ChatId == request.ChatId)
            .Include(x => x.Chat)
            .ThenInclude(x => x.Messages)
            .Select(x => x.Chat)
            .FirstOrDefaultAsync(cancellationToken);

        if (chatEntity == null)
        {
            Response.Fail<Message>("chat not found");
        }

        var messageEntity = chatEntity.Messages.First(x => x.Id == request.MessageId);

        if (messageEntity == null)
        {
            Response.Fail<Message>("message not found");
        }

        _context.Messages.Remove(messageEntity);
        await _context.SaveChangesAsync();

        var message = new Message
        {
            ChatId = messageEntity.ChatId,
            CreatedAt = messageEntity.CreatedAt.ToShortTimeString(),
            Id = messageEntity.Id,
            MessageText = messageEntity.MessageText,
            UserId = messageEntity.UserId
        };

        return Response.Ok("Ok", message);
    }
}