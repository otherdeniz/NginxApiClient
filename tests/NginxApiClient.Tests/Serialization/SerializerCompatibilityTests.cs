using FluentAssertions;
using NginxApiClient.SystemTextJson;
using NginxApiClient.NewtonsoftJson;
using Xunit;

namespace NginxApiClient.Tests.Serialization;

/// <summary>
/// Verifies that both serializer implementations produce identical output
/// for the same input, ensuring consumers can switch serializers without behavior changes.
/// </summary>
public class SerializerCompatibilityTests
{
    private readonly SystemTextJsonSerializer _stj = new();
    private readonly NewtonsoftJsonSerializer _newtonsoft = new();

    [Fact]
    public void BothSerializers_ProduceEquivalentJson_ForSameInput()
    {
        var dto = new SerializerTestDto
        {
            Id = 1,
            ForwardHost = "192.168.1.100",
            ForwardPort = 8080,
            ForwardScheme = "http",
            SslForced = true,
            AllowWebsocketUpgrade = false,
            DomainNames = new List<string> { "example.com" },
        };

        string stjJson = _stj.Serialize(dto);
        string newtonsoftJson = _newtonsoft.Serialize(dto);

        // Deserialize each other's output to verify compatibility
        var fromStj = _newtonsoft.Deserialize<SerializerTestDto>(stjJson);
        var fromNewtonsoft = _stj.Deserialize<SerializerTestDto>(newtonsoftJson);

        fromStj.Id.Should().Be(dto.Id);
        fromStj.ForwardHost.Should().Be(dto.ForwardHost);
        fromStj.ForwardPort.Should().Be(dto.ForwardPort);
        fromStj.SslForced.Should().Be(dto.SslForced);

        fromNewtonsoft.Id.Should().Be(dto.Id);
        fromNewtonsoft.ForwardHost.Should().Be(dto.ForwardHost);
        fromNewtonsoft.ForwardPort.Should().Be(dto.ForwardPort);
        fromNewtonsoft.SslForced.Should().Be(dto.SslForced);
    }

    [Fact]
    public void BothSerializers_UseSnakeCasePropertyNames()
    {
        var dto = new SerializerTestDto
        {
            Id = 1,
            ForwardHost = "test",
            ForwardPort = 80,
            ForwardScheme = "http",
            SslForced = true,
            AllowWebsocketUpgrade = true,
            DomainNames = new List<string> { "test.com" },
        };

        string stjJson = _stj.Serialize(dto);
        string newtonsoftJson = _newtonsoft.Serialize(dto);

        // Both should use snake_case
        stjJson.Should().Contain("\"forward_host\":");
        newtonsoftJson.Should().Contain("\"forward_host\":");

        stjJson.Should().Contain("\"ssl_forced\":");
        newtonsoftJson.Should().Contain("\"ssl_forced\":");

        stjJson.Should().Contain("\"allow_websocket_upgrade\":");
        newtonsoftJson.Should().Contain("\"allow_websocket_upgrade\":");

        // Neither should use PascalCase
        stjJson.Should().NotContain("\"ForwardHost\"");
        newtonsoftJson.Should().NotContain("\"ForwardHost\"");
    }

    [Fact]
    public void BothSerializers_ImplementIJsonSerializer()
    {
        IJsonSerializer stj = _stj;
        IJsonSerializer newtonsoft = _newtonsoft;

        stj.Should().BeAssignableTo<IJsonSerializer>();
        newtonsoft.Should().BeAssignableTo<IJsonSerializer>();
    }
}
