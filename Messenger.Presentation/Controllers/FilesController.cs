using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Presentation.Controllers;

[Route("files")]
[Authorize]
public class FilesController : BaseApiController
{
    public FilesController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    /// <summary>
    /// Uploads files to the local storage
    /// </summary>
    /// <param name="files">uploaded files</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 400</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFile(List<IFormFile> files, CancellationToken cancellationToken)
    {
        var uploadFilesCommand = new UploadFilesCommand
        {
            Files = files
        };

        return await Request(uploadFilesCommand, cancellationToken);
    }
}