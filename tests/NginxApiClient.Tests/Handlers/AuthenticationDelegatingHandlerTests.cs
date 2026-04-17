using System.Net;
using FluentAssertions;
using NginxApiClient.Exceptions;
using NginxApiClient.Internal;
using NginxApiClient.SystemTextJson;
using NginxApiClient.Tests.Helpers;
using Xunit;

namespace NginxApiClient.Tests.Handlers;

public class AuthenticationDelegatingHandlerTests
{
    private readonly SystemTextJsonSerializer _serializer = new();
    private readonly NginxProxyManagerClientOptions _options = new()
    {
        BaseUrl = "http://localhost:81",
        Credentials = new NginxCredentials("admin@example.com", "changeme"),
    };

    private const string ValidTokenResponse = """{"token":"jwt-test-token","expires":"2099-12-31T23:59:59Z"}""";

    private HttpClient CreateClientWithAuthHandler(MockHttpMessageHandler mockHandler)
    {
        var authHandler = new AuthenticationDelegatingHandler(_options, _serializer, mockHandler);
        return new HttpClient(authHandler) { BaseAddress = new Uri("http://localhost:81") };
    }

    [Fact]
    public async Task SendAsync_AddsBearerHeader_ToRequests()
    {
        var mock = new MockHttpMessageHandler();
        // First call: token acquisition
        mock.EnqueueResponse(HttpStatusCode.OK, ValidTokenResponse);
        // Second call: actual API request
        mock.EnqueueResponse(HttpStatusCode.OK, "[]");

        using var client = CreateClientWithAuthHandler(mock);

        await client.GetAsync("/api/nginx/proxy-hosts");

        mock.SentRequests.Should().HaveCount(2);
        var apiRequest = mock.SentRequests[1];
        apiRequest.Headers.Authorization.Should().NotBeNull();
        apiRequest.Headers.Authorization!.Scheme.Should().Be("Bearer");
        apiRequest.Headers.Authorization!.Parameter.Should().Be("jwt-test-token");
    }

    [Fact]
    public async Task SendAsync_SkipsAuth_ForTokenEndpoint()
    {
        var mock = new MockHttpMessageHandler();
        mock.EnqueueResponse(HttpStatusCode.OK, ValidTokenResponse);

        using var client = CreateClientWithAuthHandler(mock);

        await client.PostAsync("/api/tokens", new StringContent("{}"));

        mock.SentRequests.Should().HaveCount(1);
        mock.SentRequests[0].Headers.Authorization.Should().BeNull();
    }

    [Fact]
    public async Task SendAsync_RetriesOnce_On401()
    {
        var mock = new MockHttpMessageHandler();
        // Token acquisition
        mock.EnqueueResponse(HttpStatusCode.OK, ValidTokenResponse);
        // First API call returns 401
        mock.EnqueueResponse(HttpStatusCode.Unauthorized);
        // Token re-acquisition
        mock.EnqueueResponse(HttpStatusCode.OK, """{"token":"new-token","expires":"2099-12-31T23:59:59Z"}""");
        // Retry succeeds
        mock.EnqueueResponse(HttpStatusCode.OK, "[]");

        using var client = CreateClientWithAuthHandler(mock);

        var response = await client.GetAsync("/api/nginx/proxy-hosts");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // Should have: token acquire, API call (401), token re-acquire, API retry
        mock.SentRequests.Should().HaveCount(4);
    }

    [Fact]
    public async Task SendAsync_ThrowsAuthException_OnDouble401()
    {
        var mock = new MockHttpMessageHandler();
        // Token acquisition
        mock.EnqueueResponse(HttpStatusCode.OK, ValidTokenResponse);
        // First API call returns 401
        mock.EnqueueResponse(HttpStatusCode.Unauthorized);
        // Token re-acquisition
        mock.EnqueueResponse(HttpStatusCode.OK, """{"token":"still-bad","expires":"2099-12-31T23:59:59Z"}""");
        // Retry also returns 401
        mock.EnqueueResponse(HttpStatusCode.Unauthorized);

        using var client = CreateClientWithAuthHandler(mock);

        var act = () => client.GetAsync("/api/nginx/proxy-hosts");

        await act.Should().ThrowAsync<NginxAuthenticationException>();
    }

    [Fact]
    public async Task SendAsync_ThrowsAuthException_WhenTokenAcquisitionFails()
    {
        var mock = new MockHttpMessageHandler();
        // Token acquisition fails
        mock.EnqueueResponse(HttpStatusCode.Unauthorized, "{\"error\":\"bad credentials\"}");

        using var client = CreateClientWithAuthHandler(mock);

        var act = () => client.GetAsync("/api/nginx/proxy-hosts");

        await act.Should().ThrowAsync<NginxAuthenticationException>();
    }

    [Fact]
    public async Task SendAsync_PostsCorrectCredentials_ToTokenEndpoint()
    {
        var mock = new MockHttpMessageHandler();
        mock.EnqueueResponse(HttpStatusCode.OK, ValidTokenResponse);
        mock.EnqueueResponse(HttpStatusCode.OK, "[]");

        using var client = CreateClientWithAuthHandler(mock);

        await client.GetAsync("/api/nginx/proxy-hosts");

        var tokenRequest = mock.SentRequests[0];
        tokenRequest.Method.Should().Be(HttpMethod.Post);
        tokenRequest.RequestUri!.ToString().Should().Contain("/api/tokens");

        string? body = mock.SentRequestBodies[0];
        body.Should().Contain("admin@example.com");
        body.Should().Contain("changeme");
    }
}
