
using MediatR;
using Messenger.BusinessLogic;
using Microsoft.AspNetCore.Http;


namespace Messenger.BusinessLogic.Pipelines;

public class UserIdPipe<TIn, TOut> : IPipelineBehavior<TIn,TOut> where TIn : IRequest<TOut>
{
    private readonly HttpContext _httpContext;
    
    public UserIdPipe(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }
    
    public async Task<TOut> Handle(TIn request, CancellationToken cancellationToken, RequestHandlerDelegate<TOut> next)
    {

        if (request is BaseRequest baseRequest)
        {
            var userId = Guid.Parse(_httpContext.User.FindFirst("sub").Value);
            baseRequest.UserId = userId;
        }

        return await next();
    }
}