namespace NginxApiClient.Exceptions;

/// <summary>
/// Exception thrown when authentication with the NPM API fails.
/// This occurs when credentials are invalid or when token refresh fails after a 401 response.
/// </summary>
public class NginxAuthenticationException : NginxApiException
{
    /// <summary>
    /// Initializes a new instance of <see cref="NginxAuthenticationException"/> with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public NginxAuthenticationException(string message)
        : base(401, message, string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="NginxAuthenticationException"/> with full error details.
    /// </summary>
    /// <param name="errorDetail">The error detail from the NPM response.</param>
    /// <param name="rawResponse">The raw HTTP response body.</param>
    public NginxAuthenticationException(string errorDetail, string rawResponse)
        : base(401, errorDetail, rawResponse)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="NginxAuthenticationException"/> with full error details and an inner exception.
    /// </summary>
    /// <param name="errorDetail">The error detail from the NPM response.</param>
    /// <param name="rawResponse">The raw HTTP response body.</param>
    /// <param name="innerException">The inner exception.</param>
    public NginxAuthenticationException(string errorDetail, string rawResponse, Exception innerException)
        : base(401, errorDetail, rawResponse, innerException)
    {
    }
}
