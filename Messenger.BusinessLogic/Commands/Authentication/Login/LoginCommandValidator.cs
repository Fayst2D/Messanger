using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Authentication.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Password)
            .Length(6, 20)
            .Matches(@"^[\w\s\d]*$")
            .NotEmpty();
    }
}