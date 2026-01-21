using System.Net.Http.Headers;

namespace Goodtocode.SecuredHttpClient.Middleware;

public class TokenHandler(IAccessTokenProvider bearerToken) : DelegatingHandler
{
    private readonly IAccessTokenProvider _accessToken = bearerToken;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _accessToken.GetAccessTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}