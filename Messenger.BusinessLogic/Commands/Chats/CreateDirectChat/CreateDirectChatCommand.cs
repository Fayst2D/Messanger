using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Chats.CreateDirectChat;

public class CreateDirectChatCommand : BaseRequest, IRequest<Response<Chat>>
{
    public Guid PartnerId { get; set; }
}