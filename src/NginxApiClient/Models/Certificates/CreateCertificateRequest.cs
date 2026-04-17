namespace NginxApiClient.Models.Certificates;

/// <summary>
/// Request model for provisioning a new certificate (e.g., Let's Encrypt) in NPM.
/// </summary>
public class CreateCertificateRequest
{
    /// <summary>The domain names to provision the certificate for. Required.</summary>
    public List<string> DomainNames { get; set; } = new();

    /// <summary>The certificate provider. Use "letsencrypt" for Let's Encrypt. Required.</summary>
    public string Provider { get; set; } = "letsencrypt";

    /// <summary>A friendly name for the certificate.</summary>
    public string? NiceName { get; set; }

    /// <summary>Additional metadata for provisioning (e.g., Let's Encrypt email, DNS challenge settings).</summary>
    public CertificateMeta? Meta { get; set; }
}
