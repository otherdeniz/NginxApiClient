using NginxApiClient.Models.Users;

namespace NginxApiClient.Clients;

/// <summary>Client for managing NPM users and permissions.</summary>
public interface IUserClient
{
    /// <summary>Lists all users.</summary>
    Task<IReadOnlyList<UserResponse>> ListAsync(CancellationToken cancellationToken = default);
    /// <summary>Gets a user by ID.</summary>
    Task<UserResponse> GetAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Creates a new user.</summary>
    Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    /// <summary>Updates an existing user.</summary>
    Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    /// <summary>Deletes a user.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
