using NginxApiClient.Models.AuditLog;

namespace NginxApiClient.Clients;

/// <summary>Client for reading NPM audit log entries.</summary>
public interface IAuditLogClient
{
    /// <summary>Lists audit log entries.</summary>
    Task<IReadOnlyList<AuditLogEntry>> ListAsync(CancellationToken cancellationToken = default);
}
