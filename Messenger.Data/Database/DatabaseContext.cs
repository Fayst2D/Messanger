using System.Reflection;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Data.Database;

public class DatabaseContext : DbContext
{
    public DbSet<ChatEntity> Chats { get;set;}
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<UserChatEntity> UserChats { get; set; }
    public DbSet<ContactEntity> Contacts { get; set; }
    public DbSet<UserLimitEntity> UserLimits { get; set; }
    public DbSet<FileEntity> Files { get; set; }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
          
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}