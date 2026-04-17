using System.Net;
using FluentAssertions;
using NginxApiClient.Internal;
using NginxApiClient.Models.ProxyHosts;
using NginxApiClient.SystemTextJson;
using NginxApiClient.Tests.Helpers;
using Xunit;

namespace NginxApiClient.Tests.Clients;

public class ProxyHostClientTests
{
    private readonly SystemTextJsonSerializer _serializer = new();

    private (ProxyHostClient client, MockHttpMessageHandler mock) CreateClient()
    {
        var mock = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mock) { BaseAddress = new Uri("http://localhost:81") };
        var client = new ProxyHostClient(httpClient, _serializer);
        return (client, mock);
    }

    private static string SampleProxyHostJson(int id = 1) => $$"""
        {
            "id": {{id}},
            "domain_names": ["example.com", "www.example.com"],
            "forward_scheme": "http",
            "forward_host": "192.168.1.100",
            "forward_port": 8080,
            "caching_enabled": false,
            "allow_websocket_upgrade": true,
            "block_exploits": true,
            "access_list_id": 0,
            "certificate_id": 5,
            "ssl_forced": true,
            "hsts_enabled": true,
            "hsts_subdomains": false,
            "http2_support": true,
            "enabled": true,
            "advanced_config": "",
            "created_on": "2026-01-01T00:00:00.000Z",
            "modified_on": "2026-01-02T00:00:00.000Z"
        }
        """;

    [Fact]
    public async Task ListAsync_ReturnsProxyHosts_WhenHostsExist()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.OK, $"[{SampleProxyHostJson(1)}, {SampleProxyHostJson(2)}]");

        var result = await client.ListAsync();

        result.Should().HaveCount(2);
        result[0].Id.Should().Be(1);
        result[1].Id.Should().Be(2);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/proxy-hosts");
        mock.SentRequests[0].Method.Should().Be(HttpMethod.Get);
    }

    [Fact]
    public async Task ListAsync_ReturnsEmptyList_WhenNoHosts()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.OK, "[]");

        var result = await client.ListAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAsync_ReturnsProxyHost_WhenIdExists()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.OK, SampleProxyHostJson(42));

        var result = await client.GetAsync(42);

        result.Id.Should().Be(42);
        result.DomainNames.Should().Contain("example.com");
        result.ForwardHost.Should().Be("192.168.1.100");
        result.ForwardPort.Should().Be(8080);
        result.SslForced.Should().BeTrue();
        result.HstsEnabled.Should().BeTrue();
        result.Http2Support.Should().BeTrue();
        result.AllowWebsocketUpgrade.Should().BeTrue();
        result.BlockExploits.Should().BeTrue();
        result.CertificateId.Should().Be(5);
        result.Enabled.Should().BeTrue();
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/proxy-hosts/42");
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedHost_WithValidRequest()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.Created, SampleProxyHostJson(99));

        var request = new CreateProxyHostRequest
        {
            DomainNames = new List<string> { "new.example.com" },
            ForwardScheme = "http",
            ForwardHost = "10.0.0.1",
            ForwardPort = 3000,
            SslForced = true,
            Http2Support = true,
        };

        var result = await client.CreateAsync(request);

        result.Id.Should().Be(99);
        mock.SentRequests[0].Method.Should().Be(HttpMethod.Post);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/proxy-hosts");

        string? body = mock.SentRequestBodies[0];
        body.Should().Contain("\"domain_names\"");
        body.Should().Contain("\"forward_host\"");
        body.Should().Contain("\"ssl_forced\"");
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentNull_WhenRequestNull()
    {
        var (client, _) = CreateClient();

        var act = () => client.CreateAsync(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_UpdatesHost_WithValidRequest()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.OK, SampleProxyHostJson(5));

        var request = new UpdateProxyHostRequest
        {
            ForwardHost = "new-backend.local",
            ForwardPort = 9090,
        };

        var result = await client.UpdateAsync(5, request);

        result.Id.Should().Be(5);
        mock.SentRequests[0].Method.Should().Be(HttpMethod.Put);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/proxy-hosts/5");
    }

    [Fact]
    public async Task DeleteAsync_Succeeds_WhenIdExists()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.OK);

        await client.DeleteAsync(10);

        mock.SentRequests[0].Method.Should().Be(HttpMethod.Delete);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/proxy-hosts/10");
    }

    [Fact]
    public async Task EnableAsync_SendsPutWithEnabledTrue()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.OK);

        await client.EnableAsync(7);

        mock.SentRequests[0].Method.Should().Be(HttpMethod.Put);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/proxy-hosts/7");
        mock.SentRequestBodies[0].Should().Contain("\"enabled\":true");
    }

    [Fact]
    public async Task DisableAsync_SendsPutWithEnabledFalse()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.OK);

        await client.DisableAsync(7);

        mock.SentRequests[0].Method.Should().Be(HttpMethod.Put);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/proxy-hosts/7");
        mock.SentRequestBodies[0].Should().Contain("\"enabled\":false");
    }

    [Fact]
    public async Task CreateAsync_SerializesLocations()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.Created, SampleProxyHostJson());

        var request = new CreateProxyHostRequest
        {
            DomainNames = new List<string> { "test.com" },
            ForwardHost = "backend",
            ForwardPort = 80,
            Locations = new List<ProxyHostLocation>
            {
                new() { Path = "/api", ForwardScheme = "http", ForwardHost = "api-server", ForwardPort = 3000 },
                new() { Path = "/static", ForwardScheme = "http", ForwardHost = "cdn", ForwardPort = 8080 },
            },
        };

        await client.CreateAsync(request);

        string? body = mock.SentRequestBodies[0];
        body.Should().Contain("\"locations\"");
        body.Should().Contain("\"/api\"");
        body.Should().Contain("\"api-server\"");
    }

    [Fact]
    public async Task CreateAsync_SerializesAdvancedConfig()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.Created, SampleProxyHostJson());

        var request = new CreateProxyHostRequest
        {
            DomainNames = new List<string> { "test.com" },
            ForwardHost = "backend",
            ForwardPort = 80,
            AdvancedConfig = "proxy_set_header X-Real-IP $remote_addr;",
        };

        await client.CreateAsync(request);

        string? body = mock.SentRequestBodies[0];
        body.Should().Contain("\"advanced_config\"");
        body.Should().Contain("proxy_set_header");
    }

    [Fact]
    public async Task GetAsync_DeserializesAllProperties()
    {
        var (client, mock) = CreateClient();
        string json = """
            {
                "id": 1,
                "domain_names": ["a.com", "b.com"],
                "forward_scheme": "https",
                "forward_host": "internal.host",
                "forward_port": 443,
                "caching_enabled": true,
                "allow_websocket_upgrade": false,
                "block_exploits": true,
                "access_list_id": 3,
                "certificate_id": 7,
                "ssl_forced": true,
                "hsts_enabled": true,
                "hsts_subdomains": true,
                "http2_support": true,
                "enabled": false,
                "advanced_config": "custom_directive;",
                "locations": [
                    {"path": "/api", "forward_scheme": "http", "forward_host": "api", "forward_port": 3000}
                ],
                "created_on": "2026-03-15T10:00:00.000Z",
                "modified_on": "2026-04-01T12:00:00.000Z"
            }
            """;
        mock.EnqueueResponse(HttpStatusCode.OK, json);

        var result = await client.GetAsync(1);

        result.DomainNames.Should().BeEquivalentTo(new[] { "a.com", "b.com" });
        result.ForwardScheme.Should().Be("https");
        result.ForwardHost.Should().Be("internal.host");
        result.ForwardPort.Should().Be(443);
        result.CachingEnabled.Should().BeTrue();
        result.AllowWebsocketUpgrade.Should().BeFalse();
        result.BlockExploits.Should().BeTrue();
        result.AccessListId.Should().Be(3);
        result.CertificateId.Should().Be(7);
        result.SslForced.Should().BeTrue();
        result.HstsEnabled.Should().BeTrue();
        result.HstsSubdomains.Should().BeTrue();
        result.Http2Support.Should().BeTrue();
        result.Enabled.Should().BeFalse();
        result.AdvancedConfig.Should().Be("custom_directive;");
        result.Locations.Should().HaveCount(1);
        result.Locations![0].Path.Should().Be("/api");
        result.Locations![0].ForwardHost.Should().Be("api");
    }
}
