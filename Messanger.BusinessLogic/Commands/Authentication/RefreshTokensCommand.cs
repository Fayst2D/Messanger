using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Messanger.Application.Interfaces;
using Messanger.BusinessLogic.Models;
using Messanger.DataAccess;
using Messanger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Messanger.BusinessLogic.Commands.Authentication
{
    public class RefreshTokensCommand : IRequest<Response<TokenPair>>
    {
        public string RefreshToken { get; set; }
    }

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
                return Response.Fail<TokenPair>("Invalid refresh token was provided");
            }

            var refreshTokenId = Guid.Parse(refreshTokenClaims["jti"]);
            var refreshTokenEntity = await _context.RefreshTokens.SingleOrDefaultAsync(rt =>rt.Id == refreshTokenId);
            if(refreshTokenEntity == null)
            {
                return Response.Fail<TokenPair>("Provided refresh token has already been used");
            }

            _context.RefreshTokens.Remove(refreshTokenEntity);
            await _context.SaveChangesAsync();

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
            await _context.SaveChangesAsync();

            var tokenPair = _jwtGenerator.IssueTokenPair(userId,newRefreshTokenEntity.Id);
            var tokenPairModel = new TokenPair 
            {
               AccessToken = tokenPair.AccessToken,
               RefreshToken = tokenPair.RefreshToken
            };

            return Response.Ok("Ok",tokenPairModel);
        }
    }
}
