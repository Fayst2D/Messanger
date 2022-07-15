namespace Messenger.Domain.Entities;


public class UserChatEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; init; }
    public Guid ChatId { get; init; }
    public int RoleId { get; init; }
    
    public UserEntity? User { get; set; }
    public ChatEntity? Chat { get; set; }
}