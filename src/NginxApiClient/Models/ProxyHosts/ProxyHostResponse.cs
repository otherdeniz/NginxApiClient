namespace NginxApiClient.Models.ProxyHosts;

/// <summary>
/// Response model for a proxy host from the NPM API.
/// </summary>
public class ProxyHostResponse
{
    /// <summary>The unique identifier of the proxy host.</summary>
    public int Id { get; set; }

    /// <summary>The domain names this proxy host serves (e.g., ["example.com", "www.example.com"]).</summary>
    public List<string> DomainNames { get; set; } = new();

    /// <summary>The scheme used to connect to the forward host ("http" or "https").</summary>
    public string ForwardScheme { get; set; } = "http";

    /// <summary>The hostname or IP address to forward requests to.</summary>
    public string ForwardHost { get; set; } = string.Empty;

    /// <summary>The port to forward requests to.</summary>
    public int ForwardPort { get; set; }

    /// <summary>Whether response caching is enabled.</summary>
    public bool CachingEnabled { get; set; }

    /// <summary>Whether WebSocket upgrade is allowed.</summary>
    public bool AllowWebsocketUpgrade { get; set; }

    /// <summary>Whether common exploit patterns are blocked.</summary>
    public bool BlockExploits { get; set; }

    /// <summary>The ID of the associated access list, or 0 if none.</summary>
    public int AccessListId { get; set; }

    /// <summary>The ID of the associated SSL certificate, or 0 if none.</summary>
    public int CertificateId { get; set; }

    /// <summary>Whether SSL is forced (HTTP redirected to HTTPS).</summary>
    public bool SslForced { get; set; }

    /// <summary>Whether HTTP Strict Transport Security (HSTS) is enabled.</summary>
    public bool HstsEnabled { get; set; }

    /// <summary>Whether HSTS applies to subdomains.</summary>
    public bool HstsSubdomains { get; set; }

    /// <summary>Whether HTTP/2 support is enabled.</summary>
    public bool Http2Support { get; set; }

    /// <summary>Whether this proxy host is currently enabled.</summary>
    public bool Enabled { get; set; }

    /// <summary>Location-based routing entries for path-based proxying.</summary>
    public List<ProxyHostLocation>? Locations { get; set; }

    /// <summary>Custom nginx directives injected into the generated configuration.</summary>
    public string? AdvancedConfig { get; set; }

    /// <summary>Additional metadata from the NPM API.</summary>
    public ProxyHostMeta? Meta { get; set; }

    /// <summary>Creation timestamp.</summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>Last modification timestamp.</summary>
    public DateTime ModifiedOn { get; set; }
}

/// <summary>
/// Metadata associated with a proxy host.
/// </summary>
public class ProxyHostMeta
{
    /// <summary>Number of times letsencrypt has been provisioned.</summary>
    public int? LetsencryptAgree { get; set; }

    /// <summary>DNS challenge flag.</summary>
    public bool? DnsChallenge { get; set; }

    /// <summary>Letsencrypt email address.</summary>
    public string? LetsencryptEmail { get; set; }
}
