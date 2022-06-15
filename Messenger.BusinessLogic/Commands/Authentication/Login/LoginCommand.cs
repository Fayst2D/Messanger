using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Authentication.Login;
public class LoginCommand : IRequest<Response<TokenPair>>
{
        public string Email { get; set; }
        public string Password { get; set; }
}

