using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Messenger.BusinessLogic.Commands.Users.Register;

public class RegisterUserCommand : IRequest<Response<string>>
{
    [DefaultValue("123456")]
    public string Password { get; set; }
        
    [DefaultValue("user@gmail.com")]
    public string Email { get; set; }
        
    [DefaultValue("user")]
    public string Username { get; set; }
}