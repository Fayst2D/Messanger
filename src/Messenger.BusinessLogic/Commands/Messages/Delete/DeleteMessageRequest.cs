namespace Messenger.BusinessLogic.Commands.Messages.Delete;

public class DeleteMessageRequest
{
    public Guid MessageId { get; set; }
    public Guid ChatId { get; set; }
}