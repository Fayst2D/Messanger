using System.Net;
using MediatR;
using Messenger.BusinessLogic.Hubs;
using Messenger.BusinessLogic.Models;
using Messenger.Data.Database;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Contacts.Delete;

public class DeleteContactHandler : IRequestHandler<DeleteContactCommand, Response<Contact>>
{
    private readonly DatabaseContext _context;
    private readonly IHubContext<NotifyHub,IHubClient> _hubContext;
    
    public DeleteContactHandler(DatabaseContext context, IHubContext<NotifyHub, IHubClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }
    
    public async Task<Response<Contact>> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        //TODO do something with double deleting entities
        
       
        var contactEntity = await _context.Contacts
            .Include(x => x.Contact)
            .Where(x => x.ContactId == request.ContactId)
            .FirstOrDefaultAsync(cancellationToken);
        
        
        var userContactEntity = await _context.Contacts //contactEntity for contactUser
            .Include(x => x.Contact)
            .Where(x => x.UserId == request.ContactId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (contactEntity == null || userContactEntity == null)
        {
            return Response.Fail<Contact>("Contact not found",HttpStatusCode.NotFound);
        }
        
        var userContactModel = new Contact
        {
            ContactId = userContactEntity.ContactId,
            Username = userContactEntity.Contact.Username,
            Avatar = userContactEntity.Contact.Avatar,
            Email = userContactEntity.Contact.Email
        };
        
        var contactModel = new Contact
        {
            ContactId = contactEntity.ContactId,
            Username = contactEntity.Contact.Username,
            Avatar = contactEntity.Contact.Avatar,
            Email = contactEntity.Contact.Email
        };

        
        
        _context.Remove(contactEntity);
        _context.Remove(userContactEntity);

        await _context.SaveChangesAsync(cancellationToken);

        await _hubContext.Clients.User(request.UserId.ToString()).UpdateUserContactsAsync(contactModel);
        await _hubContext.Clients.User(request.ContactId.ToString()).UpdateUserContactsAsync(userContactModel);

        return Response.Ok("Ok", contactModel);
    }
}