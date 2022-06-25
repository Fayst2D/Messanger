using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.Contacts;
using Messenger.BusinessLogic.Commands.Contacts.Add;
using Messenger.BusinessLogic.Commands.Contacts.Delete;
using Messenger.BusinessLogic.Queries.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Messenger.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/contacts")]
public class ContactsController : BaseApiController
{
    public ContactsController(IMediator mediator, IMapper mapper) : base(mediator, mapper) {}
    
    /// <summary>
    /// Get user's contacts
    /// </summary>
    /// <returns>Status codes: 200</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetContacts()
    {
        return await Request(new GetContactsQuery());
    }

    /// <summary>
    /// add contact by ID
    /// </summary>
    /// <param name="contactId">ID of adding contact</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 400, 404, 422</returns>
    [HttpPost("{contactId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddContact([FromRoute]Guid contactId, CancellationToken cancellationToken)
    {
        var addContactCommand = new AddContactCommand
        {
            ContactId = contactId
        };

        return await Request(addContactCommand, cancellationToken);
    }

    /// <summary>
    /// Delete chosen contact
    /// </summary>
    /// <param name="contactId">Contact's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 404, 422</returns>
    [HttpDelete("{contactId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DeleteContact([FromRoute]Guid contactId, CancellationToken cancellationToken)
    {
        var deleteContactCommand = new DeleteContactCommand
        {
            ContactId = contactId
        };

        return await Request(deleteContactCommand, cancellationToken);
    }
}