using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Users.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Password)
            .Matches(@"^[\w\s\d]*$")
            .Length(6, 20)
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Username)
            .Matches(@"^[\w\s]*$")
            .NotEmpty()
            .MaximumLength(50);
    }
}