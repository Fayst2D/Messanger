using MediatR;
using Messenger.Data;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Contacts.Add;

public class AddContactHandler : IRequestHandler<AddContactCommand, Response<string>>
{
    private readonly DatabaseContext _context;
    
    public AddContactHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<string>> Handle(AddContactCommand request, CancellationToken cancellationToken)
    {
        //TODO do something with double adding entities
        
        var isContactExists = await _context.Users.AnyAsync(x => x.Id == request.ContactId,cancellationToken);

        if (!isContactExists)
        {
            return Response.Fail<string>("contact not found");
        }
        
        if (request.ContactId == request.UserId)
        {
            Response.Fail<string>("you can't add yourself");
        }

        var contactEntity = new ContactEntity
        {
            Id = Guid.NewGuid(),
            ContactId = request.ContactId,
            UserId = request.UserId
        };

        var userEntity = new ContactEntity //also create contact for added user
        {
            Id = Guid.NewGuid(),
            ContactId = request.UserId,
            UserId = request.ContactId
        };


        await _context.Contacts.AddAsync(contactEntity, cancellationToken);
        _context.Contacts.Add(userEntity);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Response.Ok<string>("Ok","contact added");
    }
}