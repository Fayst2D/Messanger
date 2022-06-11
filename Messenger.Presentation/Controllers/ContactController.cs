using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.Contacts;
using Messenger.BusinessLogic.Queries.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messanger.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public ContactController(IMediator mediator,IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetContacts()
    {
        return Ok(await _mediator.Send(new GetContactsQuery()));
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddContact([FromBody]AddContactRequest addContactRequest)
    {
        var addContactCommand = _mapper.Map<AddContactCommand>(addContactRequest);

        return Ok(await _mediator.Send(addContactCommand));
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteContact([FromBody] DeleteContactRequest deleteContactRequest)
    {
        var deleteContactCommand = _mapper.Map<DeleteContactCommand>(deleteContactRequest);

        return Ok(await _mediator.Send(deleteContactCommand));
    }
}