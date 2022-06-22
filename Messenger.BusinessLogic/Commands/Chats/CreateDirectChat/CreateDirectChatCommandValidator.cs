using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Chats.CreateDirectChat;

public class CreateDirectChatCommandValidator : AbstractValidator<CreateDirectChatCommand>
{
    public CreateDirectChatCommandValidator()
    {
        RuleFor(x => x.PartnerId).NotEmpty();
    }
}