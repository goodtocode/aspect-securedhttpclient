using Goodtocode.SecuredHttpClient.Extensions;
using System.Net.Http;

namespace Goodtocode.SecuredHttpClient.Tests;

[TestClass]
public class HttpClientTokenExtensionsTests
{
    [TestMethod]
    public void AddBearerTokenSetsAuthorizationHeader()
    {
        // Arrange
        var client = new System.Net.Http.HttpClient();
        var token = "abc123";

        // Act
        client.AddBearerToken(token);

        // Assert
        Assert.AreEqual("Bearer", client.DefaultRequestHeaders.Authorization?.Scheme);
        Assert.AreEqual(token, client.DefaultRequestHeaders.Authorization?.Parameter);
    }

    [TestMethod]
    public void AddBearerTokenOverwritesExistingAuthorizationHeader()
    {
        // Arrange
        var client = new System.Net.Http.HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "old");

        // Act
        client.AddBearerToken("new-token");

        // Assert
        Assert.AreEqual("Bearer", client.DefaultRequestHeaders.Authorization?.Scheme);
        Assert.AreEqual("new-token", client.DefaultRequestHeaders.Authorization?.Parameter);
    }

    [TestMethod]
    public void AddBearerTokenAllowsEmptyToken()
    {
        // Arrange
        var client = new System.Net.Http.HttpClient();

        // Act
        client.AddBearerToken(string.Empty);

        // Assert
        Assert.AreEqual("Bearer", client.DefaultRequestHeaders.Authorization?.Scheme);
        Assert.AreEqual(string.Empty, client.DefaultRequestHeaders.Authorization?.Parameter);
    }
}