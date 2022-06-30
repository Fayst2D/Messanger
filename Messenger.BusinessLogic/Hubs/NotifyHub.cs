using Microsoft.AspNetCore.SignalR;

namespace Messenger.BusinessLogic.Hubs;

public class NotifyHub : Hub<IHubClient>
{

    public async Task AddToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}