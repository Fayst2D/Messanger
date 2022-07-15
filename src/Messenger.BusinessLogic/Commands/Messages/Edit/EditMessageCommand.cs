using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Messages.Edit;

public class EditMessageCommand : BaseRequest, IRequest<Response<Message>>
{
    public Guid MessageId { get; set; }
    public Guid ChatId { get; set; }
    public string MessageText { get; set; }
}