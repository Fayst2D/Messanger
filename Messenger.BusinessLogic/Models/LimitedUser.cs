namespace Messenger.BusinessLogic.Models;

public class LimitedUser
{
    public Guid UserId { get; set; }
    public string Username { get; init; } = "";
    public string Email { get; init; } = "";
    public int LimitType { get; set; }
}