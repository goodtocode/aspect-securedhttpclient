using Goodtocode.SecuredHttpClient.Middleware;

namespace Goodtocode.SecuredHttpClient.Tests;

[TestClass]
public class AccessTokenProviderBaseTests
{
    [TestMethod]
    public async Task GetAccessTokenAsyncCachesTokenUntilExpired()
    {
        // Arrange
        var provider = new TestTokenProvider();

        // Act
        var token1 = await provider.GetAccessTokenAsync();
        var token2 = await provider.GetAccessTokenAsync();

        // Assert
        Assert.AreEqual("token-1", token1);
        Assert.AreEqual("token-1", token2); // Should be cached
    }

    private class TestTokenProvider : AccessTokenProviderBase
    {
        private int _callCount = 0;
        protected override Task<string> GetNewAccessToken()
        {
            _callCount++;
            Token = $"token-{_callCount}";
            ExpirationDateUtc = DateTime.UtcNow.AddMinutes(5);
            return Task.FromResult(Token);
        }
    }
}