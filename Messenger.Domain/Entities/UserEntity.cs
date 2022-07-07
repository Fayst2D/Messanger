using System.Security.AccessControl;

namespace Messenger.Domain.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = "";
        public string Avatar { get; set; }
        public string Password { get; set; } = "";

        public string Email { get; set; } = "";
        //public string Image { get; set; } TODO

        public ICollection<MessageEntity>? Messages { get; set; }
        public ICollection<UserChatEntity>? UserChats { get; set; }
        public ICollection<UserEntity>? UserContacts { get; set; }
        public ICollection<UserLimitEntity>? UserLimits { get; set; }
    }
}
