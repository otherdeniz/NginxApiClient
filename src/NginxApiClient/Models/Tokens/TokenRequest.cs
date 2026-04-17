namespace NginxApiClient.Models.Tokens;

/// <summary>
/// Request model for obtaining a JWT token from the NPM API.
/// Maps to <c>POST /api/tokens</c> with <c>identity</c> (email) and <c>secret</c> (password).
/// </summary>
public class TokenRequest
{
    /// <summary>
    /// The login identity (email address). Maps to NPM JSON field <c>identity</c>.
    /// </summary>
    public string Identity { get; set; } = string.Empty;

    /// <summary>
    /// The login secret (password). Maps to NPM JSON field <c>secret</c>.
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Creates a <see cref="TokenRequest"/> from <see cref="NginxCredentials"/>.
    /// </summary>
    /// <param name="credentials">The credentials to use.</param>
    /// <returns>A token request with identity and secret populated.</returns>
    public static TokenRequest FromCredentials(NginxCredentials credentials)
    {
        return new TokenRequest
        {
            Identity = credentials.Email,
            Secret = credentials.Password,
        };
    }
}
