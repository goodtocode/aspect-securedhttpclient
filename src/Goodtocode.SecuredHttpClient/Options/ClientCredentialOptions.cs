namespace Goodtocode.SecuredHttpClient.Options;

public class ClientCredentialOptions
{
    public ClientCredentialOptions() { }

    public ClientCredentialOptions(string clientId, string clientSecret, string tokenUrl, string scope)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        TokenUrl = tokenUrl;
        Scope = scope;
    }

    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string TokenUrl { get; set; }
    public required string Scope { get; set; }
}
