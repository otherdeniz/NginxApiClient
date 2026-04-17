namespace NginxApiClient.Models.RedirectionHosts;

/// <summary>
/// Request model for creating a redirection host.
/// </summary>
public class CreateRedirectionHostRequest
{
    /// <summary>The domain names to handle. Required.</summary>
    public List<string> DomainNames { get; set; } = new();

    /// <summary>The scheme to redirect to ("http" or "https").</summary>
    public string ForwardScheme { get; set; } = "http";

    /// <summary>The domain name to redirect to. Required.</summary>
    public string ForwardDomainName { get; set; } = string.Empty;

    /// <summary>Whether to preserve the original URL path. Default: false.</summary>
    public bool PreservePath { get; set; }

    /// <summary>The HTTP redirect status code (301, 302, etc.). Default: 301.</summary>
    public int ForwardHttpCode { get; set; } = 301;

    /// <summary>Whether to block common exploit patterns.</summary>
    public bool BlockExploits { get; set; }

    /// <summary>The ID of an SSL certificate, or 0 for none.</summary>
    public int CertificateId { get; set; }

    /// <summary>Whether to force SSL.</summary>
    public bool SslForced { get; set; }

    /// <summary>Whether to enable HSTS.</summary>
    public bool HstsEnabled { get; set; }

    /// <summary>Whether HSTS applies to subdomains.</summary>
    public bool HstsSubdomains { get; set; }

    /// <summary>Whether to enable HTTP/2.</summary>
    public bool Http2Support { get; set; }

    /// <summary>Custom nginx directives.</summary>
    public string? AdvancedConfig { get; set; }
}
