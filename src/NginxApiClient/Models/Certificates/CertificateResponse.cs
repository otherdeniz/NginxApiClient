namespace NginxApiClient.Models.Certificates;

/// <summary>
/// Response model for a certificate from the NPM API.
/// </summary>
public class CertificateResponse
{
    /// <summary>The unique identifier of the certificate.</summary>
    public int Id { get; set; }

    /// <summary>The certificate provider (e.g., "letsencrypt", "other").</summary>
    public string Provider { get; set; } = string.Empty;

    /// <summary>A friendly name for the certificate.</summary>
    public string NiceName { get; set; } = string.Empty;

    /// <summary>The domain names covered by this certificate.</summary>
    public List<string> DomainNames { get; set; } = new();

    /// <summary>The expiration date of the certificate.</summary>
    public DateTime ExpiresOn { get; set; }

    /// <summary>Creation timestamp.</summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>Last modification timestamp.</summary>
    public DateTime ModifiedOn { get; set; }

    /// <summary>Additional metadata.</summary>
    public CertificateMeta? Meta { get; set; }
}

/// <summary>
/// Metadata associated with a certificate.
/// </summary>
public class CertificateMeta
{
    /// <summary>Let's Encrypt email address.</summary>
    public string? LetsencryptEmail { get; set; }

    /// <summary>Whether DNS challenge is used.</summary>
    public bool? DnsChallenge { get; set; }

    /// <summary>DNS provider for DNS challenge.</summary>
    public string? DnsProvider { get; set; }

    /// <summary>DNS provider credentials.</summary>
    public string? DnsProviderCredentials { get; set; }

    /// <summary>Whether the user agreed to Let's Encrypt terms.</summary>
    public bool? LetsencryptAgree { get; set; }
}
