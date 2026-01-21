namespace Goodtocode.SecuredHttpClient.Middleware;

public abstract class AccessTokenProviderBase() : IAccessTokenProvider
{    
    protected string Token { get; set; } = string.Empty;
    protected DateTime ExpirationDateUtc { get; set; }

    protected bool TokenIsExpired => string.IsNullOrWhiteSpace(Token) || (ExpirationDateUtc - DateTime.UtcNow).TotalMinutes < 1;

    public async Task<string> GetAccessTokenAsync()
    {
        if (TokenIsExpired)
            Token = await GetNewAccessToken();
        return Token;
    }

    protected abstract Task<string> GetNewAccessToken();
}
