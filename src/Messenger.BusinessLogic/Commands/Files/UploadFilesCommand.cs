using MediatR;
using Microsoft.AspNetCore.Http;

namespace Messenger.BusinessLogic.Commands.Files;

public class UploadFilesCommand : BaseRequest, IRequest<Response<IEnumerable<string>>>
{
    public ICollection<IFormFile> Files { get; set; }
}