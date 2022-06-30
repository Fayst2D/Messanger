using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Hubs;

public interface IHubClient
{

    /// <summary>
    /// Update user's chats via SignalR
    /// </summary>
    Task UpdateUserChatsAsync(Chat chat);

    /// <summary>
    /// Notifies chat's users on the message send via SignalR
    /// </summary>
    Task NotifyOnMessageSendAsync(Message message);

    /// <summary>
    /// Notifies chat's users on the message delete via SignalR
    /// </summary>
    Task NotifyOnMessageDeleteAsync(Message message);
    
    /// <summary>
    /// Notifies chat's users on the message edit via SignalR
    /// </summary>
    Task NotifyOnMessageEditAsync(Message message);

}