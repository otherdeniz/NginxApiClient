using NginxApiClient.Internal;

namespace NginxApiClient;

/// <summary>
/// Factory for creating <see cref="INginxProxyManagerClient"/> instances without dependency injection.
/// Use this for console apps, scripts, and non-ASP.NET scenarios.
/// </summary>
public static class NginxProxyManagerClientFactory
{
    /// <summary>
    /// Creates a fully configured <see cref="INginxProxyManagerClient"/> with authentication
    /// and error handling pipeline.
    /// </summary>
    /// <param name="options">The NPM connection options.</param>
    /// <param name="serializer">The JSON serializer to use.</param>
    /// <returns>A configured client ready to make API calls.</returns>
    /// <example>
    /// <code>
    /// var client = NginxProxyManagerClientFactory.Create(
    ///     new NginxProxyManagerClientOptions
    ///     {
    ///         BaseUrl = "http://localhost:81",
    ///         Credentials = new NginxCredentials("admin@example.com", "changeme"),
    ///     },
    ///     new SystemTextJsonSerializer());
    /// </code>
    /// </example>
    public static INginxProxyManagerClient Create(NginxProxyManagerClientOptions options, IJsonSerializer serializer)
    {
        if (options is null) throw new ArgumentNullException(nameof(options));
        if (serializer is null) throw new ArgumentNullException(nameof(serializer));

        options.Validate();

        var errorHandler = new ErrorHandlingDelegatingHandler(serializer, new HttpClientHandler());
        var authHandler = new AuthenticationDelegatingHandler(options, serializer, errorHandler);

        var httpClient = new HttpClient(authHandler)
        {
            BaseAddress = new Uri(options.BaseUrl.TrimEnd('/')),
        };

        return new NginxProxyManagerClient(httpClient, serializer);
    }
}
