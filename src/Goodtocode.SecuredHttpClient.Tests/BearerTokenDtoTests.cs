using Goodtocode.SecuredHttpClient.Middleware;
using System.Text.Json;

namespace Goodtocode.SecuredHttpClient.Tests;

[TestClass]
public class BearerTokenDtoTests
{
    [TestMethod]
    public void DeserializeMapsExpectedJsonProperties()
    {
        var json = "{\"token_type\":\"Bearer\",\"expires_in\":3600,\"ext_expires_in\":7200,\"access_token\":\"token\"}";

        var dto = JsonSerializer.Deserialize<BearerTokenDto>(json);

        Assert.IsNotNull(dto);
        Assert.AreEqual("Bearer", dto.TokenType);
        Assert.AreEqual(3600, dto.ExpiresIn);
        Assert.AreEqual(7200, dto.ExtExpiresIn);
        Assert.AreEqual("token", dto.AccessToken);
    }

    [TestMethod]
    public void SerializeUsesExpectedJsonPropertyNames()
    {
        var dto = new BearerTokenDto
        {
            TokenType = "Bearer",
            ExpiresIn = 1200,
            ExtExpiresIn = 2400,
            AccessToken = "token"
        };

        var json = JsonSerializer.Serialize(dto);
        using var document = JsonDocument.Parse(json);

        Assert.AreEqual("Bearer", document.RootElement.GetProperty("token_type").GetString());
        Assert.AreEqual(1200, document.RootElement.GetProperty("expires_in").GetInt32());
        Assert.AreEqual(2400, document.RootElement.GetProperty("ext_expires_in").GetInt32());
        Assert.AreEqual("token", document.RootElement.GetProperty("access_token").GetString());
    }
}
