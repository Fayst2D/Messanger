using System.Net;
using MediatR;
using Messenger.BusinessLogic.Hubs;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Data.Database;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Messages.Send;

public class SendMessageHandler : IRequestHandler<SendMessageCommand, Response<Message>>
{
    private readonly DatabaseContext _context;
    private readonly IHubContext<NotifyHub, IHubClient> _hubContext;

    public SendMessageHandler(DatabaseContext context, IHubContext<NotifyHub, IHubClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<Response<Message>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var muteEntity = await _context.UserLimits
            .Where(x => x.ChatId == request.ChatId && x.UserId == request.UserId &&
                        x.LimitType == (int)LimitTypes.Mute)
            .FirstOrDefaultAsync(cancellationToken);
        
        var userEntity = await _context.Users
            .FirstOrDefaultAsync(cancellationToken);

        if (muteEntity != null)
        {
            return Response.Fail<Message>($"You will be unmute on {muteEntity.UnLimitedAt}", HttpStatusCode.BadRequest);
        }

        
        var messageEntity = new MessageEntity
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ChatId = request.ChatId,
            CreatedAt = DateTime.Now,
            MessageText = request.MessageText,
            Attachment = request.Attachment,
        };

        _context.Messages.Add(messageEntity);
        await _context.SaveChangesAsync(cancellationToken);

        var message = new Message
        {
            Id = messageEntity.Id,
            UserId = messageEntity.UserId,
            ChatId = messageEntity.ChatId,
            CreatedAt = messageEntity.CreatedAt.ToString(),
            MessageText = messageEntity.MessageText,
            Attachment = messageEntity.Attachment,
            UserAvatar = userEntity.Avatar
        };

        await _hubContext.Clients.Group(request.ChatId.ToString()).NotifyOnMessageSendAsync(message);
        

        return Response.Ok("Ok", message);
    }
}

