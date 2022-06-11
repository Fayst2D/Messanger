namespace Messenger.Domain.Entities;

public class ContactEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ContactId { get; set; }

    public UserEntity User { get; set; }
    public UserEntity Contact { get; set; }
}