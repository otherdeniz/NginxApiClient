namespace NginxApiClient.Models.AccessLists;

/// <summary>Response model for an access list from the NPM API.</summary>
public class AccessListResponse
{
    /// <summary>The unique identifier.</summary>
    public int Id { get; set; }
    /// <summary>The name of the access list.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Whether to satisfy any authorization requirement.</summary>
    public bool SatisfyAny { get; set; }
    /// <summary>Whether to pass authorization headers to the backend.</summary>
    public bool PassAuth { get; set; }
    /// <summary>Authorization entries (HTTP basic auth users).</summary>
    public List<AccessListAuth>? Items { get; set; }
    /// <summary>Client IP allow/deny rules.</summary>
    public List<AccessListClient>? Clients { get; set; }
    /// <summary>Creation timestamp.</summary>
    public DateTime CreatedOn { get; set; }
    /// <summary>Last modification timestamp.</summary>
    public DateTime ModifiedOn { get; set; }
}

/// <summary>An authorization entry (username/password) in an access list.</summary>
public class AccessListAuth
{
    /// <summary>The username.</summary>
    public string Username { get; set; } = string.Empty;
    /// <summary>The password.</summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>A client IP rule in an access list.</summary>
public class AccessListClient
{
    /// <summary>The IP address or CIDR range.</summary>
    public string Address { get; set; } = string.Empty;
    /// <summary>Whether this is an allow or deny rule.</summary>
    public string Directive { get; set; } = "allow";
}

/// <summary>Request model for creating an access list.</summary>
public class CreateAccessListRequest
{
    /// <summary>The name of the access list. Required.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Whether to satisfy any authorization requirement.</summary>
    public bool SatisfyAny { get; set; }
    /// <summary>Whether to pass authorization headers to the backend.</summary>
    public bool PassAuth { get; set; }
    /// <summary>Authorization entries.</summary>
    public List<AccessListAuth>? Items { get; set; }
    /// <summary>Client IP rules.</summary>
    public List<AccessListClient>? Clients { get; set; }
}

/// <summary>Request model for updating an access list.</summary>
public class UpdateAccessListRequest
{
    /// <summary>The name.</summary>
    public string? Name { get; set; }
    /// <summary>Whether to satisfy any.</summary>
    public bool? SatisfyAny { get; set; }
    /// <summary>Whether to pass auth.</summary>
    public bool? PassAuth { get; set; }
    /// <summary>Authorization entries.</summary>
    public List<AccessListAuth>? Items { get; set; }
    /// <summary>Client IP rules.</summary>
    public List<AccessListClient>? Clients { get; set; }
}
