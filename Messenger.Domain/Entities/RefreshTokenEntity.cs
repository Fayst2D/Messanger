namespace Messenger.Domain.Entities
{
    public class RefreshTokenEntity
    {
        public Guid Id { get; init; }
        public Guid UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime IssuedAt { get; set; }

    }
}
