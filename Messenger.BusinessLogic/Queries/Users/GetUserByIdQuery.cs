using System.Net;
using MediatR;
using Messenger.Data;
using Messenger.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.Users;

public class GetUserByIdQuery : IRequest<Response<User>>
{
    public Guid UserId { get; set; }
}

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery,Response<User>>
{
    private readonly DatabaseContext _context;

    public GetUserByIdHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context
            .Users
            .AsNoTracking()
            .Where(userEntity => userEntity.Id == request.UserId)
            .Select(userEntity => new User
            {
                Email = userEntity.Email,
                Password = userEntity.Password,
                Username = userEntity.Username
            }).SingleOrDefaultAsync(user => user.UserId == request.UserId, cancellationToken);

        if (user == null)
        {
            return Response.Fail<User>("User not found", HttpStatusCode.NotFound);
        }
        
        return Response.Ok("Ok", user);
    }
}