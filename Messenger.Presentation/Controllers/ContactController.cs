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
[Route("[controller]")]
public class ContactController : BaseApiController
{
    public ContactController(IMediator mediator, IMapper mapper) : base(mediator, mapper) {}
    
    [HttpGet("get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetContacts()
    {
        return await Request(new GetContactsQuery());
    }

    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddContact([FromBody]AddContactRequest addContactRequest)
    {
        var addContactCommand = _mapper.Map<AddContactCommand>(addContactRequest);

        return await Request(addContactCommand);
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContact([FromBody] DeleteContactRequest deleteContactRequest)
    {
        var deleteContactCommand = _mapper.Map<DeleteContactCommand>(deleteContactRequest);

        return await Request(deleteContactCommand);
    }
}