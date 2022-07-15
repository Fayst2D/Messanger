using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.UserChats.LeaveChat;

public class LeaveChatCommand : BaseRequest, IRequest<Response<Chat>>
{
    public Guid ChatId { get; set; }
}