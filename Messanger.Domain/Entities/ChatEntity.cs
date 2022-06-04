using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Domain.Entities
{
    public class ChatEntity
    {
        public Guid Id { get; set; }
        //public string Image { get; set; }
        public string Title { get; set; }
        public int MembersCount { get; set; }
        public int ChatType { get; set; }

        public ICollection<MessageEntity> Messages {get;set;}
        public ICollection<UserChatEntity> ChatUsers {get;set;}
    }
}
