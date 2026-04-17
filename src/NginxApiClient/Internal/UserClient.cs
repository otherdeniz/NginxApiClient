using NginxApiClient.Clients;
using NginxApiClient.Models.Users;

namespace NginxApiClient.Internal;

internal sealed class UserClient : IUserClient
{
    private readonly HttpClient _httpClient;
    private readonly IJsonSerializer _serializer;
    private const string BasePath = "/api/users";

    public UserClient(HttpClient httpClient, IJsonSerializer serializer)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task<IReadOnlyList<UserResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BasePath, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<List<UserResponse>>(json) ?? new List<UserResponse>();
    }

    public async Task<UserResponse> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BasePath}/{id}", cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<UserResponse>(json);
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        string requestJson = _serializer.Serialize(request);
        using var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(BasePath, content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<UserResponse>(json);
    }

    public async Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        string requestJson = _serializer.Serialize(request);
        using var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{BasePath}/{id}", content, cancellationToken).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return _serializer.Deserialize<UserResponse>(json);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default) =>
        await _httpClient.DeleteAsync($"{BasePath}/{id}", cancellationToken).ConfigureAwait(false);
}
