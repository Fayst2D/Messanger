using System.Net;
using AutoMapper;
using MediatR;
using Messenger.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Presentation.Controllers;

public class BaseApiController : ControllerBase
{
    private readonly IMediator _mediator;
    protected readonly IMapper _mapper;

    public BaseApiController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [NonAction]
    protected async Task<IActionResult> Request<T>(IRequest<Response<T>> request, CancellationToken cancellationToken = default) where T : class
    {
        var response = await _mediator.Send(request, cancellationToken);

        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => BadRequest(response),
            HttpStatusCode.Conflict => Conflict(response),
            HttpStatusCode.NotFound => NotFound(response),
            _ => Ok(response)
        };

    }
}

