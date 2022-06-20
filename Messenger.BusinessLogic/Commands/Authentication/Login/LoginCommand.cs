using System.ComponentModel;
using MediatR;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Authentication.Login;
public class LoginCommand : IRequest<Response<TokenPair>>
{
        /// <summary>user's email</summary>
        [DefaultValue("user@gmail.com")]
        public string Email { get; set; }
        
        /// <summary>user's password</summary>
        [DefaultValue("123456")]
        public string Password { get; set; }
}

