using Microsoft.Extensions.Options;

namespace Goodtocode.SecuredHttpClient.Options;

public class ClientCredentialOptionsValidation : IValidateOptions<ClientCredentialOptions>
{
    public ValidateOptionsResult Validate(string? name, ClientCredentialOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ClientId))
            return ValidateOptionsResult.Fail("ClientId is required.");
        if (string.IsNullOrWhiteSpace(options.ClientSecret))
            return ValidateOptionsResult.Fail("ClientSecret is required.");
        if (string.IsNullOrWhiteSpace(options.TokenUrl))
            return ValidateOptionsResult.Fail("TokenUrl is required.");
        if (string.IsNullOrWhiteSpace(options.Scope))
            return ValidateOptionsResult.Fail("Scope is required.");

        return ValidateOptionsResult.Success;
    }
}
