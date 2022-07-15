using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Messages.Delete;

public class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
{
    public DeleteMessageCommandValidator()
    {
        RuleFor(x => x.ChatId)
            .NotEmpty();

        RuleFor(x => x.MessageId)
            .NotEmpty();
    }   
}