using NginxApiClient.Clients;
using NginxApiClient.Models.Streams;

namespace NginxApiClient.Internal;

internal sealed class StreamClient : IStreamClient
{
    private readonly HttpClient _httpClient;
    private readonly IJsonSerializer _serializer;
    private const string BasePath = "/api/nginx/streams";

    public StreamClient(HttpClient httpClient, IJsonSerializer serializer)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task<IReadOnlyList<StreamResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BasePath, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<List<StreamResponse>>(json) ?? new List<StreamResponse>();
    }

    public async Task<StreamResponse> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BasePath}/{id}", cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<StreamResponse>(json);
    }

    public async Task<StreamResponse> CreateAsync(CreateStreamRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        string requestJson = _serializer.Serialize(request);
        using var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(BasePath, content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<StreamResponse>(json);
    }

    public async Task<StreamResponse> UpdateAsync(int id, UpdateStreamRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        string requestJson = _serializer.Serialize(request);
        using var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{BasePath}/{id}", content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<StreamResponse>(json);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default) =>
        await _httpClient.DeleteAsync($"{BasePath}/{id}", cancellationToken).ConfigureAwait(false);

    public async Task EnableAsync(int id, CancellationToken cancellationToken = default)
    {
        using var content = new StringContent(_serializer.Serialize(new { enabled = true }), System.Text.Encoding.UTF8, "application/json");
        await _httpClient.PutAsync($"{BasePath}/{id}", content, cancellationToken).ConfigureAwait(false);
    }

    public async Task DisableAsync(int id, CancellationToken cancellationToken = default)
    {
        using var content = new StringContent(_serializer.Serialize(new { enabled = false }), System.Text.Encoding.UTF8, "application/json");
        await _httpClient.PutAsync($"{BasePath}/{id}", content, cancellationToken).ConfigureAwait(false);
    }
}
