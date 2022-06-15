using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.UserChats.JoinChannel;

public class JoinChannelCommand : BaseRequest, IRequest<Response<Chat>>
{
    public Guid ChatId { get; set; }
}