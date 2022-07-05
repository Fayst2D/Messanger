using System.Net;
using MediatR;
using Messenger.BusinessLogic.Hubs;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Data.Database;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Messages.Delete;

public class DeleteMessageHandler : IRequestHandler<DeleteMessageCommand, Response<Message>>
{
    private readonly DatabaseContext _context;
    private readonly IHubContext<NotifyHub, IHubClient> _hubContext;

    public DeleteMessageHandler(DatabaseContext context, IHubContext<NotifyHub, IHubClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
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
            return Response.Fail<Message>("Chat not found", HttpStatusCode.NotFound);
        }

        var messageEntity = chatEntity.Messages.First(x => x.Id == request.MessageId);

        _context.Messages.Remove(messageEntity);
        await _context.SaveChangesAsync(cancellationToken);

        var message = new Message
        {
            ChatId = messageEntity.ChatId,
            CreatedAt = messageEntity.CreatedAt.ToShortTimeString(),
            Id = messageEntity.Id,
            MessageText = messageEntity.MessageText,
            UserId = messageEntity.UserId
        };

        await _hubContext.Clients.Group(request.ChatId.ToString()).NotifyOnMessageDeleteAsync(message);

        return Response.Ok("Ok", message);
    }
}