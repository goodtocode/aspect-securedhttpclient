using Goodtocode.SecuredHttpClient.Middleware;

namespace Goodtocode.SecuredHttpClient.Tests;

[TestClass]
public class TokenHandlerTests
{
    [TestMethod]
    public async Task SendAsyncAttachesBearerToken()
    {
        // Arrange
        var token = "test-token";
        var provider = new TestAccessTokenProvider(token);

        var testHandler = new TestHandler();
        var handler = new TokenHandler(provider)
        {
            InnerHandler = testHandler
        };
        var client = new HttpClient(handler);

        // Act
        await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://test"));

        // Assert
        Assert.AreEqual(token, testHandler.LastRequest?.Headers.Authorization?.Parameter);
        Assert.AreEqual("Bearer", testHandler.LastRequest?.Headers.Authorization?.Scheme);
    }

    [TestMethod]
    public async Task SendAsyncOverwritesExistingAuthorizationHeader()
    {
        // Arrange
        var token = "new-token";
        var provider = new TestAccessTokenProvider(token);
        var testHandler = new TestHandler();
        var handler = new TokenHandler(provider)
        {
            InnerHandler = testHandler
        };
        var client = new HttpClient(handler);
        var request = new HttpRequestMessage(HttpMethod.Get, "http://test")
        {
            Headers = { Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "old") }
        };

        // Act
        await client.SendAsync(request);

        // Assert
        Assert.AreEqual("Bearer", testHandler.LastRequest?.Headers.Authorization?.Scheme);
        Assert.AreEqual(token, testHandler.LastRequest?.Headers.Authorization?.Parameter);
    }

    [TestMethod]
    public async Task SendAsyncUsesProviderPerRequestAndAllowsEmptyToken()
    {
        // Arrange
        var provider = new CountingAccessTokenProvider();
        var testHandler = new TestHandler();
        var handler = new TokenHandler(provider)
        {
            InnerHandler = testHandler
        };
        var client = new HttpClient(handler);

        // Act
        provider.NextToken = string.Empty;
        await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://test"));
        provider.NextToken = "token-2";
        await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://test"));

        // Assert
        Assert.AreEqual(2, provider.CallCount);
        Assert.AreEqual("Bearer", testHandler.LastRequest?.Headers.Authorization?.Scheme);
        Assert.AreEqual("token-2", testHandler.LastRequest?.Headers.Authorization?.Parameter);
    }

    private class TestAccessTokenProvider(string token) : IAccessTokenProvider
    {
        private readonly string _token = token;

        public Task<string> GetAccessTokenAsync() => Task.FromResult(_token);
    }

    private class CountingAccessTokenProvider : IAccessTokenProvider
    {
        public int CallCount { get; private set; }
        public string NextToken { get; set; } = string.Empty;

        public Task<string> GetAccessTokenAsync()
        {
            CallCount++;
            return Task.FromResult(NextToken);
        }
    }

    private class TestHandler : DelegatingHandler
    {
        public HttpRequestMessage? LastRequest;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        }
    }
}