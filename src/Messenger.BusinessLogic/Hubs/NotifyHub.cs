using Messenger.Data.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Hubs;

[Authorize]
public class NotifyHub : Hub<IHubClient>
{
    private readonly DatabaseContext _context;
    
    public NotifyHub(DatabaseContext context)
    {
        _context = context;
    }
    
    public override Task OnConnectedAsync()
    {
        var user = _context.Users
            .Include(x => x.UserChats)
            .FirstOrDefault(x => x.Id == Guid.Parse(Context.User.FindFirst("sub")!.Value));
        
        foreach (var item in user.UserChats)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, item.ChatId.ToString());
        }
        
        return base.OnConnectedAsync();
    }

    public async Task AddToChat(Guid chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task RemoveFromChat(Guid chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
}