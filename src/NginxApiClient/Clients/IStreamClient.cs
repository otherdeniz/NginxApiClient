using NginxApiClient.Models.Streams;

namespace NginxApiClient.Clients;

/// <summary>Client for managing NPM TCP/UDP streams.</summary>
public interface IStreamClient
{
    /// <summary>Lists all streams.</summary>
    Task<IReadOnlyList<StreamResponse>> ListAsync(CancellationToken cancellationToken = default);
    /// <summary>Gets a stream by ID.</summary>
    Task<StreamResponse> GetAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Creates a new stream.</summary>
    Task<StreamResponse> CreateAsync(CreateStreamRequest request, CancellationToken cancellationToken = default);
    /// <summary>Updates an existing stream.</summary>
    Task<StreamResponse> UpdateAsync(int id, UpdateStreamRequest request, CancellationToken cancellationToken = default);
    /// <summary>Deletes a stream.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Enables a stream.</summary>
    Task EnableAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Disables a stream.</summary>
    Task DisableAsync(int id, CancellationToken cancellationToken = default);
}
