namespace Messenger.Domain.Entities;

public class UserLimitEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
    public DateTime LimitedAt { get; set; }
    public DateTime UnLimitedAt { get; set; }
    public int LimitType { get; set; }

    public UserEntity User { get; set; }
    public ChatEntity Chat { get; set; }
}