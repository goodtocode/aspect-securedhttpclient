using Microsoft.Extensions.Options;

namespace Goodtocode.SecuredHttpClient.Options;

public class AuthCodePkceOptionsValidation : IValidateOptions<AuthCodePkceOptions>
{
    public ValidateOptionsResult Validate(string? name, AuthCodePkceOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ClientId))
            return ValidateOptionsResult.Fail("ClientId is required.");
        if (string.IsNullOrWhiteSpace(options.CodeVerifier))
            return ValidateOptionsResult.Fail("CodeVerifier is required.");
        if (options.RedirectUri == null)
            return ValidateOptionsResult.Fail("RedirectUri is required.");
        if (options.TokenUrl == null)
            return ValidateOptionsResult.Fail("TokenUrl is required.");
        if (string.IsNullOrWhiteSpace(options.Scope))
            return ValidateOptionsResult.Fail("Scope is required.");

        return ValidateOptionsResult.Success;
    }
}