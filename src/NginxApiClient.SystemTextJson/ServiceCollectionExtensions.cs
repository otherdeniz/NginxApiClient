using Microsoft.Extensions.DependencyInjection;
using NginxApiClient.Internal;

namespace NginxApiClient.SystemTextJson;

/// <summary>
/// Extension methods for registering NginxApiClient with System.Text.Json serialization
/// in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds NginxApiClient services to the DI container using System.Text.Json serialization.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure <see cref="NginxProxyManagerClientOptions"/>.</param>
    /// <returns>The service collection for chaining.</returns>
    /// <example>
    /// <code>
    /// services.AddNginxApiClient(options =>
    /// {
    ///     options.BaseUrl = "http://localhost:81";
    ///     options.Credentials = new NginxCredentials("admin@example.com", "changeme");
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddNginxApiClient(
        this IServiceCollection services,
        Action<NginxProxyManagerClientOptions> configure)
    {
        var options = new NginxProxyManagerClientOptions();
        configure(options);
        options.Validate();

        var serializer = new SystemTextJsonSerializer();

        services.AddSingleton(options);
        services.AddSingleton<IJsonSerializer>(serializer);

        services.AddHttpClient<INginxProxyManagerClient, NginxProxyManagerClient>(client =>
        {
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/'));
        })
        .AddHttpMessageHandler(() => new AuthenticationDelegatingHandler(options, serializer))
        .AddHttpMessageHandler(() => new ErrorHandlingDelegatingHandler(serializer));

        return services;
    }
}
