namespace NginxApiClient.Models.AuditLog;

/// <summary>Response model for an audit log entry from the NPM API.</summary>
public class AuditLogEntry
{
    /// <summary>The unique identifier.</summary>
    public int Id { get; set; }
    /// <summary>The user ID that performed the action.</summary>
    public int UserId { get; set; }
    /// <summary>The object type affected (e.g., "proxy-host", "certificate").</summary>
    public string ObjectType { get; set; } = string.Empty;
    /// <summary>The ID of the affected object.</summary>
    public int ObjectId { get; set; }
    /// <summary>The action performed (e.g., "created", "updated", "deleted").</summary>
    public string Action { get; set; } = string.Empty;
    /// <summary>Additional metadata about the action.</summary>
    public object? Meta { get; set; }
    /// <summary>Creation timestamp.</summary>
    public DateTime CreatedOn { get; set; }
    /// <summary>Last modification timestamp.</summary>
    public DateTime ModifiedOn { get; set; }
}
