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
    private RedirectionHostClient? _redirectionHosts;
    private DeadHostClient? _deadHosts;
    private StreamClient? _streams;
    private AccessListClient? _accessLists;
    private UserClient? _users;
    private SettingsClient? _settings;
    private AuditLogClient? _auditLog;
    private ReportsClient? _reports;

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

    /// <inheritdoc />
    public IRedirectionHostClient RedirectionHosts =>
        _redirectionHosts ??= new RedirectionHostClient(_httpClient, _serializer);

    /// <inheritdoc />
    public IDeadHostClient DeadHosts =>
        _deadHosts ??= new DeadHostClient(_httpClient, _serializer);

    /// <inheritdoc />
    public IStreamClient Streams =>
        _streams ??= new StreamClient(_httpClient, _serializer);

    /// <inheritdoc />
    public IAccessListClient AccessLists =>
        _accessLists ??= new AccessListClient(_httpClient, _serializer);

    /// <inheritdoc />
    public IUserClient Users =>
        _users ??= new UserClient(_httpClient, _serializer);

    /// <inheritdoc />
    public ISettingsClient Settings =>
        _settings ??= new SettingsClient(_httpClient, _serializer);

    /// <inheritdoc />
    public IAuditLogClient AuditLog =>
        _auditLog ??= new AuditLogClient(_httpClient, _serializer);

    /// <inheritdoc />
    public IReportsClient Reports =>
        _reports ??= new ReportsClient(_httpClient, _serializer);
}
