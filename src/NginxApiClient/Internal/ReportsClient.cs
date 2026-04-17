using NginxApiClient.Clients;
using NginxApiClient.Models.Reports;

namespace NginxApiClient.Internal;

internal sealed class ReportsClient : IReportsClient
{
    private readonly HttpClient _httpClient;
    private readonly IJsonSerializer _serializer;
    private const string BasePath = "/api/reports/hosts";

    public ReportsClient(HttpClient httpClient, IJsonSerializer serializer)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task<HostReportResponse> GetHostsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BasePath, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<HostReportResponse>(json);
    }
}
