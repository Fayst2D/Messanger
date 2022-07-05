namespace Messenger.BusinessLogic.Models;

public class Chat
{
    public Guid Id { get; set; }
    public string Title { get; init; } = "";
    public int MembersCount { get; set; }
    public int ChatType { get; set; }
}