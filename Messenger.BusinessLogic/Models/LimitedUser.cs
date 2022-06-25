namespace Messenger.BusinessLogic.Models;

public class LimitedUser
{
    public Guid UserId { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    public int LimitType { get; set; }
}