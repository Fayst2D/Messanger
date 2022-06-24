using MediatR;

namespace Messenger.BusinessLogic.Commands.UserChats.LeaveChat;

public class LeaveChatCommand : BaseRequest, IRequest<Response<string>>
{
    public Guid ChatId { get; set; }
}