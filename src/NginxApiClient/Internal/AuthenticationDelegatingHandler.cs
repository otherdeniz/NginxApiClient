using System.Net;
using System.Net.Http.Headers;
using NginxApiClient.Exceptions;
using NginxApiClient.Models.Tokens;

namespace NginxApiClient.Internal;

/// <summary>
/// HTTP pipeline handler that transparently adds JWT Bearer authentication to all requests
/// and handles token refresh on 401 responses. Sits in the <see cref="HttpClient"/> pipeline.
/// </summary>
internal sealed class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly TokenStore _tokenStore;
    private readonly NginxProxyManagerClientOptions _options;
    private readonly IJsonSerializer _serializer;
    private const string TokenEndpoint = "/api/tokens";

    /// <summary>
    /// Initializes a new <see cref="AuthenticationDelegatingHandler"/>.
    /// </summary>
    /// <param name="options">Client configuration options.</param>
    /// <param name="serializer">JSON serializer for token requests/responses.</param>
    public AuthenticationDelegatingHandler(NginxProxyManagerClientOptions options, IJsonSerializer serializer)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _tokenStore = new TokenStore(AcquireTokenAsync);
    }

    /// <summary>
    /// Initializes a new <see cref="AuthenticationDelegatingHandler"/> with a specific inner handler.
    /// </summary>
    /// <param name="options">Client configuration options.</param>
    /// <param name="serializer">JSON serializer.</param>
    /// <param name="innerHandler">The inner HTTP message handler.</param>
    public AuthenticationDelegatingHandler(NginxProxyManagerClientOptions options, IJsonSerializer serializer, HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _tokenStore = new TokenStore(AcquireTokenAsync);
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Skip auth for token endpoint requests to avoid infinite loop
        if (IsTokenEndpoint(request))
        {
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        // Add Bearer token
        string token = await _tokenStore.GetTokenAsync(cancellationToken).ConfigureAwait(false);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        // Handle 401 — re-auth and retry once
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _tokenStore.Invalidate();
            string newToken = await _tokenStore.GetTokenAsync(cancellationToken).ConfigureAwait(false);

            // Clone the request for retry (original request may have been consumed)
            using var retryRequest = await CloneRequestAsync(request).ConfigureAwait(false);
            retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

            var retryResponse = await base.SendAsync(retryRequest, cancellationToken).ConfigureAwait(false);

            if (retryResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                string body = await retryResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new NginxAuthenticationException("Authentication failed after token refresh", body);
            }

            response.Dispose();
            return retryResponse;
        }

        return response;
    }

    private async Task<TokenResponse> AcquireTokenAsync(CancellationToken cancellationToken)
    {
        var tokenRequest = TokenRequest.FromCredentials(_options.Credentials);
        string json = _serializer.Serialize(tokenRequest);

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"),
        };

        var response = await base.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new NginxAuthenticationException("Failed to acquire token", responseBody);
        }

        var tokenResponse = _serializer.Deserialize<TokenResponse>(responseBody);
        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.Token))
        {
            throw new NginxAuthenticationException("Invalid token response from NPM API", responseBody);
        }

        return tokenResponse;
    }

    private static bool IsTokenEndpoint(HttpRequestMessage request)
    {
        return request.RequestUri?.AbsolutePath?.TrimEnd('/').EndsWith("/tokens", StringComparison.OrdinalIgnoreCase) == true
            || request.RequestUri?.ToString().Contains("/tokens") == true;
    }

    private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri);

        if (request.Content != null)
        {
            string content = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            clone.Content = new StringContent(content, System.Text.Encoding.UTF8, request.Content.Headers.ContentType?.MediaType ?? "application/json");
        }

        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return clone;
    }
}
