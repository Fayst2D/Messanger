using MediatR;
using Messanger.Application.Interfaces;
using Messanger.BusinessLogic.Models;
using Messanger.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Messanger.Domain.Entities;

namespace Messanger.BusinessLogic.Commands.Authentication
{
    public class LoginCommand : IRequest<Response<TokenPair>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginHandler : IRequestHandler<LoginCommand, Response<TokenPair>>
    {
        private readonly DatabaseContext _context;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IConfiguration _config;
        public LoginHandler(DatabaseContext context,IJwtGenerator jwtGenerator, IConfiguration config)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
            _config = config;
        }
        public async Task<Response<TokenPair>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == request.Email);

            if (user == null)
            {
                return Response.Fail<TokenPair>("User not found");
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password,user.Password))
            {
                return Response.Fail<TokenPair>("Incorrect password");
            }
                
            var refreshTokenLifeTime = int.Parse(_config["JWTAuth:RefreshTokenLifetime"]);
            var refreshTokenEntity = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                IssuedAt = DateTime.Now,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenLifeTime)
            };
            await _context.RefreshTokens.AddAsync(refreshTokenEntity,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var tokenPair = _jwtGenerator.IssueTokenPair(user.Id,refreshTokenEntity.Id);
            var tokenPairModel = new TokenPair
            {
                AccessToken = tokenPair.AccessToken,
                RefreshToken = tokenPair.RefreshToken
            };

            return Response.Ok("Ok",tokenPairModel);
        }
    }
}
