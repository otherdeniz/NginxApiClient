namespace NginxApiClient.Models.RedirectionHosts;

/// <summary>
/// Response model for a redirection host from the NPM API.
/// </summary>
public class RedirectionHostResponse
{
    /// <summary>The unique identifier.</summary>
    public int Id { get; set; }

    /// <summary>The domain names this redirection host handles.</summary>
    public List<string> DomainNames { get; set; } = new();

    /// <summary>The scheme to redirect to ("http" or "https").</summary>
    public string ForwardScheme { get; set; } = "http";

    /// <summary>The domain name to redirect to.</summary>
    public string ForwardDomainName { get; set; } = string.Empty;

    /// <summary>Whether to preserve the original URL path in the redirect.</summary>
    public bool PreservePath { get; set; }

    /// <summary>The HTTP status code for the redirect (301, 302, etc.).</summary>
    public int ForwardHttpCode { get; set; } = 301;

    /// <summary>Whether this redirection host is enabled.</summary>
    public bool Enabled { get; set; }

    /// <summary>Whether to block common exploit patterns.</summary>
    public bool BlockExploits { get; set; }

    /// <summary>The ID of the associated SSL certificate, or 0 if none.</summary>
    public int CertificateId { get; set; }

    /// <summary>Whether SSL is forced.</summary>
    public bool SslForced { get; set; }

    /// <summary>Whether HSTS is enabled.</summary>
    public bool HstsEnabled { get; set; }

    /// <summary>Whether HSTS applies to subdomains.</summary>
    public bool HstsSubdomains { get; set; }

    /// <summary>Whether HTTP/2 support is enabled.</summary>
    public bool Http2Support { get; set; }

    /// <summary>Custom nginx directives.</summary>
    public string? AdvancedConfig { get; set; }

    /// <summary>Creation timestamp.</summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>Last modification timestamp.</summary>
    public DateTime ModifiedOn { get; set; }
}
