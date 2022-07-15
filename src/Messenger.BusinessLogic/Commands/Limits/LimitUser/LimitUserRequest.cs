namespace Messenger.BusinessLogic.Commands.Limits.LimitUser;

public class LimitUserRequest
{
    public Guid ChatId { get; set; }
    public Guid LimitedUserId { get; set; }
    public DateTime UnLimitedAt{ get; set; }
}