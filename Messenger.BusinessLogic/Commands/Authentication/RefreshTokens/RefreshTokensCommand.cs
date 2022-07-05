using MediatR;
using Messenger.BusinessLogic.Models;


namespace Messenger.BusinessLogic.Commands.Authentication.RefreshTokens
{
    public class RefreshTokensCommand : IRequest<Response<TokenPair>>
    {
        public string RefreshToken { get; set; } = "";
    }
}
