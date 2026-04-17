using System.Net;
using NginxApiClient.Exceptions;

namespace NginxApiClient.Internal;

/// <summary>
/// HTTP pipeline handler that maps non-success HTTP responses to typed exceptions.
/// Sits after <see cref="AuthenticationDelegatingHandler"/> in the pipeline.
/// Does NOT handle 401 responses (those are handled by the auth handler).
/// </summary>
internal sealed class ErrorHandlingDelegatingHandler : DelegatingHandler
{
    private readonly IJsonSerializer _serializer;

    /// <summary>
    /// Initializes a new <see cref="ErrorHandlingDelegatingHandler"/>.
    /// </summary>
    /// <param name="serializer">JSON serializer for parsing error responses.</param>
    public ErrorHandlingDelegatingHandler(IJsonSerializer serializer)
    {
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    /// <summary>
    /// Initializes a new <see cref="ErrorHandlingDelegatingHandler"/> with a specific inner handler.
    /// </summary>
    /// <param name="serializer">JSON serializer.</param>
    /// <param name="innerHandler">The inner HTTP message handler.</param>
    public ErrorHandlingDelegatingHandler(IJsonSerializer serializer, HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response;

        try
        {
            response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            throw new NginxApiException("Failed to connect to NPM instance", ex);
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            throw new NginxApiException("Request to NPM instance timed out", ex);
        }

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        // Don't handle 401 — that's the auth handler's job
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return response;
        }

        string rawResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        string errorDetail = ParseErrorDetail(rawResponse);
        int statusCode = (int)response.StatusCode;

        response.Dispose();

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NginxNotFoundException(errorDetail, rawResponse);
        }

        throw new NginxApiException(statusCode, errorDetail, rawResponse);
    }

    private string ParseErrorDetail(string rawResponse)
    {
        if (string.IsNullOrWhiteSpace(rawResponse))
        {
            return "No error detail provided";
        }

        try
        {
            var errorObj = _serializer.Deserialize<NpmErrorResponse>(rawResponse);
            if (errorObj?.Error?.Message is { Length: > 0 } errorMessage)
            {
                return errorMessage;
            }

            if (errorObj?.Message is { Length: > 0 } message)
            {
                return message;
            }
        }
        catch
        {
            // If we can't parse the error response, return the raw body
        }

        return rawResponse.Length > 500 ? rawResponse.Substring(0, 500) : rawResponse;
    }

    /// <summary>
    /// Internal model for parsing NPM error responses. NPM uses varying error formats.
    /// </summary>
    private class NpmErrorResponse
    {
        /// <summary>Top-level message field.</summary>
        public string? Message { get; set; }

        /// <summary>Nested error object.</summary>
        public NpmErrorDetail? Error { get; set; }
    }

    /// <summary>Nested error detail in NPM responses.</summary>
    private class NpmErrorDetail
    {
        /// <summary>Error message.</summary>
        public string? Message { get; set; }
    }
}
