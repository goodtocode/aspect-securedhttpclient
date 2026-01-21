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
}