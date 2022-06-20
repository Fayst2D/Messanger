using FluentValidation;

namespace Messenger.BusinessLogic.Commands.Contacts.Add;

public class AddContactCommandValidator : AbstractValidator<AddContactCommand>
{
    public AddContactCommandValidator()
    {
        RuleFor(x => x.ContactId)
            .NotEmpty();
    }
}