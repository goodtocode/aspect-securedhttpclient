using Goodtocode.SecuredHttpClient.Options;

namespace Goodtocode.SecuredHttpClient.Tests;

[TestClass]
public class ClientCredentialOptionsTests
{
    [TestMethod]
    public void ConstructorAssignsValues()
    {
        var options = new ClientCredentialOptions("client-id", "client-secret", "http://localhost/token", "scope");

        Assert.AreEqual("client-id", options.ClientId);
        Assert.AreEqual("client-secret", options.ClientSecret);
        Assert.AreEqual("http://localhost/token", options.TokenUrl);
        Assert.AreEqual("scope", options.Scope);
    }
}
