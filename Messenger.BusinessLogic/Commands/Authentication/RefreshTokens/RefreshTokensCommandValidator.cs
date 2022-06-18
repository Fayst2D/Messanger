using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Authentication.RefreshTokens;

public class RefreshTokensCommandValidator : AbstractValidator<RefreshTokensCommand>
{
    public RefreshTokensCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}