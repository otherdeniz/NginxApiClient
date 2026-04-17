namespace NginxApiClient.Models.Users;

/// <summary>Response model for a user from the NPM API.</summary>
public class UserResponse
{
    /// <summary>The unique identifier.</summary>
    public int Id { get; set; }
    /// <summary>The user's name.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>The user's nickname.</summary>
    public string Nickname { get; set; } = string.Empty;
    /// <summary>The user's email.</summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>Whether the user is an administrator.</summary>
    public bool IsDisabled { get; set; }
    /// <summary>The user's avatar URL.</summary>
    public string? Avatar { get; set; }
    /// <summary>The user's roles.</summary>
    public List<string>? Roles { get; set; }
    /// <summary>The user's permissions.</summary>
    public UserPermissions? Permissions { get; set; }
    /// <summary>Creation timestamp.</summary>
    public DateTime CreatedOn { get; set; }
    /// <summary>Last modification timestamp.</summary>
    public DateTime ModifiedOn { get; set; }
}

/// <summary>User permission flags.</summary>
public class UserPermissions
{
    /// <summary>Permission to manage proxy hosts.</summary>
    public string? ProxyHosts { get; set; }
    /// <summary>Permission to manage redirection hosts.</summary>
    public string? RedirectionHosts { get; set; }
    /// <summary>Permission to manage dead hosts.</summary>
    public string? DeadHosts { get; set; }
    /// <summary>Permission to manage streams.</summary>
    public string? Streams { get; set; }
    /// <summary>Permission to manage access lists.</summary>
    public string? AccessLists { get; set; }
    /// <summary>Permission to manage certificates.</summary>
    public string? Certificates { get; set; }
}

/// <summary>Request model for creating a user.</summary>
public class CreateUserRequest
{
    /// <summary>The user's name. Required.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>The user's nickname.</summary>
    public string Nickname { get; set; } = string.Empty;
    /// <summary>The user's email. Required.</summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>The user's roles.</summary>
    public List<string>? Roles { get; set; }
    /// <summary>Whether the user is disabled.</summary>
    public bool IsDisabled { get; set; }
    /// <summary>The user's password. Required for creation.</summary>
    public string? Auth { get; set; }
    /// <summary>The user's permissions.</summary>
    public UserPermissions? Permissions { get; set; }
}

/// <summary>Request model for updating a user.</summary>
public class UpdateUserRequest
{
    /// <summary>The user's name.</summary>
    public string? Name { get; set; }
    /// <summary>The user's nickname.</summary>
    public string? Nickname { get; set; }
    /// <summary>The user's email.</summary>
    public string? Email { get; set; }
    /// <summary>The user's roles.</summary>
    public List<string>? Roles { get; set; }
    /// <summary>Whether the user is disabled.</summary>
    public bool? IsDisabled { get; set; }
    /// <summary>The user's permissions.</summary>
    public UserPermissions? Permissions { get; set; }
}
