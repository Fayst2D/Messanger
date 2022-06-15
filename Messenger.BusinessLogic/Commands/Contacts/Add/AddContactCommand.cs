using MediatR;

namespace Messenger.BusinessLogic.Commands.Contacts.Add;


public class AddContactCommand : BaseRequest, IRequest<Response<string>>
{
    public Guid ContactId { get; set; }
}



