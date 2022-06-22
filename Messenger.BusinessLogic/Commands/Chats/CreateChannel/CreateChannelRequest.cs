using System.ComponentModel;

namespace Messenger.BusinessLogic.Commands.Chats.CreateChannel;

public class CreateChannelRequest
{
    [DefaultValue("Channel")]
    public string Title { get; set; }
}