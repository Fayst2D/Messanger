using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Messages.Send;

public class SendMessageCommand : BaseRequest, IRequest<Response<Message>>
{
    public string Message { get; set; }
    public Guid ChatId { get; set; }
}