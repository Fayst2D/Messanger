using System.ComponentModel;

namespace Messenger.BusinessLogic.Commands.UserChats.CreateChannel;

public class CreateChannelRequest
{
    [DefaultValue("Channel")]
    public string Title { get; set; }
}