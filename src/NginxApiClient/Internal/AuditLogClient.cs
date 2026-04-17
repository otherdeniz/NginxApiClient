using NginxApiClient.Clients;
using NginxApiClient.Models.AuditLog;

namespace NginxApiClient.Internal;

internal sealed class AuditLogClient : IAuditLogClient
{
    private readonly HttpClient _httpClient;
    private readonly IJsonSerializer _serializer;
    private const string BasePath = "/api/audit-log";

    public AuditLogClient(HttpClient httpClient, IJsonSerializer serializer)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task<IReadOnlyList<AuditLogEntry>> ListAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BasePath, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<List<AuditLogEntry>>(json) ?? new List<AuditLogEntry>();
    }
}
