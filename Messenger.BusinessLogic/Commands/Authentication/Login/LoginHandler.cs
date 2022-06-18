using System.Net;
using MediatR;
using Messenger.ApplicationServices.Interfaces;
using Messenger.BusinessLogic.Models;
using Messenger.Data;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Messenger.BusinessLogic.Commands.Authentication.Login;

public class LoginHandler : IRequestHandler<LoginCommand, Response<TokenPair>>
{
    private readonly DatabaseContext _context;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IConfiguration _config;

    public LoginHandler(DatabaseContext context, IJwtGenerator jwtGenerator, IConfiguration config)
    {
        _context = context;
        _jwtGenerator = jwtGenerator;
        _config = config;
    }

    public async Task<Response<TokenPair>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == request.Email, cancellationToken);

        if (user == null)
        {
            return Response.Fail<TokenPair>("User not found",HttpStatusCode.NotFound);
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Response.Fail<TokenPair>("Incorrect password", HttpStatusCode.BadRequest);
        }

        var refreshTokenLifeTime = int.Parse(_config["JWTAuth:RefreshTokenLifetime"]);
        var refreshTokenEntity = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            IssuedAt = DateTime.Now,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenLifeTime)
        };
        await _context.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var tokenPair = _jwtGenerator.IssueTokenPair(user.Id, refreshTokenEntity.Id);
        var tokenPairModel = new TokenPair
        {
            AccessToken = tokenPair.AccessToken,
            RefreshToken = tokenPair.RefreshToken
        };

        return Response.Ok("Ok", tokenPairModel);
    }
}