using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Application.Interfaces
{
    public interface IJwtGenerator
    {
        public IDictionary<string, string> ParseToken(string token);
        public string IssueToken(IDictionary<string, object> claims, TimeSpan lifetime);
        public TokenPair IssueTokenPair(Guid userId, Guid refreshTokenId);
        public record TokenPair(string AccessToken, string RefreshToken);
    }
}
