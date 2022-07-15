using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Messages.Send;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.MessageText)
            .Length(1, 2000)
            .NotEmpty();

        RuleFor(x => x.ChatId)
            .NotEmpty();
    }
}