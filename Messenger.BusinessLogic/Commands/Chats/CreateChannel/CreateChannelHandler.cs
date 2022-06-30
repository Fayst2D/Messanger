using MediatR;
using Messenger.BusinessLogic.Hubs;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.BusinessLogic.Commands.Chats.CreateChannel;


public class CreateChannelHandler : IRequestHandler<CreateChannelCommand, Response<Chat>>
{
    private readonly DatabaseContext _context;
    //private readonly IHubContext<NotifyHub, IHubClient> _hubContext;

    public CreateChannelHandler(DatabaseContext context)
    {
        _context = context;
        //_hubContext = hubContext;
    }
    
    public async Task<Response<Chat>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = new ChatEntity
        {
            Id = Guid.NewGuid(),
            ChatType = (int)ChatTypes.Channel,
            MembersCount = 1,
            Title = request.Title,
        };

        _context.Chats.Add(channel);

        _context.UserChats.Add(new UserChatEntity
        {
            Id = Guid.NewGuid(),
            ChatId = channel.Id,
            UserId = request.UserId,
            RoleId = (int)UserChatRoles.Owner
        });

        await _context.SaveChangesAsync(cancellationToken);

        var chat = new Chat()
        {
            Id = channel.Id,
            MembersCount = channel.MembersCount,
            Title = channel.Title
        };

        //await _hubContext.Clients.User(request.UserId.ToString()).AddToGroup(chat.Id.ToString());
        //await _hubContext.Clients.Group(chat.Id.ToString()).UpdateUserChatsAsync(chat);
        
            

        return Response.Ok("Ok", chat);
    }
}