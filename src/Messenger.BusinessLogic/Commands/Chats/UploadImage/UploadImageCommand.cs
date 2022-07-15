using MediatR;
using Microsoft.AspNetCore.Http;

namespace Messenger.BusinessLogic.Commands.Chats.UploadImage;

public class UploadImageCommand : BaseRequest, IRequest<Response<string>>
{
    public Guid ChatId { get; set; }
    public IFormFile Image { get; set; }
}