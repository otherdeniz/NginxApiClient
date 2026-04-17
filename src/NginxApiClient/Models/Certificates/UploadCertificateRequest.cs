namespace NginxApiClient.Models.Certificates;

/// <summary>
/// Request model for uploading a custom SSL certificate to NPM.
/// </summary>
public class UploadCertificateRequest
{
    /// <summary>A friendly name for the certificate. Required.</summary>
    public string NiceName { get; set; } = string.Empty;

    /// <summary>The PEM-encoded certificate content. Required.</summary>
    public string Certificate { get; set; } = string.Empty;

    /// <summary>The PEM-encoded private key content. Required.</summary>
    public string CertificateKey { get; set; } = string.Empty;

    /// <summary>The PEM-encoded intermediate certificate chain, if applicable.</summary>
    public string? IntermediateCertificate { get; set; }
}
