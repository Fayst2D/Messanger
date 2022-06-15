using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Messages.Delete;

public class DeleteMessageCommand : BaseRequest, IRequest<Response<Message>>
{
    public Guid MessageId { get; set; }
    public Guid ChatId { get; set; }
}