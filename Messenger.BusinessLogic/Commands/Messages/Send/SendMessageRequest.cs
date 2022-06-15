namespace Messenger.BusinessLogic.Commands.Messages.Send;

public class SendMessageRequest
{
    public string Message { get; set; }
    public Guid ChatId { get; set; }
}