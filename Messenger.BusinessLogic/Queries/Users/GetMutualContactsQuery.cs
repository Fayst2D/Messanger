using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.Users;

public class GetMutualContactsQuery : BaseRequest, IRequest<Response<IEnumerable<Contact>>>
{
    public Guid PartnerId { get; set; }
}

public class GetMutualContactsHandler : IRequestHandler<GetMutualContactsQuery, Response<IEnumerable<Contact>>>
{
    private readonly DatabaseContext _context;

    public GetMutualContactsHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<IEnumerable<Contact>>> Handle(GetMutualContactsQuery request, CancellationToken cancellationToken)
    {
        var userContacts = await _context.Contacts
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId)
            .Include(x => x.Contact)
            .Select(x => new Contact
            {
                ContactId = x.ContactId,
                Email = x.Contact.Email,
                Username = x.User.Username
            })
            .ToListAsync(cancellationToken);

        var partnerContacts = await _context.Contacts
            .AsNoTracking()
            .Where(x => x.UserId == request.PartnerId)
            .Include(x => x.Contact)
            .Select(x => new Contact
            {
                ContactId = x.ContactId,
                Email = x.Contact.Email,
                Username = x.User.Username
            })
            .ToListAsync(cancellationToken);
        
        var mutualContacts = new List<Contact>();
        
        foreach (var userContact in userContacts)
        {
            foreach (var partnerContact in partnerContacts)
            {
                if (userContact.ContactId == partnerContact.ContactId)
                {
                    mutualContacts.Add(userContact);
                }
            }
        }
        
        return Response.Ok<IEnumerable<Contact>>("Ok", mutualContacts);
    }
}