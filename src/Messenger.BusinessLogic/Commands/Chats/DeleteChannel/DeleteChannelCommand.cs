using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Chats.DeleteChannel;

public class DeleteChannelCommand : BaseRequest, IRequest<Response<Chat>>
{
    public Guid ChatId { get; set; }
}