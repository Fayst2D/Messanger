using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Limits.LimitUser;

public class LimitUserCommandValidator : AbstractValidator<LimitUserCommand>
{
    public LimitUserCommandValidator()
    {
        RuleFor(x => x.ChatId)
            .NotEmpty();

        RuleFor(x => x.LimitedUserId)
            .NotEmpty();

        RuleFor(x => x.UnLimitedAt)
            .NotEmpty();
    }
}