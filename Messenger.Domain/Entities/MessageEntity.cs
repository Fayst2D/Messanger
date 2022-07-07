using System.Security.AccessControl;

namespace Messenger.Domain.Entities
{
    public class MessageEntity
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public Guid ChatId { get; init; }
        public string MessageText { get; set; } = "";
        public string Attachment { get; set; } = "";
        public DateTime CreatedAt { get; init; }

        public UserEntity? User { get; set; }
        public ChatEntity? Chat { get; set; }        
    }
}
