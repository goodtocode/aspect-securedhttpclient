using Goodtocode.SecuredHttpClient.Options;

namespace Goodtocode.SecuredHttpClient.Tests;

[TestClass]
public class ClientCredentialOptionsValidationTests
{
    [TestMethod]
    public void ValidateReturnsFailureWhenClientIdMissing()
    {
        var validator = new ClientCredentialOptionsValidation();
        var options = new ClientCredentialOptions
        {
            ClientSecret = "secret",
            TokenUrl = "http://localhost/token",
            Scope = "scope"
        };

        var result = validator.Validate(null, options);

        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual("ClientId is required.", result.FailureMessage);
    }

    [TestMethod]
    public void ValidateReturnsFailureWhenClientSecretMissing()
    {
        var validator = new ClientCredentialOptionsValidation();
        var options = new ClientCredentialOptions
        {
            ClientId = "id",
            TokenUrl = "http://localhost/token",
            Scope = "scope"
        };

        var result = validator.Validate(null, options);

        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual("ClientSecret is required.", result.FailureMessage);
    }

    [TestMethod]
    public void ValidateReturnsFailureWhenTokenUrlMissing()
    {
        var validator = new ClientCredentialOptionsValidation();
        var options = new ClientCredentialOptions
        {
            ClientId = "id",
            ClientSecret = "secret",
            Scope = "scope"
        };

        var result = validator.Validate(null, options);

        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual("TokenUrl is required.", result.FailureMessage);
    }

    [TestMethod]
    public void ValidateReturnsFailureWhenScopeMissing()
    {
        var validator = new ClientCredentialOptionsValidation();
        var options = new ClientCredentialOptions
        {
            ClientId = "id",
            ClientSecret = "secret",
            TokenUrl = "http://localhost/token"
        };

        var result = validator.Validate(null, options);

        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual("Scope is required.", result.FailureMessage);
    }

    [TestMethod]
    public void ValidateReturnsSuccessWhenAllValuesProvided()
    {
        var validator = new ClientCredentialOptionsValidation();
        var options = new ClientCredentialOptions
        {
            ClientId = "id",
            ClientSecret = "secret",
            TokenUrl = "http://localhost/token",
            Scope = "scope"
        };

        var result = validator.Validate(null, options);

        Assert.IsTrue(result.Succeeded);
    }
}
