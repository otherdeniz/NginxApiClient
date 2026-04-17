namespace NginxApiClient.Models.DeadHosts;

/// <summary>Response model for a dead host (custom 404 page) from the NPM API.</summary>
public class DeadHostResponse
{
    /// <summary>The unique identifier.</summary>
    public int Id { get; set; }
    /// <summary>The domain names this dead host handles.</summary>
    public List<string> DomainNames { get; set; } = new();
    /// <summary>Whether this dead host is enabled.</summary>
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

/// <summary>Request model for creating a dead host.</summary>
public class CreateDeadHostRequest
{
    /// <summary>The domain names to handle. Required.</summary>
    public List<string> DomainNames { get; set; } = new();
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

/// <summary>Request model for updating a dead host.</summary>
public class UpdateDeadHostRequest
{
    /// <summary>The domain names to handle.</summary>
    public List<string>? DomainNames { get; set; }
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
