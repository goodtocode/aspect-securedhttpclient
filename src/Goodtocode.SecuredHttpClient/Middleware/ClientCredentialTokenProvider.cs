using Goodtocode.SecuredHttpClient.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Goodtocode.SecuredHttpClient.Middleware;


public class ClientCredentialTokenProvider(IOptions<ClientCredentialOptions> accessTokenSetting) : AccessTokenProviderBase
{
    private IOptions<ClientCredentialOptions> AccessTokenSetting { get; } = accessTokenSetting;

    protected override async Task<string> GetNewAccessToken()
    {
        var httpClient = new HttpClient();
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "scope", AccessTokenSetting.Value.Scope },
            { "client_id", AccessTokenSetting.Value.ClientId },
            { "client_secret", AccessTokenSetting.Value.ClientSecret },
            { "grant_type", "client_credentials" }
        });

        var response = await httpClient.PostAsync(AccessTokenSetting.Value.TokenUrl, content);
        if (!response.IsSuccessStatusCode) return string.Empty;
        var tokenResponse = JsonSerializer.Deserialize<BearerTokenDto>(await response.Content.ReadAsStringAsync());
        ExpirationDateUtc = DateTime.UtcNow.AddSeconds(tokenResponse?.ExpiresIn ?? 3600);
        return tokenResponse?.AccessToken ?? string.Empty;
    }
}