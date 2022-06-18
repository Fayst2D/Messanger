using System.Net;
using MediatR;
using Messenger.Data;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Messenger.ApplicationServices.Interfaces;
using Messenger.BusinessLogic.Models;

namespace Messenger.BusinessLogic.Commands.Authentication.RefreshTokens;

public class RefreshTokensHandler : IRequestHandler<RefreshTokensCommand ,Response<TokenPair>>
    {
        private readonly DatabaseContext _context;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IConfiguration _config;
        public RefreshTokensHandler(DatabaseContext context, IJwtGenerator jwtGenerator, IConfiguration config)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
            _config = config;
        }
        public async Task<Response<TokenPair>> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenClaims = _jwtGenerator.ParseToken(request.RefreshToken);

            if(refreshTokenClaims == null)
            {
                return Response.Fail<TokenPair>("Invalid refresh token was provided", HttpStatusCode.BadRequest);
            }

            var refreshTokenId = Guid.Parse(refreshTokenClaims["jti"]);
            var refreshTokenEntity = await _context.RefreshTokens.SingleOrDefaultAsync(rt =>rt.Id == refreshTokenId,cancellationToken);
            if(refreshTokenEntity == null)
            {
                return Response.Fail<TokenPair>("Provided refresh token has already been used", HttpStatusCode.NotFound);
            }

            _context.RefreshTokens.Remove(refreshTokenEntity);
            await _context.SaveChangesAsync(cancellationToken);

            var userId = Guid.Parse(refreshTokenClaims["sub"]);
            var refreshTokenLifeTime = int.Parse(_config["JWTAuth:RefreshTokenLifeTime"]);
            var newRefreshTokenEntity = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenLifeTime),
                IssuedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(newRefreshTokenEntity);
            await _context.SaveChangesAsync(cancellationToken);

            var tokenPair = _jwtGenerator.IssueTokenPair(userId,newRefreshTokenEntity.Id);
            var tokenPairModel = new TokenPair 
            {
               AccessToken = tokenPair.AccessToken,
               RefreshToken = tokenPair.RefreshToken
            };

            return Response.Ok("Ok",tokenPairModel);
        }
    }