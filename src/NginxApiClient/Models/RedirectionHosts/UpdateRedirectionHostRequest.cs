namespace NginxApiClient.Models.RedirectionHosts;

/// <summary>
/// Request model for updating a redirection host.
/// </summary>
public class UpdateRedirectionHostRequest
{
    /// <summary>The domain names to handle.</summary>
    public List<string>? DomainNames { get; set; }

    /// <summary>The scheme to redirect to.</summary>
    public string? ForwardScheme { get; set; }

    /// <summary>The domain name to redirect to.</summary>
    public string? ForwardDomainName { get; set; }

    /// <summary>Whether to preserve the original URL path.</summary>
    public bool? PreservePath { get; set; }

    /// <summary>The HTTP redirect status code.</summary>
    public int? ForwardHttpCode { get; set; }

    /// <summary>Whether to block common exploit patterns.</summary>
    public bool? BlockExploits { get; set; }

    /// <summary>The ID of an SSL certificate.</summary>
    public int? CertificateId { get; set; }

    /// <summary>Whether to force SSL.</summary>
    public bool? SslForced { get; set; }

    /// <summary>Whether to enable HSTS.</summary>
    public bool? HstsEnabled { get; set; }

    /// <summary>Whether HSTS applies to subdomains.</summary>
    public bool? HstsSubdomains { get; set; }

    /// <summary>Whether to enable HTTP/2.</summary>
    public bool? Http2Support { get; set; }

    /// <summary>Custom nginx directives.</summary>
    public string? AdvancedConfig { get; set; }
}
