using NginxApiClient.Models.ProxyHosts;

namespace NginxApiClient.Clients;

/// <summary>
/// Client for managing NGINX Proxy Manager proxy hosts.
/// Provides full CRUD operations plus enable/disable functionality.
/// </summary>
public interface IProxyHostClient
{
    /// <summary>Lists all proxy hosts.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All proxy hosts configured in NPM.</returns>
    /// <exception cref="Exceptions.NginxApiException">Thrown when the API returns an error.</exception>
    Task<IReadOnlyList<ProxyHostResponse>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a proxy host by ID.</summary>
    /// <param name="id">The proxy host ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The proxy host.</returns>
    /// <exception cref="Exceptions.NginxNotFoundException">Thrown when the proxy host is not found.</exception>
    Task<ProxyHostResponse> GetAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Creates a new proxy host.</summary>
    /// <param name="request">The proxy host configuration.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created proxy host with its assigned ID.</returns>
    /// <exception cref="Exceptions.NginxApiException">Thrown when the request is invalid.</exception>
    Task<ProxyHostResponse> CreateAsync(CreateProxyHostRequest request, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing proxy host.</summary>
    /// <param name="id">The proxy host ID.</param>
    /// <param name="request">The updated configuration.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated proxy host.</returns>
    /// <exception cref="Exceptions.NginxNotFoundException">Thrown when the proxy host is not found.</exception>
    Task<ProxyHostResponse> UpdateAsync(int id, UpdateProxyHostRequest request, CancellationToken cancellationToken = default);

    /// <summary>Deletes a proxy host.</summary>
    /// <param name="id">The proxy host ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="Exceptions.NginxNotFoundException">Thrown when the proxy host is not found.</exception>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Enables a proxy host.</summary>
    /// <param name="id">The proxy host ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="Exceptions.NginxNotFoundException">Thrown when the proxy host is not found.</exception>
    Task EnableAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Disables a proxy host.</summary>
    /// <param name="id">The proxy host ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="Exceptions.NginxNotFoundException">Thrown when the proxy host is not found.</exception>
    Task DisableAsync(int id, CancellationToken cancellationToken = default);
}
