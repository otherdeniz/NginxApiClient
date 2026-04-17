namespace NginxApiClient.Exceptions;

/// <summary>
/// Base exception for all NGINX Proxy Manager API errors.
/// Contains the HTTP status code, error detail from the NPM response, and the raw response body.
/// </summary>
public class NginxApiException : Exception
{
    /// <summary>
    /// The HTTP status code returned by the NPM API, or 0 if the error was not HTTP-related.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// The error detail message parsed from the NPM API response body, if available.
    /// </summary>
    public string ErrorDetail { get; }

    /// <summary>
    /// The raw HTTP response body, useful for debugging. Never contains credentials or tokens.
    /// </summary>
    public string RawResponse { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="NginxApiException"/> with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public NginxApiException(string message)
        : base(message)
    {
        ErrorDetail = string.Empty;
        RawResponse = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="NginxApiException"/> with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception that caused this error.</param>
    public NginxApiException(string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorDetail = string.Empty;
        RawResponse = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="NginxApiException"/> with full error details from an NPM API response.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="errorDetail">The error detail parsed from the response body.</param>
    /// <param name="rawResponse">The raw HTTP response body.</param>
    public NginxApiException(int statusCode, string errorDetail, string rawResponse)
        : base($"NPM API error {statusCode}: {errorDetail}")
    {
        StatusCode = statusCode;
        ErrorDetail = errorDetail ?? string.Empty;
        RawResponse = rawResponse ?? string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="NginxApiException"/> with full error details and an inner exception.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="errorDetail">The error detail parsed from the response body.</param>
    /// <param name="rawResponse">The raw HTTP response body.</param>
    /// <param name="innerException">The inner exception that caused this error.</param>
    public NginxApiException(int statusCode, string errorDetail, string rawResponse, Exception innerException)
        : base($"NPM API error {statusCode}: {errorDetail}", innerException)
    {
        StatusCode = statusCode;
        ErrorDetail = errorDetail ?? string.Empty;
        RawResponse = rawResponse ?? string.Empty;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{GetType().Name}: HTTP {StatusCode} — {ErrorDetail}{(InnerException != null ? $"\n---> {InnerException}" : "")}";
    }
}
