using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Domain.Entities;


public class UserChatEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
    public int RoleId { get; set; }
    public UserEntity User { get; set; }
    public ChatEntity Chat { get; set; }
}