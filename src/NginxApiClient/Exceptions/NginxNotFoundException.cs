namespace NginxApiClient.Exceptions;

/// <summary>
/// Exception thrown when a requested NPM resource is not found (HTTP 404).
/// </summary>
public class NginxNotFoundException : NginxApiException
{
    /// <summary>
    /// Initializes a new instance of <see cref="NginxNotFoundException"/> with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public NginxNotFoundException(string message)
        : base(404, message, string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="NginxNotFoundException"/> with full error details.
    /// </summary>
    /// <param name="errorDetail">The error detail from the NPM response.</param>
    /// <param name="rawResponse">The raw HTTP response body.</param>
    public NginxNotFoundException(string errorDetail, string rawResponse)
        : base(404, errorDetail, rawResponse)
    {
    }
}
