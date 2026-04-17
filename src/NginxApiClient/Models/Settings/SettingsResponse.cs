namespace NginxApiClient.Models.Settings;

/// <summary>Response model for NPM settings.</summary>
public class SettingsResponse
{
    /// <summary>The unique identifier.</summary>
    public int Id { get; set; }
    /// <summary>The default site behavior ("congratulations", "404", "redirect", "html").</summary>
    public string DefaultSite { get; set; } = string.Empty;
    /// <summary>The redirect URL when default site is "redirect".</summary>
    public string? DefaultSiteRedirect { get; set; }
    /// <summary>Creation timestamp.</summary>
    public DateTime CreatedOn { get; set; }
    /// <summary>Last modification timestamp.</summary>
    public DateTime ModifiedOn { get; set; }
}

/// <summary>Request model for updating NPM settings.</summary>
public class UpdateSettingsRequest
{
    /// <summary>The default site behavior.</summary>
    public string? DefaultSite { get; set; }
    /// <summary>The redirect URL when default site is "redirect".</summary>
    public string? DefaultSiteRedirect { get; set; }
}
