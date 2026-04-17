using FluentAssertions;
using NginxApiClient.SystemTextJson;
using Xunit;

namespace NginxApiClient.Tests.Serialization;

public class SystemTextJsonSerializerTests
{
    private readonly SystemTextJsonSerializer _serializer = new();

    [Fact]
    public void Serialize_ProducesSnakeCaseJson()
    {
        var dto = new SerializerTestDto
        {
            Id = 1,
            ForwardHost = "192.168.1.100",
            ForwardPort = 8080,
            ForwardScheme = "http",
            SslForced = true,
            AllowWebsocketUpgrade = false,
            DomainNames = new List<string> { "example.com", "www.example.com" },
        };

        string json = _serializer.Serialize(dto);

        json.Should().Contain("\"forward_host\":");
        json.Should().Contain("\"forward_port\":");
        json.Should().Contain("\"forward_scheme\":");
        json.Should().Contain("\"ssl_forced\":");
        json.Should().Contain("\"allow_websocket_upgrade\":");
        json.Should().Contain("\"domain_names\":");
        json.Should().NotContain("\"ForwardHost\"");
        json.Should().NotContain("\"ForwardPort\"");
    }

    [Fact]
    public void Deserialize_HandlesSnakeCaseJson()
    {
        string json = """
            {
                "id": 42,
                "forward_host": "10.0.0.1",
                "forward_port": 3000,
                "forward_scheme": "https",
                "ssl_forced": true,
                "allow_websocket_upgrade": true,
                "domain_names": ["test.com"]
            }
            """;

        var dto = _serializer.Deserialize<SerializerTestDto>(json);

        dto.Id.Should().Be(42);
        dto.ForwardHost.Should().Be("10.0.0.1");
        dto.ForwardPort.Should().Be(3000);
        dto.ForwardScheme.Should().Be("https");
        dto.SslForced.Should().BeTrue();
        dto.AllowWebsocketUpgrade.Should().BeTrue();
        dto.DomainNames.Should().ContainSingle("test.com");
    }

    [Fact]
    public void RoundTrip_PreservesAllProperties()
    {
        var original = new SerializerTestDto
        {
            Id = 7,
            ForwardHost = "backend.local",
            ForwardPort = 9090,
            ForwardScheme = "http",
            SslForced = false,
            AllowWebsocketUpgrade = true,
            DomainNames = new List<string> { "a.com", "b.com" },
            AdvancedConfig = "proxy_set_header X-Real-IP $remote_addr;",
        };

        string json = _serializer.Serialize(original);
        var deserialized = _serializer.Deserialize<SerializerTestDto>(json);

        deserialized.Id.Should().Be(original.Id);
        deserialized.ForwardHost.Should().Be(original.ForwardHost);
        deserialized.ForwardPort.Should().Be(original.ForwardPort);
        deserialized.ForwardScheme.Should().Be(original.ForwardScheme);
        deserialized.SslForced.Should().Be(original.SslForced);
        deserialized.AllowWebsocketUpgrade.Should().Be(original.AllowWebsocketUpgrade);
        deserialized.DomainNames.Should().BeEquivalentTo(original.DomainNames);
        deserialized.AdvancedConfig.Should().Be(original.AdvancedConfig);
    }

    [Fact]
    public void Deserialize_ReturnsDefault_WhenJsonIsNull()
    {
        var result = _serializer.Deserialize<SerializerTestDto>(null!);
        result.Should().BeNull();
    }

    [Fact]
    public void Deserialize_ReturnsDefault_WhenJsonIsEmpty()
    {
        var result = _serializer.Deserialize<SerializerTestDto>(string.Empty);
        result.Should().BeNull();
    }

    [Fact]
    public void Serialize_OmitsNullProperties()
    {
        var dto = new SerializerTestDto
        {
            Id = 1,
            ForwardHost = "localhost",
            ForwardPort = 80,
            ForwardScheme = "http",
            AdvancedConfig = null,
        };

        string json = _serializer.Serialize(dto);

        json.Should().NotContain("\"advanced_config\"");
    }
}
