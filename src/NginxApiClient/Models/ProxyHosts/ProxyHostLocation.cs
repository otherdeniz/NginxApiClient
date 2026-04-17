namespace NginxApiClient.Models.ProxyHosts;

/// <summary>
/// A location-based routing entry within a proxy host, enabling path-based proxying
/// to different backend servers.
/// </summary>
public class ProxyHostLocation
{
    /// <summary>The URL path to match (e.g., "/api", "/static").</summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>The scheme used to connect to the forward host ("http" or "https").</summary>
    public string ForwardScheme { get; set; } = "http";

    /// <summary>The hostname or IP address to forward matching requests to.</summary>
    public string ForwardHost { get; set; } = string.Empty;

    /// <summary>The port to forward matching requests to.</summary>
    public int ForwardPort { get; set; }

    /// <summary>Custom nginx directives for this location block.</summary>
    public string? AdvancedConfig { get; set; }
}
