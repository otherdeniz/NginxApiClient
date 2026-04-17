namespace NginxApiClient;

/// <summary>
/// Abstraction for JSON serialization and deserialization.
/// Implementation packages (<c>NginxApiClient.SystemTextJson</c> and <c>NginxApiClient.NewtonsoftJson</c>)
/// provide concrete implementations with <c>snake_case</c> JSON property naming to match the NPM API.
/// </summary>
public interface IJsonSerializer
{
    /// <summary>
    /// Deserializes a JSON string into an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>The deserialized object, or <c>default(T)</c> if the input is null or empty.</returns>
    T Deserialize<T>(string json);

    /// <summary>
    /// Serializes an object to a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <returns>A JSON string representation of the object.</returns>
    string Serialize<T>(T value);
}
