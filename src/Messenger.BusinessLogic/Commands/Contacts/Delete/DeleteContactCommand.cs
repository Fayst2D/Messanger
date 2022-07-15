using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Contacts.Delete;



public class DeleteContactCommand : BaseRequest, IRequest<Response<Contact>>
{
    public Guid ContactId { get; set; }
}

