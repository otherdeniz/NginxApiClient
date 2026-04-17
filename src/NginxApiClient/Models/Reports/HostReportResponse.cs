namespace NginxApiClient.Models.Reports;

/// <summary>Response model for host report statistics from the NPM API.</summary>
public class HostReportResponse
{
    /// <summary>Number of proxy hosts.</summary>
    public int Proxy { get; set; }
    /// <summary>Number of redirection hosts.</summary>
    public int Redirection { get; set; }
    /// <summary>Number of stream hosts.</summary>
    public int Stream { get; set; }
    /// <summary>Number of dead hosts.</summary>
    public int Dead { get; set; }
}
