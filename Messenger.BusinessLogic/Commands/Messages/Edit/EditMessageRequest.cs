namespace Messenger.BusinessLogic.Commands.Messages.Edit;

public class EditMessageRequest
{
    public Guid MessageId { get; set; }
    public Guid ChatId { get; set; }
    public string MessageText { get; set; }
}