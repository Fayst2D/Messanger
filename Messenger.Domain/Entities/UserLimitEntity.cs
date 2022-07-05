namespace Messenger.Domain.Entities;

public class UserLimitEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; init; }
    public Guid ChatId { get; init; }
    public DateTime LimitedAt { get; init; }
    public DateTime UnLimitedAt { get; init; }
    public int LimitType { get; init; }

    public UserEntity? User { get; set; }
    public ChatEntity? Chat { get; set; }
}