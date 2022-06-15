using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Messenger.BusinessLogic.Commands.Users.Register;

public class RegisterUserCommand : IRequest<Response<string>>
{
    /// <summary>User password </summary>
    /// <example>123456789</example>
    [Required]
    [MinLength(6)]
    [MaxLength(20)]
    [RegularExpression(@"^[\w\s\d]*$")]
    public string Password { get; set; }
        
    /// <summary>User email</summary>
    /// <example>admin@mail.ru</example>
    [Required]
    [EmailAddress]
    public string Email { get; set; }
        
    /// <summary>User display name.</summary>
    /// <example>Tony Lore</example>
    [Required]
    [MaxLength(50)]
    [RegularExpression(@"^[\w\s]*$")]
    public string Username { get; set; }
}