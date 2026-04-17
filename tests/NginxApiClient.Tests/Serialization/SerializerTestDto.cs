namespace NginxApiClient.Tests.Serialization;

/// <summary>
/// Test DTO that mimics NPM API model properties for serializer testing.
/// </summary>
public class SerializerTestDto
{
    public int Id { get; set; }
    public string ForwardHost { get; set; } = string.Empty;
    public int ForwardPort { get; set; }
    public string ForwardScheme { get; set; } = string.Empty;
    public bool SslForced { get; set; }
    public bool AllowWebsocketUpgrade { get; set; }
    public List<string> DomainNames { get; set; } = new();
    public string? AdvancedConfig { get; set; }
}
