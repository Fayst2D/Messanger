using Messanger.Application.Interfaces;
using Messanger.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Application.Services
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly SymmetricSecurityKey _securityKey;
        private readonly TimeSpan _accessTokenLifetime;
        private readonly TimeSpan _refreshTokenLifetime;

        public JwtGenerator(IConfiguration configuration)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _tokenHandler.InboundClaimTypeMap.Clear();
            _tokenHandler.OutboundClaimTypeMap.Clear();

            var jwtSecret = Encoding.ASCII.GetBytes(configuration["JWTAuth:Secret"]);
            _securityKey = new SymmetricSecurityKey(jwtSecret);

            var accessTokenLifetimeInMinutes = int.Parse(configuration["JWTAuth:AccessTokenLifetime"]);
            _accessTokenLifetime = TimeSpan.FromMinutes(accessTokenLifetimeInMinutes);

            var refreshTokenLifetimeInDays = int.Parse(configuration["JwtAuth:RefreshTokenLifetime"]);
            _refreshTokenLifetime = TimeSpan.FromDays(refreshTokenLifetimeInDays);
        }
        public string IssueToken(IDictionary<string, object> claims, TimeSpan lifetime)
        {
            var descriptor = new SecurityTokenDescriptor
            {
                Claims = claims,
                Expires = DateTime.UtcNow.Add(lifetime),
                SigningCredentials = new SigningCredentials(
            _securityKey,
            SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            var tokenObject = handler.CreateToken(descriptor);
            var encodedToken = handler.WriteToken(tokenObject);

            return encodedToken;
        }

        public IJwtGenerator.TokenPair IssueTokenPair(Guid userId,Guid refreshTokenId)
        {
            var accessToken = IssueToken(
                new Dictionary<string, object>
                {
                    {"sub", userId}
                },
                _accessTokenLifetime);

            var refreshToken = IssueToken(new Dictionary<string, object>
                {
                    {"sub", userId},
                    {"jti", refreshTokenId}
                },
                _refreshTokenLifetime);

            return new IJwtGenerator.TokenPair(accessToken, refreshToken);
        }

        public IDictionary<string, string> ParseToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var claimsPrincipal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                return claimsPrincipal.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
