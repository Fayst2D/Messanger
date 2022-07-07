using MediatR;
using Microsoft.AspNetCore.Http;

namespace Messenger.BusinessLogic.Commands.Users.ChangeAvatar;

public class ChangeAvatarCommand : BaseRequest, IRequest<Response<string>>
{
    public IFormFile Avatar { get; set; }
}