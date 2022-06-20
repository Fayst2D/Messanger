using FluentValidation;

namespace Messenger.BusinessLogic.Commands.UserChats.JoinChannel;

public class JoinChannelCommandValidator : AbstractValidator<JoinChannelCommand>
{
    public JoinChannelCommandValidator()
    {
        RuleFor(x => x.ChatId)
            .NotEmpty();
    }
}