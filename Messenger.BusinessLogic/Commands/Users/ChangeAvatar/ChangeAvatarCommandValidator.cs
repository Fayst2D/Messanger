using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Users.ChangeAvatar;

public class ChangeAvatarCommandValidator : AbstractValidator<ChangeAvatarCommand>
{
    public ChangeAvatarCommandValidator()
    {
        RuleFor(x => x.Avatar).NotEmpty();
    }
}