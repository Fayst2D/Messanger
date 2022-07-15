using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Files;

public class UploadFilesCommandValidator : AbstractValidator<UploadFilesCommand>
{
    public UploadFilesCommandValidator()
    {
        RuleFor(x => x.Files).NotEmpty();
    }
}