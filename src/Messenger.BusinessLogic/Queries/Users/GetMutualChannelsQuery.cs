using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data.Database;
using Messenger.Domain.Constants;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.Users;

public class GetMutualChannelsQuery : BaseRequest, IRequest<Response<IEnumerable<Chat>>>
{
    public Guid PartnerId { get; set; }
}

public class GetMutualChannelsHandler : IRequestHandler<GetMutualChannelsQuery, Response<IEnumerable<Chat>>>
{
    private readonly DatabaseContext _context;

    public GetMutualChannelsHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    // TODO optimize it
    public async Task<Response<IEnumerable<Chat>>> Handle(GetMutualChannelsQuery request, CancellationToken cancellationToken)
    {
        var partnerChannels = await _context.UserChats
            .AsNoTracking()
            .Include(x => x.Chat)
            .Where(x => x.UserId == request.PartnerId && x.Chat.ChatType == (int)ChatTypes.Channel)
            .Select(x => new Chat
            {
                Id = x.ChatId,
                ChatType = x.Chat.ChatType,
                Image = x.Chat.Image,
                MembersCount = x.Chat.MembersCount,
                Title = x.Chat.Title
            })
            .ToListAsync(cancellationToken);

        var userChannels = await _context.UserChats
            .AsNoTracking()
            .Include(x => x.Chat)
            .Where(x => x.UserId == request.UserId && x.Chat.ChatType == (int)ChatTypes.Channel)
            .Select(x => new Chat
            {
                Id = x.ChatId,
                ChatType = x.Chat.ChatType,
                Image = x.Chat.Image,
                MembersCount = x.Chat.MembersCount,
                Title = x.Chat.Title
            })
            .ToListAsync(cancellationToken);

        var mutualChannels = new List<Chat>();
        
        foreach (var userChannel in userChannels)
        {
            foreach (var partnerChannel in partnerChannels)
            {
                if (userChannel.Id == partnerChannel.Id)
                {
                    mutualChannels.Add(userChannel);
                }
            }
        }

        return Response.Ok<IEnumerable<Chat>>("Ok", mutualChannels);
    }
}