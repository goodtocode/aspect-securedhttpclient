namespace Goodtocode.SecuredHttpClient.Middleware;

public interface IAccessTokenProvider
{
    Task<string> GetAccessTokenAsync();
}