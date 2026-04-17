using NginxApiClient.Clients;
using NginxApiClient.Models.Settings;

namespace NginxApiClient.Internal;

internal sealed class SettingsClient : ISettingsClient
{
    private readonly HttpClient _httpClient;
    private readonly IJsonSerializer _serializer;
    private const string BasePath = "/api/settings";

    public SettingsClient(HttpClient httpClient, IJsonSerializer serializer)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task<SettingsResponse> GetAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BasePath, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<SettingsResponse>(json);
    }

    public async Task<SettingsResponse> UpdateAsync(UpdateSettingsRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        string requestJson = _serializer.Serialize(request);
        using var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(BasePath, content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<SettingsResponse>(json);
    }
}
