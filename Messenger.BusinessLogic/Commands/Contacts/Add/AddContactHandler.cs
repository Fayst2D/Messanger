using System.Net;
using MediatR;
using Messenger.BusinessLogic.Hubs;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Data.Database;
using Messenger.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Contacts.Add;

public class AddContactHandler : IRequestHandler<AddContactCommand, Response<Contact>>
{
    private readonly DatabaseContext _context;
    private readonly IHubContext<NotifyHub,IHubClient> _hubContext;

    public AddContactHandler(DatabaseContext context, IHubContext<NotifyHub, IHubClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }
    
    public async Task<Response<Contact>> Handle(AddContactCommand request, CancellationToken cancellationToken)
    {
        //TODO do something with double adding entities
        
        var isContactExists = await _context.Users.AnyAsync(x => x.Id == request.ContactId,cancellationToken);

        if (!isContactExists)
        {
            return Response.Fail<Contact>("Contact not found", HttpStatusCode.NotFound);
        }
        
        if (request.ContactId == request.UserId)
        {
            Response.Fail<Contact>("You can't add yourself", HttpStatusCode.BadRequest);
        }

        var contact = await _context.Users.Where(x => x.Id == request.ContactId).FirstOrDefaultAsync(cancellationToken);
        var contactEntity = new ContactEntity
        {
            Id = Guid.NewGuid(),
            ContactId = request.ContactId,
            UserId = request.UserId
        };
        
        var contactModel = new Contact
        {
            ContactId = contactEntity.ContactId,
            Username = contact.Username,
            Avatar = contact.Avatar,
            Email = contact.Email
        };
        

        var user = await _context.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);
        var userEntity = new ContactEntity //also create contact for added user
        {
            Id = Guid.NewGuid(),
            ContactId = request.UserId,
            UserId = request.ContactId
        };

        var userModel = new Contact
        {
            ContactId = userEntity.ContactId,
            Email = user.Email,
            Username = user.Username
        };
            


        await _context.Contacts.AddAsync(contactEntity, cancellationToken);
        _context.Contacts.Add(userEntity);

        await _context.SaveChangesAsync(cancellationToken);

        await _hubContext.Clients.User(request.UserId.ToString()).UpdateUserContactsAsync(contactModel);
        await _hubContext.Clients.User(request.ContactId.ToString()).UpdateUserContactsAsync(userModel);
        
        return Response.Ok<Contact>("Ok",contactModel);
    }
}