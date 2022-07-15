using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Contacts.Add;


public class AddContactCommand : BaseRequest, IRequest<Response<Contact>>
{
    public Guid ContactId { get; set; }
}



