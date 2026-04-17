using NginxApiClient.Clients;
using NginxApiClient.Models.ProxyHosts;

namespace NginxApiClient.Internal;

/// <summary>
/// Internal implementation of <see cref="IProxyHostClient"/>.
/// </summary>
internal sealed class ProxyHostClient : IProxyHostClient
{
    private readonly HttpClient _httpClient;
    private readonly IJsonSerializer _serializer;
    private const string BasePath = "/api/nginx/proxy-hosts";

    public ProxyHostClient(HttpClient httpClient, IJsonSerializer serializer)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ProxyHostResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BasePath, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<List<ProxyHostResponse>>(json) ?? new List<ProxyHostResponse>();
    }

    /// <inheritdoc />
    public async Task<ProxyHostResponse> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BasePath}/{id}", cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<ProxyHostResponse>(json);
    }

    /// <inheritdoc />
    public async Task<ProxyHostResponse> CreateAsync(CreateProxyHostRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        string requestJson = _serializer.Serialize(request);
        using var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(BasePath, content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<ProxyHostResponse>(json);
    }

    /// <inheritdoc />
    public async Task<ProxyHostResponse> UpdateAsync(int id, UpdateProxyHostRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        string requestJson = _serializer.Serialize(request);
        using var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{BasePath}/{id}", content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<ProxyHostResponse>(json);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _httpClient.DeleteAsync($"{BasePath}/{id}", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task EnableAsync(int id, CancellationToken cancellationToken = default)
    {
        using var content = new StringContent(
            _serializer.Serialize(new { enabled = true }),
            System.Text.Encoding.UTF8,
            "application/json");
        await _httpClient.PutAsync($"{BasePath}/{id}", content, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task DisableAsync(int id, CancellationToken cancellationToken = default)
    {
        using var content = new StringContent(
            _serializer.Serialize(new { enabled = false }),
            System.Text.Encoding.UTF8,
            "application/json");
        await _httpClient.PutAsync($"{BasePath}/{id}", content, cancellationToken).ConfigureAwait(false);
    }
}
