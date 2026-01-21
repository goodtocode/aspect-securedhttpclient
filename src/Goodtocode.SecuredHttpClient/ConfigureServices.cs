using Goodtocode.SecuredHttpClient.Middleware;
using Goodtocode.SecuredHttpClient.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Goodtocode.SecuredHttpClient;

public static class ConfigureServices
{
    public static IServiceCollection AddClientCredentialHttpClient(this IServiceCollection services,
        IConfiguration configuration,
        string clientName,
        Uri baseAddress)
    {
        services.AddSingleton<IValidateOptions<ClientCredentialOptions>, ClientCredentialOptionsValidation>();
        services.AddOptions<ClientCredentialOptions>()
        .Bind(configuration.GetSection(nameof(ClientCredentialOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddScoped<IAccessTokenProvider, ClientCredentialTokenProvider>();
        services.AddTransient<TokenHandler>();

        services.AddHttpClient(clientName, options =>
        {
            options.DefaultRequestHeaders.Clear();
            options.BaseAddress = baseAddress;
        })
            .AddHttpMessageHandler<TokenHandler>();

        return services;
    }

    public static IServiceCollection AddAuthCodePkceHttpClient(this IServiceCollection services,
    IConfiguration configuration,
    string clientName,
    Uri baseAddress,
    int maxRetry = 5)
    {
        services.AddSingleton<IValidateOptions<AuthCodePkceOptions>, AuthCodePkceOptionsValidation>();
        services.AddOptions<AuthCodePkceOptions>()
        .Bind(configuration.GetSection(nameof(AuthCodePkceOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddScoped<IAccessTokenProvider, AuthCodePkceTokenProvider>();
        services.AddTransient<TokenHandler>();

        services.AddHttpClient(clientName, options =>
        {
            options.DefaultRequestHeaders.Clear();
            options.BaseAddress = baseAddress;
        })
            .AddHttpMessageHandler<TokenHandler>();
            //.AddStandardResilienceHandler(options =>
            //{
            //    options.Retry.UseJitter = true;
            //    options.Retry.MaxRetryAttempts = maxRetry;
            //});

        return services;
    }
}