using NginxApiClient.Models.Reports;

namespace NginxApiClient.Clients;

/// <summary>Client for accessing NPM host reports and statistics.</summary>
public interface IReportsClient
{
    /// <summary>Gets host count statistics.</summary>
    Task<HostReportResponse> GetHostsAsync(CancellationToken cancellationToken = default);
}
