using Goodtocode.SecuredHttpClient.Middleware;
using Goodtocode.SecuredHttpClient.Options;
using OptionsFactory = Microsoft.Extensions.Options.Options;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Goodtocode.SecuredHttpClient.Tests;

[TestClass]
public class ClientCredentialTokenProviderTests
{
    [TestMethod]
    public async Task GetAccessTokenAsyncReturnsTokenWhenRequestSucceeds()
    {
        var responseJson = "{\"token_type\":\"Bearer\",\"expires_in\":1200,\"ext_expires_in\":1200,\"access_token\":\"test-token\"}";
        var (tokenUrl, serverTask) = StartTokenServer(HttpStatusCode.OK, responseJson);
        var options = OptionsFactory.Create(new ClientCredentialOptions("client-id", "client-secret", tokenUrl, "scope"));
        var provider = new ClientCredentialTokenProvider(options);

        var token = await provider.GetAccessTokenAsync();
        await serverTask;

        Assert.AreEqual("test-token", token);
    }

    [TestMethod]
    public async Task GetAccessTokenAsyncReturnsEmptyStringWhenRequestFails()
    {
        var (tokenUrl, serverTask) = StartTokenServer(HttpStatusCode.InternalServerError, null);
        var options = OptionsFactory.Create(new ClientCredentialOptions("client-id", "client-secret", tokenUrl, "scope"));
        var provider = new ClientCredentialTokenProvider(options);

        var token = await provider.GetAccessTokenAsync();
        await serverTask;

        Assert.AreEqual(string.Empty, token);
    }

    private static (string TokenUrl, Task ServerTask) StartTokenServer(HttpStatusCode statusCode, string? responseJson)
    {
        var port = GetFreePort();
        var listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{port}/");
        listener.Start();

        var serverTask = Task.Run(async () =>
        {
            try
            {
                var context = await listener.GetContextAsync();
                context.Response.StatusCode = (int)statusCode;
                if (!string.IsNullOrEmpty(responseJson))
                {
                    var buffer = Encoding.UTF8.GetBytes(responseJson);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = buffer.Length;
                    await context.Response.OutputStream.WriteAsync(buffer);
                }

                context.Response.OutputStream.Close();
            }
            finally
            {
                listener.Stop();
                listener.Close();
            }
        });

        return ($"http://localhost:{port}/", serverTask);
    }

    private static int GetFreePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}
