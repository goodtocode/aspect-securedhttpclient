# Goodtocode.SecuredHttpClient

[![NuGet CI/CD](https://github.com/goodtocode/aspect-securedhttpclient/actions/workflows/gtc-securedhttpclient-nuget.yml/badge.svg)](https://github.com/goodtocode/aspect-securedhttpclient/actions/workflows/gtc-securedhttpclient-nuget.yml)

A secure, resilient HTTP client registration and access token management library for .NET and Blazor. Easily add OAuth2-protected HttpClients to your application with support for both Client Credentials and Authorization Code PKCE flows.

## Features
- Register HttpClients that automatically acquire and attach OAuth2 access tokens
- Supports Client Credentials and Authorization Code PKCE flows
- Built-in token caching and refresh
- Pluggable token providers via `IAccessTokenProvider`
- Resilience (retry with jitter) for HTTP requests
- Extension methods for adding bearer tokens to requests
- Simple integration with Blazor, ASP.NET Core, and .NET DI

## Installation
Install via NuGet:

```
dotnet add package Goodtocode.SecuredHttpClient
```

## Quick Start

### 1. Register a Secured HttpClient (Blazor/ASP.NET Core)

#### Client Credentials Flow
```csharp
services.AddClientCredentialHttpClient(
    configuration, // IConfiguration
    clientName: "MyApiClient",
    baseAddress: new Uri("https://api.example.com"),
    maxRetry: 5 // optional
);
```

#### Authorization Code PKCE Flow
```csharp
services.AddAuthCodePkceHttpClient(
    configuration, // IConfiguration
    clientName: "MyApiClient",
    baseAddress: new Uri("https://api.example.com"),
    maxRetry: 5 // optional
);
```

#### Custom Registration for Blazor RCL
```csharp
public static IServiceCollection AddAccessTokenHttpClient(
    this IServiceCollection services,
    Action<ResilientHttpClientOptions> configureOptions)
{
    var options = new ResilientHttpClientOptions();
    configureOptions(options);

    if (options.BaseAddress == null)
        throw new ArgumentNullException(nameof(configureOptions), "BaseAddress must be provided.");
    if (string.IsNullOrWhiteSpace(options.ClientName))
        throw new ArgumentNullException(nameof(configureOptions), "ClientName must be provided.");

    services.AddOptions<AuthCodePkceOptions>()
        .ValidateDataAnnotations()
        .ValidateOnStart();
    services.AddScoped<IAccessTokenProvider, DownstreamApiAccessTokenProvider>();
    services.AddScoped<TokenHandler>();

    services.AddHttpClient(options.ClientName, clientOptions =>
    {
        clientOptions.DefaultRequestHeaders.Clear();
        clientOptions.BaseAddress = options.BaseAddress;
    })
    .AddHttpMessageHandler<TokenHandler>()
    .AddStandardResilienceHandler(resilienceOptions =>
    {
        resilienceOptions.Retry.UseJitter = true;
        resilienceOptions.Retry.MaxRetryAttempts = options.MaxRetry;
    });

    return services;
}
```

### 2. Add Bearer Token to HttpClient (Manual)
```csharp
using Goodtocode.SecuredHttpClient.Extensions;

httpClient.AddBearerToken("your-access-token");
```

## Options
- `ClientCredentialOptions`: For client credentials flow (ClientId, ClientSecret, TokenUrl, Scope)
- `AuthCodePkceOptions`: For PKCE flow (ClientId, CodeVerifier, RedirectUri, TokenUrl, Scope)
- `ResilientHttpClientOptions`: For base address, client name, and retry settings

## How It Works
- Token providers implement `IAccessTokenProvider` and handle token acquisition and caching
- `TokenHandler` automatically attaches the access token to outgoing requests
- Resilience is provided via retry policies with jitter

## License
MIT

## Contact
- [GitHub Repo](https://github.com/goodtocode/aspect-securedhttpclient)
- [@goodtocode](https://twitter.com/goodtocode)

## Version History

| Version | Date       | Changes                       |
|---------|------------|-------------------------------|
| 1.1.0   | 2026-01-22 | Bump from .NET 9 to .NET 10   |