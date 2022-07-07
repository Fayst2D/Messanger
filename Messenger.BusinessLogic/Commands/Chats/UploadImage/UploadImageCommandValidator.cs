using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Chats.UploadImage;

public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{
    public UploadImageCommandValidator()
    {
        RuleFor(x => x.Image).NotEmpty();
        RuleFor(x => x.ChatId).NotEmpty();
    }
}