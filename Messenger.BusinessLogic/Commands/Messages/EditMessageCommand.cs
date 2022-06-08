using MediatR;
using Messenger.Data;
using Messenger.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Messages;

public class EditMessageRequest
{
    public Guid MessageId { get; set; }
    public Guid ChatId { get; set; }
    public string MessageText { get; set; }
}

public class EditMessageCommand : BaseRequest, IRequest<Response<Message>>
{
    public Guid MessageId { get; set; }
    public Guid ChatId { get; set; }
    public string MessageText { get; set; }
}


public class EditMessageHandler : IRequestHandler<EditMessageCommand, Response<Message>>
{
    private readonly DatabaseContext _context;

    public EditMessageHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response<Message>> Handle(EditMessageCommand request, CancellationToken cancellationToken)
    {

        var chatEntity = await _context.UserChats
            .Where(x => x.UserId == request.UserId && x.ChatId == request.ChatId)
            .Include(x => x.Chat)
            .ThenInclude(x => x.Messages)
            .Select(x => x.Chat)
            .FirstOrDefaultAsync(cancellationToken);

        if (chatEntity == null)
        {
            return Response.Fail<Message>("chat not found");
        }

        var messageEntity = chatEntity.Messages.First(x => x.Id == request.MessageId);
        messageEntity.MessageText = request.MessageText;
        

        _context.Messages.Update(messageEntity);
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