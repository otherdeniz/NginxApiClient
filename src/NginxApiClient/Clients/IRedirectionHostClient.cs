using NginxApiClient.Models.RedirectionHosts;

namespace NginxApiClient.Clients;

/// <summary>Client for managing NPM redirection hosts.</summary>
public interface IRedirectionHostClient
{
    /// <summary>Lists all redirection hosts.</summary>
    Task<IReadOnlyList<RedirectionHostResponse>> ListAsync(CancellationToken cancellationToken = default);
    /// <summary>Gets a redirection host by ID.</summary>
    Task<RedirectionHostResponse> GetAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Creates a new redirection host.</summary>
    Task<RedirectionHostResponse> CreateAsync(CreateRedirectionHostRequest request, CancellationToken cancellationToken = default);
    /// <summary>Updates an existing redirection host.</summary>
    Task<RedirectionHostResponse> UpdateAsync(int id, UpdateRedirectionHostRequest request, CancellationToken cancellationToken = default);
    /// <summary>Deletes a redirection host.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Enables a redirection host.</summary>
    Task EnableAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Disables a redirection host.</summary>
    Task DisableAsync(int id, CancellationToken cancellationToken = default);
}
