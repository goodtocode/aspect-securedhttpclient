using Goodtocode.SecuredHttpClient.Middleware;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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

        var handler = new TokenHandler(provider)
        {
            InnerHandler = new TestHandler()
        };
        var client = new HttpClient(handler);

        // Act
        var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://test"));

        // Assert
        Assert.AreEqual(token, TestHandler.LastRequest?.Headers.Authorization?.Parameter);
        Assert.AreEqual("Bearer", TestHandler.LastRequest?.Headers.Authorization?.Scheme);
    }

    private class TestAccessTokenProvider : IAccessTokenProvider
    {
        private readonly string _token;
        public TestAccessTokenProvider(string token) => _token = token;
        public Task<string> GetAccessTokenAsync() => Task.FromResult(_token);
    }

    private class TestHandler : DelegatingHandler
    {
        public static HttpRequestMessage? LastRequest;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        }
    }
}