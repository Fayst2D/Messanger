using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Messages.Edit;

public class EditMessageCommandValidator : AbstractValidator<EditMessageCommand>
{
    public EditMessageCommandValidator()
    {
        RuleFor(x => x.MessageText)
            .Length(1, 2000)
            .NotEmpty();

        RuleFor(x => x.ChatId)
            .NotEmpty();

        RuleFor(x => x.MessageId)
            .NotEmpty();
    }
}