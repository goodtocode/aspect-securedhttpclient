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

    public required string ClientId { get; set; }
    public required string CodeVerifier { get; set; }
    public required Uri RedirectUri { get; set; }
    public required Uri TokenUrl { get; set; }
    public required string Scope { get; set; }
}