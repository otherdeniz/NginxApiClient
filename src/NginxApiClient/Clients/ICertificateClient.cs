using NginxApiClient.Models.Certificates;

namespace NginxApiClient.Clients;

/// <summary>
/// Client for managing NGINX Proxy Manager SSL certificates.
/// Supports Let's Encrypt provisioning, custom certificate upload, and certificate lifecycle management.
/// </summary>
public interface ICertificateClient
{
    /// <summary>Lists all certificates.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All certificates configured in NPM.</returns>
    Task<IReadOnlyList<CertificateResponse>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a certificate by ID.</summary>
    /// <param name="id">The certificate ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The certificate.</returns>
    /// <exception cref="Exceptions.NginxNotFoundException">Thrown when the certificate is not found.</exception>
    Task<CertificateResponse> GetAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Creates/provisions a new certificate (e.g., Let's Encrypt).</summary>
    /// <param name="request">The certificate provisioning request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created certificate.</returns>
    Task<CertificateResponse> CreateAsync(CreateCertificateRequest request, CancellationToken cancellationToken = default);

    /// <summary>Uploads a custom SSL certificate.</summary>
    /// <param name="request">The certificate upload request with certificate and key content.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The uploaded certificate.</returns>
    Task<CertificateResponse> UploadAsync(UploadCertificateRequest request, CancellationToken cancellationToken = default);

    /// <summary>Downloads a certificate's content.</summary>
    /// <param name="id">The certificate ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The certificate content as bytes.</returns>
    /// <exception cref="Exceptions.NginxNotFoundException">Thrown when the certificate is not found.</exception>
    Task<byte[]> DownloadAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Renews an existing certificate.</summary>
    /// <param name="id">The certificate ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The renewed certificate.</returns>
    /// <exception cref="Exceptions.NginxNotFoundException">Thrown when the certificate is not found.</exception>
    Task<CertificateResponse> RenewAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Deletes a certificate.</summary>
    /// <param name="id">The certificate ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="Exceptions.NginxNotFoundException">Thrown when the certificate is not found.</exception>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
