using NginxApiClient.Clients;

namespace NginxApiClient.Internal;

/// <summary>
/// Internal root implementation of <see cref="INginxProxyManagerClient"/>.
/// Provides access to per-resource clients via lazy-initialized properties.
/// </summary>
internal sealed class NginxProxyManagerClient : INginxProxyManagerClient
{
    private readonly HttpClient _httpClient;
    private readonly IJsonSerializer _serializer;

    private ProxyHostClient? _proxyHosts;
    private CertificateClient? _certificates;

    public NginxProxyManagerClient(HttpClient httpClient, IJsonSerializer serializer)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    /// <inheritdoc />
    public IProxyHostClient ProxyHosts =>
        _proxyHosts ??= new ProxyHostClient(_httpClient, _serializer);

    /// <inheritdoc />
    public ICertificateClient Certificates =>
        _certificates ??= new CertificateClient(_httpClient, _serializer);

    // Phase 2 resource clients — stub implementations until Epic 6
    /// <inheritdoc />
    public IRedirectionHostClient RedirectionHosts =>
        throw new NotImplementedException("Redirection host management will be available in a future release.");

    /// <inheritdoc />
    public IDeadHostClient DeadHosts =>
        throw new NotImplementedException("Dead host management will be available in a future release.");

    /// <inheritdoc />
    public IStreamClient Streams =>
        throw new NotImplementedException("Stream management will be available in a future release.");

    /// <inheritdoc />
    public IAccessListClient AccessLists =>
        throw new NotImplementedException("Access list management will be available in a future release.");

    /// <inheritdoc />
    public IUserClient Users =>
        throw new NotImplementedException("User management will be available in a future release.");

    /// <inheritdoc />
    public ISettingsClient Settings =>
        throw new NotImplementedException("Settings management will be available in a future release.");

    /// <inheritdoc />
    public IAuditLogClient AuditLog =>
        throw new NotImplementedException("Audit log access will be available in a future release.");

    /// <inheritdoc />
    public IReportsClient Reports =>
        throw new NotImplementedException("Reports access will be available in a future release.");
}
