using System.Net;
using MediatR;
using Messenger.Data;
using Messenger.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Contacts.Delete;

public class DeleteContactHandler : IRequestHandler<DeleteContactCommand, Response<string>>
{
    private readonly DatabaseContext _context;
    
    public DeleteContactHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<string>> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        //TODO do something with double deleting entities
        
        var contactEntity = await _context.Contacts
            .Where(x => x.ContactId == request.ContactId)
            .FirstOrDefaultAsync(cancellationToken);
        
        var mirrorContactEntity = await _context.Contacts //contactEntity for contactUser
            .Where(x => x.UserId == request.ContactId)
            .FirstOrDefaultAsync(cancellationToken);

        if (contactEntity == null || mirrorContactEntity == null)
        {
            return Response.Fail<string>("Contact not found",HttpStatusCode.NotFound);
        }
        
        _context.Remove(contactEntity);
        _context.Remove(mirrorContactEntity);

        await _context.SaveChangesAsync(cancellationToken);

        return Response.Ok("Ok", "Contact deleted");
    }
}