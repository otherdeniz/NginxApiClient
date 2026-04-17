using NginxApiClient.Clients;
using NginxApiClient.Models.DeadHosts;

namespace NginxApiClient.Internal;

internal sealed class DeadHostClient : IDeadHostClient
{
    private readonly HttpClient _httpClient;
    private readonly IJsonSerializer _serializer;
    private const string BasePath = "/api/nginx/dead-hosts";

    public DeadHostClient(HttpClient httpClient, IJsonSerializer serializer)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task<IReadOnlyList<DeadHostResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BasePath, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<List<DeadHostResponse>>(json) ?? new List<DeadHostResponse>();
    }

    public async Task<DeadHostResponse> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BasePath}/{id}", cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<DeadHostResponse>(json);
    }

    public async Task<DeadHostResponse> CreateAsync(CreateDeadHostRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        string requestJson = _serializer.Serialize(request);
        using var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(BasePath, content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<DeadHostResponse>(json);
    }

    public async Task<DeadHostResponse> UpdateAsync(int id, UpdateDeadHostRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        string requestJson = _serializer.Serialize(request);
        using var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{BasePath}/{id}", content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<DeadHostResponse>(json);
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
