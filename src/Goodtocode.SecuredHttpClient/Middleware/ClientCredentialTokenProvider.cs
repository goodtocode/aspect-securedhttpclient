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

        // Ensure no null values are passed to the dictionary
        var scope = AccessTokenSetting.Value.Scope ?? string.Empty;
        var clientId = AccessTokenSetting.Value.ClientId ?? string.Empty;
        var clientSecret = AccessTokenSetting.Value.ClientSecret ?? string.Empty;
        var tokenUrl = AccessTokenSetting.Value.TokenUrl ?? string.Empty;

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "scope", scope },
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "grant_type", "client_credentials" }
        });

        var response = await httpClient.PostAsync(tokenUrl, content);
        if (!response.IsSuccessStatusCode) return string.Empty;
        var tokenResponse = JsonSerializer.Deserialize<BearerTokenDto>(await response.Content.ReadAsStringAsync());
        ExpirationDateUtc = DateTime.UtcNow.AddSeconds(tokenResponse?.ExpiresIn ?? 3600);
        return tokenResponse?.AccessToken ?? string.Empty;
    }
}