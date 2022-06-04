using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Domain.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        //public string Image { get; set; } for future

        public ICollection<MessageEntity> Messages { get; set; }
        public ICollection<UserChatEntity> UserChats { get; set; }
        public ICollection<UserEntity> UserFriends { get; set; }
    }
}
