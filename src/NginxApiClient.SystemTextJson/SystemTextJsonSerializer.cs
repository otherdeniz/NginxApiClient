using System.Text.Json;

namespace NginxApiClient.SystemTextJson;

/// <summary>
/// JSON serializer implementation using <c>System.Text.Json</c>.
/// Configures <c>snake_case</c> property naming to match the NGINX Proxy Manager API.
/// This class is thread-safe.
/// </summary>
public sealed class SystemTextJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _options;

    /// <summary>
    /// Initializes a new instance of <see cref="SystemTextJsonSerializer"/> with default options
    /// configured for the NPM API (<c>snake_case</c> naming, case-insensitive deserialization).
    /// </summary>
    public SystemTextJsonSerializer()
        : this(CreateDefaultOptions())
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SystemTextJsonSerializer"/> with custom options.
    /// </summary>
    /// <param name="options">Custom <see cref="JsonSerializerOptions"/> to use for serialization.</param>
    public SystemTextJsonSerializer(JsonSerializerOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public T Deserialize<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default!;
        }

        return JsonSerializer.Deserialize<T>(json, _options)!;
    }

    /// <inheritdoc />
    public string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, _options);
    }

    private static JsonSerializerOptions CreateDefaultOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        };
    }
}
