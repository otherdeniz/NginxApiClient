using NginxApiClient.Models.DeadHosts;

namespace NginxApiClient.Clients;

/// <summary>Client for managing NPM dead hosts (custom 404 pages).</summary>
public interface IDeadHostClient
{
    /// <summary>Lists all dead hosts.</summary>
    Task<IReadOnlyList<DeadHostResponse>> ListAsync(CancellationToken cancellationToken = default);
    /// <summary>Gets a dead host by ID.</summary>
    Task<DeadHostResponse> GetAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Creates a new dead host.</summary>
    Task<DeadHostResponse> CreateAsync(CreateDeadHostRequest request, CancellationToken cancellationToken = default);
    /// <summary>Updates an existing dead host.</summary>
    Task<DeadHostResponse> UpdateAsync(int id, UpdateDeadHostRequest request, CancellationToken cancellationToken = default);
    /// <summary>Deletes a dead host.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Enables a dead host.</summary>
    Task EnableAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Disables a dead host.</summary>
    Task DisableAsync(int id, CancellationToken cancellationToken = default);
}
