using NginxApiClient.Models.Tokens;

namespace NginxApiClient.Internal;

/// <summary>
/// Thread-safe in-memory store for JWT tokens. Uses <see cref="SemaphoreSlim"/>
/// to ensure only one token acquisition/refresh occurs at a time under concurrent access.
/// </summary>
internal sealed class TokenStore
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly Func<CancellationToken, Task<TokenResponse>> _tokenAcquirer;
    private readonly TimeSpan _refreshThreshold;

    private string? _token;
    private DateTime _expiresAt;

    /// <summary>
    /// Initializes a new <see cref="TokenStore"/>.
    /// </summary>
    /// <param name="tokenAcquirer">Function that acquires a new token from the NPM API.</param>
    /// <param name="refreshThreshold">How long before expiry to proactively refresh. Default: 30 seconds.</param>
    public TokenStore(Func<CancellationToken, Task<TokenResponse>> tokenAcquirer, TimeSpan? refreshThreshold = null)
    {
        _tokenAcquirer = tokenAcquirer ?? throw new ArgumentNullException(nameof(tokenAcquirer));
        _refreshThreshold = refreshThreshold ?? TimeSpan.FromSeconds(30);
    }

    /// <summary>
    /// Gets the current valid token, acquiring or refreshing if necessary.
    /// Thread-safe — concurrent callers will wait for a single acquisition.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A valid JWT bearer token string.</returns>
    public async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(_token) && DateTime.UtcNow < _expiresAt.Subtract(_refreshThreshold))
        {
            return _token!;
        }

        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            // Double-check after acquiring semaphore (another thread may have refreshed)
            if (!string.IsNullOrEmpty(_token) && DateTime.UtcNow < _expiresAt.Subtract(_refreshThreshold))
            {
                return _token!;
            }

            var response = await _tokenAcquirer(cancellationToken).ConfigureAwait(false);
            _token = response.Token;
            _expiresAt = response.Expires;
            return _token;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Invalidates the current token, forcing re-acquisition on the next <see cref="GetTokenAsync"/> call.
    /// </summary>
    public void Invalidate()
    {
        _token = null;
        _expiresAt = DateTime.MinValue;
    }

    /// <inheritdoc />
    public override string ToString() => "TokenStore(token=***)";
}
