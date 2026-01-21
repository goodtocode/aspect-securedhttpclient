namespace Goodtocode.SecuredHttpClient.Extensions;

public static class HttpClientTokenExtensions
{
    public static void AddBearerToken(
        this HttpClient httpClient, string token)
    {
        httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}