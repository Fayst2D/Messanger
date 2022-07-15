using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Chats.CreateChannel;

public class CreateChannelCommand : BaseRequest, IRequest<Response<Chat>>
{
    public string Title { get; set; }
}