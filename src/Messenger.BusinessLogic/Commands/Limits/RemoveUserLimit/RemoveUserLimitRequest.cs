namespace Messenger.BusinessLogic.Commands.Limits.RemoveUserLimit;

public class RemoveUserLimitRequest
{
    public Guid ChatId { get; set; }
    public Guid LimitedUserId { get; set; }
}