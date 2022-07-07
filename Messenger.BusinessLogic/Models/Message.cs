namespace Messenger.BusinessLogic.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
        public string CreatedAt { get; init; } = "";
        public string MessageText { get; init; } = "";
        public string Attachment { get; set; }
    }
}
