namespace NginxApiClient.Models.ProxyHosts;

/// <summary>
/// Request model for updating an existing proxy host in NGINX Proxy Manager.
/// All properties are optional — only set the ones you want to change.
/// </summary>
public class UpdateProxyHostRequest
{
    /// <summary>The domain names this proxy host will serve.</summary>
    public List<string>? DomainNames { get; set; }

    /// <summary>The scheme used to connect to the forward host ("http" or "https").</summary>
    public string? ForwardScheme { get; set; }

    /// <summary>The hostname or IP address to forward requests to.</summary>
    public string? ForwardHost { get; set; }

    /// <summary>The port to forward requests to.</summary>
    public int? ForwardPort { get; set; }

    /// <summary>Whether to enable response caching.</summary>
    public bool? CachingEnabled { get; set; }

    /// <summary>Whether to allow WebSocket upgrade.</summary>
    public bool? AllowWebsocketUpgrade { get; set; }

    /// <summary>Whether to block common exploit patterns.</summary>
    public bool? BlockExploits { get; set; }

    /// <summary>The ID of an access list to associate, or 0 for none.</summary>
    public int? AccessListId { get; set; }

    /// <summary>The ID of an SSL certificate to associate, or 0 for none.</summary>
    public int? CertificateId { get; set; }

    /// <summary>Whether to force SSL (redirect HTTP to HTTPS).</summary>
    public bool? SslForced { get; set; }

    /// <summary>Whether to enable HSTS.</summary>
    public bool? HstsEnabled { get; set; }

    /// <summary>Whether HSTS applies to subdomains.</summary>
    public bool? HstsSubdomains { get; set; }

    /// <summary>Whether to enable HTTP/2 support.</summary>
    public bool? Http2Support { get; set; }

    /// <summary>Location-based routing entries.</summary>
    public List<ProxyHostLocation>? Locations { get; set; }

    /// <summary>Custom nginx directives.</summary>
    public string? AdvancedConfig { get; set; }

    /// <summary>Additional metadata.</summary>
    public ProxyHostMeta? Meta { get; set; }
}
