using NginxApiClient.Clients;
using NginxApiClient.Models.Certificates;

namespace NginxApiClient.Internal;

/// <summary>
/// Internal implementation of <see cref="ICertificateClient"/>.
/// </summary>
internal sealed class CertificateClient : ICertificateClient
{
    private readonly HttpClient _httpClient;
    private readonly IJsonSerializer _serializer;
    private const string BasePath = "/api/nginx/certificates";

    public CertificateClient(HttpClient httpClient, IJsonSerializer serializer)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<CertificateResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BasePath, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<List<CertificateResponse>>(json) ?? new List<CertificateResponse>();
    }

    /// <inheritdoc />
    public async Task<CertificateResponse> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BasePath}/{id}", cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<CertificateResponse>(json);
    }

    /// <inheritdoc />
    public async Task<CertificateResponse> CreateAsync(CreateCertificateRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        string requestJson = _serializer.Serialize(request);
        using var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(BasePath, content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<CertificateResponse>(json);
    }

    /// <inheritdoc />
    public async Task<CertificateResponse> UploadAsync(UploadCertificateRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        string requestJson = _serializer.Serialize(request);
        using var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{BasePath}/upload", content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<CertificateResponse>(json);
    }

    /// <inheritdoc />
    public async Task<byte[]> DownloadAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BasePath}/{id}/download", cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<CertificateResponse> RenewAsync(int id, CancellationToken cancellationToken = default)
    {
        using var content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{BasePath}/{id}/renew", content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<CertificateResponse>(json);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _httpClient.DeleteAsync($"{BasePath}/{id}", cancellationToken).ConfigureAwait(false);
    }
}
