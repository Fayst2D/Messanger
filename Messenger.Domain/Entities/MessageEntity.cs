using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Entities
{
    public class MessageEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
        public string MessageText { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserEntity User { get; set; }
        public ChatEntity Chat { get; set; }        
    }
}
