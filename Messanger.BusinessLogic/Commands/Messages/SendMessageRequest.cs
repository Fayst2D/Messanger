namespace Messanger.BusinessLogic.Commands.Messages;

public class SendMessageRequest
{
    public string Message { get; set; }
    public Guid ChatId { get; set; }
}