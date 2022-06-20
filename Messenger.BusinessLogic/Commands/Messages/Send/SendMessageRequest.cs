using System.ComponentModel;

namespace Messenger.BusinessLogic.Commands.Messages.Send;

public class SendMessageRequest
{
    [DefaultValue("message")]
    public string MessageText { get; set; }
    public Guid ChatId { get; set; }
}