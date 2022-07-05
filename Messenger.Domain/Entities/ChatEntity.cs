namespace Messenger.Domain.Entities
{
    public class ChatEntity
    {
        public Guid Id { get; set; }
        //public string Image { get; set; } TODO
        public string Title { get; init; } = "";
        public int MembersCount { get; set; }
        public int ChatType { get; init; }

        public ICollection<MessageEntity>? Messages { get; set; }
        public ICollection<UserChatEntity>? ChatUsers { get; set; }
    }
}
