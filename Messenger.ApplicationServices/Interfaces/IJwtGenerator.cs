namespace Messenger.ApplicationServices.Interfaces;

public interface IJwtGenerator
{
    public IDictionary<string, string>? ParseToken(string token);
    public string IssueToken(IDictionary<string, object> claims, TimeSpan lifetime);
    public TokenPair IssueTokenPair(Guid userId, Guid refreshTokenId);
    public record TokenPair(string AccessToken, string RefreshToken);
}