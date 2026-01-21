using Microsoft.Extensions.Options;

namespace Goodtocode.SecuredHttpClient.Options;

public class AuthCodePkceOptions
{
    public AuthCodePkceOptions() { }

    public AuthCodePkceOptions(string clientId, string codeVerifier, Uri redirectUri, Uri tokenUrl, string scope)
    {
        ClientId = clientId;
        CodeVerifier = codeVerifier;
        RedirectUri = redirectUri;
        TokenUrl = tokenUrl;
        Scope = scope;
    }

    public string? ClientId { get; set; }
    public string? CodeVerifier { get; set; }
    public Uri? RedirectUri { get; set; }
    public Uri? TokenUrl { get; set; }
    public string? Scope { get; set; }
}