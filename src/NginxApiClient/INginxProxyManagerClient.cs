using NginxApiClient.Clients;

namespace NginxApiClient;

/// <summary>
/// Root client interface for the NGINX Proxy Manager API.
/// Access per-resource clients via the typed properties (e.g., <c>client.ProxyHosts.ListAsync()</c>).
/// Mock this interface and its sub-interfaces in unit tests to test consumer code without a live NPM instance.
/// </summary>
public interface INginxProxyManagerClient
{
    /// <summary>Client for managing proxy hosts.</summary>
    IProxyHostClient ProxyHosts { get; }

    /// <summary>Client for managing SSL certificates.</summary>
    ICertificateClient Certificates { get; }

    /// <summary>Client for managing redirection hosts.</summary>
    IRedirectionHostClient RedirectionHosts { get; }

    /// <summary>Client for managing dead hosts (custom 404 pages).</summary>
    IDeadHostClient DeadHosts { get; }

    /// <summary>Client for managing TCP/UDP streams.</summary>
    IStreamClient Streams { get; }

    /// <summary>Client for managing access lists.</summary>
    IAccessListClient AccessLists { get; }

    /// <summary>Client for managing NPM users.</summary>
    IUserClient Users { get; }

    /// <summary>Client for managing NPM settings.</summary>
    ISettingsClient Settings { get; }

    /// <summary>Client for reading audit log entries.</summary>
    IAuditLogClient AuditLog { get; }

    /// <summary>Client for accessing host reports and statistics.</summary>
    IReportsClient Reports { get; }
}
