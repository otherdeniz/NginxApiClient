using System.Net;
using FluentAssertions;
using NginxApiClient.Exceptions;
using NginxApiClient.Internal;
using NginxApiClient.SystemTextJson;
using NginxApiClient.Tests.Helpers;
using Xunit;

namespace NginxApiClient.Tests.Handlers;

public class ErrorHandlingDelegatingHandlerTests
{
    private readonly SystemTextJsonSerializer _serializer = new();

    private HttpClient CreateClientWithErrorHandler(MockHttpMessageHandler mockHandler)
    {
        var errorHandler = new ErrorHandlingDelegatingHandler(_serializer, mockHandler);
        return new HttpClient(errorHandler) { BaseAddress = new Uri("http://localhost:81") };
    }

    [Fact]
    public async Task SendAsync_PassesThrough_On200()
    {
        var mock = new MockHttpMessageHandler();
        mock.EnqueueResponse(HttpStatusCode.OK, "{\"id\": 1}");
        using var client = CreateClientWithErrorHandler(mock);

        var response = await client.GetAsync("/api/nginx/proxy-hosts");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SendAsync_ThrowsNotFoundException_On404()
    {
        var mock = new MockHttpMessageHandler();
        mock.EnqueueResponse(HttpStatusCode.NotFound, "{\"error\":{\"message\":\"Not Found\"}}");
        using var client = CreateClientWithErrorHandler(mock);

        var act = () => client.GetAsync("/api/nginx/proxy-hosts/999");

        var ex = await act.Should().ThrowAsync<NginxNotFoundException>();
        ex.Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task SendAsync_ThrowsNginxApiException_On422()
    {
        var mock = new MockHttpMessageHandler();
        mock.EnqueueResponse(HttpStatusCode.UnprocessableEntity, "{\"error\":{\"message\":\"domain_names is required\"}}");
        using var client = CreateClientWithErrorHandler(mock);

        var act = () => client.PostAsync("/api/nginx/proxy-hosts", new StringContent("{}"));

        var ex = await act.Should().ThrowAsync<NginxApiException>();
        ex.Which.StatusCode.Should().Be(422);
        ex.Which.ErrorDetail.Should().Contain("domain_names is required");
    }

    [Fact]
    public async Task SendAsync_ThrowsNginxApiException_On500()
    {
        var mock = new MockHttpMessageHandler();
        mock.EnqueueResponse(HttpStatusCode.InternalServerError, "{\"message\":\"Internal Server Error\"}");
        using var client = CreateClientWithErrorHandler(mock);

        var act = () => client.GetAsync("/api/nginx/proxy-hosts");

        var ex = await act.Should().ThrowAsync<NginxApiException>();
        ex.Which.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task SendAsync_PassesThrough401_ForAuthHandler()
    {
        var mock = new MockHttpMessageHandler();
        mock.EnqueueResponse(HttpStatusCode.Unauthorized, "");
        using var client = CreateClientWithErrorHandler(mock);

        var response = await client.GetAsync("/api/nginx/proxy-hosts");

        // 401 should pass through — auth handler deals with it
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SendAsync_IncludesRawResponse_InException()
    {
        string errorBody = "{\"error\":{\"message\":\"validation failed\"}}";
        var mock = new MockHttpMessageHandler();
        mock.EnqueueResponse(HttpStatusCode.BadRequest, errorBody);
        using var client = CreateClientWithErrorHandler(mock);

        var act = () => client.GetAsync("/api/test");

        var ex = await act.Should().ThrowAsync<NginxApiException>();
        ex.Which.RawResponse.Should().Contain("validation failed");
    }

    [Fact]
    public async Task SendAsync_HandlesNonJsonErrorResponse()
    {
        var mock = new MockHttpMessageHandler();
        mock.EnqueueResponse(HttpStatusCode.BadGateway, "Bad Gateway");
        using var client = CreateClientWithErrorHandler(mock);

        var act = () => client.GetAsync("/api/test");

        var ex = await act.Should().ThrowAsync<NginxApiException>();
        ex.Which.StatusCode.Should().Be(502);
        ex.Which.ErrorDetail.Should().Contain("Bad Gateway");
    }

    [Fact]
    public async Task SendAsync_DoesNotExposeCredentials_InExceptions()
    {
        var mock = new MockHttpMessageHandler();
        mock.EnqueueResponse(HttpStatusCode.Forbidden, "{\"message\":\"Access denied\"}");
        using var client = CreateClientWithErrorHandler(mock);

        var act = () => client.GetAsync("/api/test");

        var ex = await act.Should().ThrowAsync<NginxApiException>();
        ex.Which.Message.Should().NotContain("password");
        ex.Which.Message.Should().NotContain("secret");
        ex.Which.ToString().Should().NotContain("Bearer");
    }
}
