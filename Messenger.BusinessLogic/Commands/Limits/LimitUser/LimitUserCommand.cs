using MediatR;


namespace Messenger.BusinessLogic.Commands.Limits.LimitUser;



public class LimitUserCommand : BaseRequest, IRequest<Response<string>>
{
    public Guid ChatId { get; set; }
    public Guid LimitedUserId { get; set; }
    public DateTime LimitedAt { get; set; }
    public DateTime UnLimitedAt { get; set; }
    public int LimitType { get; set; }
}

