using MediatR;
using Messanger.BusinessLogic.Models;
using Messanger.DataAccess;
using Messanger.Domain.Entities;
using Messanger.Domain.Enums;

namespace Messanger.BusinessLogic.Commands.UserChats;

public class CreateChannelCommand : BaseRequest, IRequest<Response<Chat>>
{
    public string Title { get; set; }
}

public class CreateChannelHandler : IRequestHandler<CreateChannelCommand, Response<Chat>>
{
    private readonly DatabaseContext _context;

    public CreateChannelHandler(DatabaseContext context)
    {
        _context = context;
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
            RoleId = (int)UserRoles.Owner
        });

        await _context.SaveChangesAsync();

        var chat = new Chat()
        {
            Id = channel.Id,
            MembersCount = channel.MembersCount,
            Title = channel.Title
        };

        return Response.Ok("Ok", chat);
    }
}