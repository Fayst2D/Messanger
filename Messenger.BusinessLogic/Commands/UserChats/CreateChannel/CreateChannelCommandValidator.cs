using FluentValidation;

namespace Messenger.BusinessLogic.Commands.UserChats.CreateChannel;

public class CreateChannelCommandValidator : AbstractValidator<CreateChannelCommand>
{
    public CreateChannelCommandValidator()
    {
        RuleFor(x => x.Title)
            .Length(1, 50)
            .NotEmpty();
    }
}