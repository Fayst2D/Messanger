using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Contacts.Delete;

public class DeleteContactCommandValidator : AbstractValidator<DeleteContactCommand>
{
    public DeleteContactCommandValidator()
    {
        RuleFor(x => x.ContactId)
            .NotEmpty();
    }
}