using MediatR;

namespace Messenger.BusinessLogic.Commands.Contacts.Delete;



public class DeleteContactCommand : BaseRequest, IRequest<Response<string>>
{
    public Guid ContactId { get; set; }
}

