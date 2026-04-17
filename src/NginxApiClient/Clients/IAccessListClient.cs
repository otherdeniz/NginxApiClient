using NginxApiClient.Models.AccessLists;

namespace NginxApiClient.Clients;

/// <summary>Client for managing NPM access lists.</summary>
public interface IAccessListClient
{
    /// <summary>Lists all access lists.</summary>
    Task<IReadOnlyList<AccessListResponse>> ListAsync(CancellationToken cancellationToken = default);
    /// <summary>Gets an access list by ID.</summary>
    Task<AccessListResponse> GetAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Creates a new access list.</summary>
    Task<AccessListResponse> CreateAsync(CreateAccessListRequest request, CancellationToken cancellationToken = default);
    /// <summary>Updates an existing access list.</summary>
    Task<AccessListResponse> UpdateAsync(int id, UpdateAccessListRequest request, CancellationToken cancellationToken = default);
    /// <summary>Deletes an access list.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
