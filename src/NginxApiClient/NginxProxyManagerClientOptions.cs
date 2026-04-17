namespace NginxApiClient;

/// <summary>
/// Configuration options for connecting to an NGINX Proxy Manager instance.
/// </summary>
public class NginxProxyManagerClientOptions
{
    /// <summary>
    /// The base URL of the NPM instance (e.g., <c>http://localhost:81</c>).
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// The credentials used to authenticate with the NPM API.
    /// </summary>
    public NginxCredentials Credentials { get; set; } = new();

    /// <summary>
    /// Validates that the options are correctly configured.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when BaseUrl, Email, or Password is null or empty.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(BaseUrl))
        {
            throw new ArgumentException("BaseUrl must not be null or empty.", nameof(BaseUrl));
        }

        if (string.IsNullOrWhiteSpace(Credentials.Email))
        {
            throw new ArgumentException("Credentials.Email must not be null or empty.", nameof(Credentials.Email));
        }

        if (string.IsNullOrWhiteSpace(Credentials.Password))
        {
            throw new ArgumentException("Credentials.Password must not be null or empty.", nameof(Credentials.Password));
        }
    }
}

/// <summary>
/// Credentials for authenticating with the NGINX Proxy Manager API.
/// </summary>
public class NginxCredentials
{
    /// <summary>
    /// The email address used to log in to NPM.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The password used to log in to NPM.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of <see cref="NginxCredentials"/>.
    /// </summary>
    public NginxCredentials()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="NginxCredentials"/> with the specified email and password.
    /// </summary>
    /// <param name="email">The login email address.</param>
    /// <param name="password">The login password.</param>
    public NginxCredentials(string email, string password)
    {
        Email = email;
        Password = password;
    }

    /// <inheritdoc />
    public override string ToString() => $"NginxCredentials(Email={Email})";
}
