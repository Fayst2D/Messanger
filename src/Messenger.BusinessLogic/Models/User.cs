namespace Messenger.BusinessLogic.Models;

public class User
{
    public Guid UserId { get; init; }
    public string Username { get; init; } = "";
    public string Avatar { get; set; }
    public string Email { get; init; } = "";

    //public string Password { get; set; }
}