using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.Contacts;



public class GetContactsQuery : BaseRequest, IRequest<Response<IEnumerable<Contact>>>
{
    
}

public class GetContactsHandler : IRequestHandler<GetContactsQuery, Response<IEnumerable<Contact>>>
{
    private readonly DatabaseContext _context;
    
    public GetContactsHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<IEnumerable<Contact>>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
    {
        var contacts = await _context.Contacts
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId)
            .Include(x => x.Contact)
            .Select(x => new Contact
            {
                ContactId = x.ContactId,
                Email = x.Contact!.Email,
                Username = x.Contact.Username
            }).Take(100).ToListAsync(cancellationToken);

        return Response.Ok<IEnumerable<Contact>>("Ok",contacts);
    }
}