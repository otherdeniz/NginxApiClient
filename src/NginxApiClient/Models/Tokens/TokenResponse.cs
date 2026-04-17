namespace NginxApiClient.Models.Tokens;

/// <summary>
/// Response model for a JWT token from the NPM API.
/// Contains the bearer token and its expiration time.
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// The JWT bearer token string.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// The expiration date/time of the token.
    /// </summary>
    public DateTime Expires { get; set; }
}
