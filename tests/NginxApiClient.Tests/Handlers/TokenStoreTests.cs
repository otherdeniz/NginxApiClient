using FluentAssertions;
using NginxApiClient.Internal;
using NginxApiClient.Models.Tokens;
using Xunit;

namespace NginxApiClient.Tests.Handlers;

public class TokenStoreTests
{
    [Fact]
    public async Task GetTokenAsync_AcquiresToken_WhenNoTokenExists()
    {
        int callCount = 0;
        var store = new TokenStore(async ct =>
        {
            Interlocked.Increment(ref callCount);
            return new TokenResponse { Token = "jwt-token-123", Expires = DateTime.UtcNow.AddHours(1) };
        });

        string token = await store.GetTokenAsync();

        token.Should().Be("jwt-token-123");
        callCount.Should().Be(1);
    }

    [Fact]
    public async Task GetTokenAsync_ReturnsCachedToken_WhenNotExpired()
    {
        int callCount = 0;
        var store = new TokenStore(async ct =>
        {
            Interlocked.Increment(ref callCount);
            return new TokenResponse { Token = "jwt-token", Expires = DateTime.UtcNow.AddHours(1) };
        });

        await store.GetTokenAsync();
        await store.GetTokenAsync();
        await store.GetTokenAsync();

        callCount.Should().Be(1);
    }

    [Fact]
    public async Task GetTokenAsync_RefreshesToken_WhenNearExpiry()
    {
        int callCount = 0;
        var store = new TokenStore(async ct =>
        {
            Interlocked.Increment(ref callCount);
            return new TokenResponse
            {
                Token = $"token-{callCount}",
                Expires = callCount == 1
                    ? DateTime.UtcNow.AddSeconds(10) // Near expiry (within 30s threshold)
                    : DateTime.UtcNow.AddHours(1),
            };
        }, refreshThreshold: TimeSpan.FromSeconds(30));

        string token1 = await store.GetTokenAsync();
        string token2 = await store.GetTokenAsync(); // Should trigger refresh

        callCount.Should().Be(2);
        token2.Should().Be("token-2");
    }

    [Fact]
    public async Task GetTokenAsync_ConcurrentCalls_TriggerSingleAcquisition()
    {
        int callCount = 0;
        var store = new TokenStore(async ct =>
        {
            Interlocked.Increment(ref callCount);
            await Task.Delay(50, ct); // Simulate network delay
            return new TokenResponse { Token = "shared-token", Expires = DateTime.UtcNow.AddHours(1) };
        });

        var tasks = Enumerable.Range(0, 10).Select(_ => store.GetTokenAsync()).ToArray();
        var tokens = await Task.WhenAll(tasks);

        callCount.Should().Be(1);
        tokens.Should().AllBe("shared-token");
    }

    [Fact]
    public async Task Invalidate_ForcesReAcquisition()
    {
        int callCount = 0;
        var store = new TokenStore(async ct =>
        {
            Interlocked.Increment(ref callCount);
            return new TokenResponse { Token = $"token-{callCount}", Expires = DateTime.UtcNow.AddHours(1) };
        });

        string token1 = await store.GetTokenAsync();
        store.Invalidate();
        string token2 = await store.GetTokenAsync();

        callCount.Should().Be(2);
        token1.Should().Be("token-1");
        token2.Should().Be("token-2");
    }

    [Fact]
    public void ToString_DoesNotExposeToken()
    {
        var store = new TokenStore(async ct =>
            new TokenResponse { Token = "secret-jwt", Expires = DateTime.UtcNow.AddHours(1) });

        store.ToString().Should().NotContain("secret-jwt");
        store.ToString().Should().Contain("***");
    }
}
