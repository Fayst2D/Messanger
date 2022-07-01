using MediatR;
using Microsoft.AspNetCore.Http;

namespace Messenger.BusinessLogic.Commands.Files;

public class UploadFilesCommand : BaseRequest, IRequest<Response<string>>
{
    public List<IFormFile> Files { get; set; }
}