using NginxApiClient.Models.Settings;

namespace NginxApiClient.Clients;

/// <summary>Client for managing NPM settings.</summary>
public interface ISettingsClient
{
    /// <summary>Gets the current NPM settings.</summary>
    Task<SettingsResponse> GetAsync(CancellationToken cancellationToken = default);
    /// <summary>Updates NPM settings.</summary>
    Task<SettingsResponse> UpdateAsync(UpdateSettingsRequest request, CancellationToken cancellationToken = default);
}
