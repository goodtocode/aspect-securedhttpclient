using Goodtocode.SecuredHttpClient.Middleware;
using Goodtocode.SecuredHttpClient.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Goodtocode.SecuredHttpClient.Tests;

[TestClass]
public class ConfigureServicesTests
{
    [TestMethod]
    public void AddClientCredentialHttpClientRegistersServicesAndConfiguresHttpClient()
    {
        var services = new ServiceCollection();
        var settings = new Dictionary<string, string?>
        {
            ["ClientCredentialOptions:ClientId"] = "client-id",
            ["ClientCredentialOptions:ClientSecret"] = "client-secret",
            ["ClientCredentialOptions:TokenUrl"] = "http://localhost/token",
            ["ClientCredentialOptions:Scope"] = "scope"
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
        var baseAddress = new Uri("http://localhost/api/");

        services.AddClientCredentialHttpClient(configuration, "secured", baseAddress);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<ClientCredentialOptions>>().Value;

        Assert.AreEqual("client-id", options.ClientId);
        Assert.AreEqual("client-secret", options.ClientSecret);
        Assert.AreEqual("http://localhost/token", options.TokenUrl);
        Assert.AreEqual("scope", options.Scope);

        using var scope = provider.CreateScope();
        var tokenProvider = scope.ServiceProvider.GetRequiredService<IAccessTokenProvider>();
        Assert.IsInstanceOfType(tokenProvider, typeof(ClientCredentialTokenProvider));
        Assert.IsNotNull(provider.GetRequiredService<IValidateOptions<ClientCredentialOptions>>());
        Assert.IsNotNull(provider.GetRequiredService<TokenHandler>());

        var clientFactory = provider.GetRequiredService<IHttpClientFactory>();
        var client = clientFactory.CreateClient("secured");
        Assert.AreEqual(baseAddress, client.BaseAddress);
    }
}
