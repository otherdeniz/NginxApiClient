using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NginxApiClient.NewtonsoftJson;

/// <summary>
/// JSON serializer implementation using <c>Newtonsoft.Json</c>.
/// Configures <c>snake_case</c> property naming to match the NGINX Proxy Manager API.
/// This class is thread-safe.
/// </summary>
public sealed class NewtonsoftJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerSettings _settings;

    /// <summary>
    /// Initializes a new instance of <see cref="NewtonsoftJsonSerializer"/> with default settings
    /// configured for the NPM API (<c>snake_case</c> naming, null value handling).
    /// </summary>
    public NewtonsoftJsonSerializer()
        : this(CreateDefaultSettings())
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="NewtonsoftJsonSerializer"/> with custom settings.
    /// </summary>
    /// <param name="settings">Custom <see cref="JsonSerializerSettings"/> to use for serialization.</param>
    public NewtonsoftJsonSerializer(JsonSerializerSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <inheritdoc />
    public T Deserialize<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default!;
        }

        return JsonConvert.DeserializeObject<T>(json, _settings)!;
    }

    /// <inheritdoc />
    public string Serialize<T>(T value)
    {
        return JsonConvert.SerializeObject(value, _settings);
    }

    private static JsonSerializerSettings CreateDefaultSettings()
    {
        return new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(),
            },
            NullValueHandling = NullValueHandling.Ignore,
        };
    }
}
