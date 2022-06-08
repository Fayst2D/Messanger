namespace Messenger.BusinessLogic.Models;

public class Chat
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int MembersCount { get; set; }
}