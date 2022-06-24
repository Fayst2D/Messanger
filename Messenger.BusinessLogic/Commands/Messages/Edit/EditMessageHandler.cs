using System.Net;
using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Messages.Edit;

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
            return Response.Fail<Message>("Chat not found", HttpStatusCode.NotFound);
        }

        var messageEntity = chatEntity.Messages.First(x => x.Id == request.MessageId);
        messageEntity.MessageText = request.MessageText;
        

        _context.Messages.Update(messageEntity);
        await _context.SaveChangesAsync(cancellationToken);

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