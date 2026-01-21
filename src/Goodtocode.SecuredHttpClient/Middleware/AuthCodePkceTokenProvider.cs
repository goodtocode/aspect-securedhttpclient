using Goodtocode.SecuredHttpClient.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Goodtocode.SecuredHttpClient.Middleware;

public class AuthCodePkceTokenProvider(IOptions<AuthCodePkceOptions> accessTokenSetting) : AccessTokenProviderBase
{
    private IOptions<AuthCodePkceOptions> AccessTokenSetting { get; } = accessTokenSetting;

    protected override async Task<string> GetNewAccessToken()
    {
        var httpClient = new HttpClient();
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", AccessTokenSetting.Value.ClientId },
            { "code_verifier", AccessTokenSetting.Value.CodeVerifier },
            //{ "code", code }, // ToDo: code is required, using Scope as placeholder
            { "redirect_uri", AccessTokenSetting.Value.RedirectUri.ToString() }
        });

        var response = await httpClient.PostAsync(AccessTokenSetting.Value.TokenUrl, content);
        if (!response.IsSuccessStatusCode) return string.Empty;
        var tokenResponse = JsonSerializer.Deserialize<BearerTokenDto>(await response.Content.ReadAsStringAsync());
        ExpirationDateUtc = DateTime.UtcNow.AddSeconds(tokenResponse?.ExpiresIn ?? 3600);
        return tokenResponse?.AccessToken ?? string.Empty;
    }
}