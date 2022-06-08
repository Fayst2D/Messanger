using MediatR;
using Messenger.Data;
using Messenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Messenger.BusinessLogic.Commands.Users
{
    public class RegisterUserCommand : IRequest<Response<string>>
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<string>>
    {
        private readonly DatabaseContext _context;
        public RegisterUserCommandHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Response<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Username = request.Username
            };

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            return Response.Ok("Ok","UserRegistered");
        }
    }
}
