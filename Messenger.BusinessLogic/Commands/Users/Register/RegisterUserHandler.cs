using System.Net;
using MediatR;
using Messenger.Data;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Users.Register;


public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Response<string>>
{
    private readonly DatabaseContext _context;

    public RegisterUserHandler(DatabaseContext context)
    {
        _context = context;
    }


    public async Task<Response<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailTaken = await _context.Users.AnyAsync(x => x.Email == request.Email,cancellationToken);

        if (emailTaken)
        {
            return Response.Fail<string>("Email is already taken",HttpStatusCode.Conflict);
        }

        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Username = request.Username
        };

        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return Response.Ok("Ok", "User registered");
    }
}

