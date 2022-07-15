using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Limits.RemoveUserLimit;

public class RemoveUserLimitCommand : BaseRequest, IRequest<Response<string>>
{
    public Guid ChatId { get; set; }
    public Guid LimitedUserId { get; set; }
}