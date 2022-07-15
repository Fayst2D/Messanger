using FluentValidation;

namespace Messenger.BusinessLogic.Commands.UserChats.LeaveChat;

public class LeaveChatValidator : AbstractValidator<LeaveChatCommand>
{
    public LeaveChatValidator()
    {
        RuleFor(x => x.ChatId).NotEmpty();
    }
}