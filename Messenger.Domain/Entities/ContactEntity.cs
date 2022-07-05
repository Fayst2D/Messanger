namespace Messenger.Domain.Entities;

public class ContactEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; init; }
    public Guid ContactId { get; init; }

    public UserEntity? User { get; set; }
    public UserEntity? Contact { get; set; }
}