using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Limits.RemoveUserLimit;

public class RemoveUserLimitValidator : AbstractValidator<RemoveUserLimitCommand>
{
    public RemoveUserLimitValidator()
    {
        RuleFor(x => x.ChatId).NotEmpty();
        RuleFor(x => x.LimitedUserId).NotEmpty();
    }
}