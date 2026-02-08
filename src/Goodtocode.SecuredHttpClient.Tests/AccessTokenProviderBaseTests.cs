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

    [TestMethod]
    public async Task GetAccessTokenAsyncRefreshesWhenExpiredWithinOneMinute()
    {
        // Arrange
        var provider = new ConfigurableTokenProvider
        {
            NextToken = "token-1",
            NextExpirationDateUtc = DateTime.UtcNow.AddSeconds(30)
        };

        // Act
        var token1 = await provider.GetAccessTokenAsync();
        provider.NextToken = "token-2";
        provider.NextExpirationDateUtc = DateTime.UtcNow.AddMinutes(5);
        var token2 = await provider.GetAccessTokenAsync();

        // Assert
        Assert.AreEqual("token-1", token1);
        Assert.AreEqual("token-2", token2);
        Assert.AreEqual(2, provider.CallCount);
    }

    [TestMethod]
    public async Task GetAccessTokenAsyncRefreshesWhenTokenIsWhitespace()
    {
        // Arrange
        var provider = new ConfigurableTokenProvider
        {
            NextToken = "   ",
            NextExpirationDateUtc = DateTime.UtcNow.AddMinutes(10)
        };

        // Act
        var token1 = await provider.GetAccessTokenAsync();
        provider.NextToken = "token-2";
        provider.NextExpirationDateUtc = DateTime.UtcNow.AddMinutes(10);
        var token2 = await provider.GetAccessTokenAsync();

        // Assert
        Assert.AreEqual("   ", token1);
        Assert.AreEqual("token-2", token2);
        Assert.AreEqual(2, provider.CallCount);
    }

    [TestMethod]
    public async Task GetAccessTokenAsyncDoesNotRefreshWhenMoreThanOneMinuteRemaining()
    {
        // Arrange
        var provider = new ConfigurableTokenProvider
        {
            NextToken = "token-1",
            NextExpirationDateUtc = DateTime.UtcNow.AddMinutes(2)
        };

        // Act
        var token1 = await provider.GetAccessTokenAsync();
        provider.NextToken = "token-2";
        provider.NextExpirationDateUtc = DateTime.UtcNow.AddMinutes(2);
        var token2 = await provider.GetAccessTokenAsync();

        // Assert
        Assert.AreEqual("token-1", token1);
        Assert.AreEqual("token-1", token2);
        Assert.AreEqual(1, provider.CallCount);
    }

    private class TestTokenProvider : AccessTokenProviderBase
    {
        private int _callCount;
        protected override Task<string> GetNewAccessToken()
        {
            _callCount++;
            Token = $"token-{_callCount}";
            ExpirationDateUtc = DateTime.UtcNow.AddMinutes(5);
            return Task.FromResult(Token);
        }
    }

    private class ConfigurableTokenProvider : AccessTokenProviderBase
    {
        public int CallCount { get; private set; }
        public string NextToken { get; set; } = string.Empty;
        public DateTime NextExpirationDateUtc { get; set; } = DateTime.UtcNow.AddMinutes(5);

        protected override Task<string> GetNewAccessToken()
        {
            CallCount++;
            Token = NextToken;
            ExpirationDateUtc = NextExpirationDateUtc;
            return Task.FromResult(Token);
        }
    }
}