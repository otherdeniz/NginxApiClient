namespace NginxApiClient.Models.ProxyHosts;

/// <summary>
/// Request model for creating a new proxy host in NGINX Proxy Manager.
/// </summary>
public class CreateProxyHostRequest
{
    /// <summary>The domain names this proxy host will serve. Required.</summary>
    public List<string> DomainNames { get; set; } = new();

    /// <summary>The scheme used to connect to the forward host ("http" or "https"). Default: "http".</summary>
    public string ForwardScheme { get; set; } = "http";

    /// <summary>The hostname or IP address to forward requests to. Required.</summary>
    public string ForwardHost { get; set; } = string.Empty;

    /// <summary>The port to forward requests to. Required.</summary>
    public int ForwardPort { get; set; }

    /// <summary>Whether to enable response caching. Default: false.</summary>
    public bool CachingEnabled { get; set; }

    /// <summary>Whether to allow WebSocket upgrade. Default: false.</summary>
    public bool AllowWebsocketUpgrade { get; set; }

    /// <summary>Whether to block common exploit patterns. Default: false.</summary>
    public bool BlockExploits { get; set; }

    /// <summary>The ID of an access list to associate, or 0 for none.</summary>
    public int AccessListId { get; set; }

    /// <summary>The ID of an SSL certificate to associate, or 0 for none.</summary>
    public int CertificateId { get; set; }

    /// <summary>Whether to force SSL (redirect HTTP to HTTPS). Default: false.</summary>
    public bool SslForced { get; set; }

    /// <summary>Whether to enable HSTS. Default: false.</summary>
    public bool HstsEnabled { get; set; }

    /// <summary>Whether HSTS applies to subdomains. Default: false.</summary>
    public bool HstsSubdomains { get; set; }

    /// <summary>Whether to enable HTTP/2 support. Default: false.</summary>
    public bool Http2Support { get; set; }

    /// <summary>Location-based routing entries for path-based proxying.</summary>
    public List<ProxyHostLocation>? Locations { get; set; }

    /// <summary>Custom nginx directives to inject into the generated configuration.</summary>
    public string? AdvancedConfig { get; set; }

    /// <summary>Additional metadata (e.g., Let's Encrypt settings).</summary>
    public ProxyHostMeta? Meta { get; set; }
}
