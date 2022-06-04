using System.Security.Claims;
using MediatR;
using Messanger.BusinessLogic.Commands.Authentication;

namespace Messanger.BusinessLogic.Pipelines;

public class UserIdPipe<TIn, TOut> : IPipelineBehavior<TIn,TOut> where TIn : IRequest<TOut>
{
    private readonly HttpContext _httpContext;
    
    public UserIdPipe(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext.HttpContext;
    }
    
    public Task<TOut> Handle(TIn request, CancellationToken cancellationToken, RequestHandlerDelegate<TOut> next)
    {

        if (request is BaseRequest baseRequest)
        {
            var userId = Guid.Parse(_httpContext.User.FindFirst("sub").Value);
            baseRequest.UserId = userId;
        }

        return next();
    }
}