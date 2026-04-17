# NginxApiClient

A comprehensive C# client library for the [NGINX Proxy Manager](https://nginxproxymanager.com/) REST API.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Features

- **Full API coverage** — all 11 NPM resource groups (proxy hosts, certificates, redirection hosts, dead hosts, streams, access lists, users, settings, audit log, reports)
- **Multi-target** — supports .NET Framework 4.8 through .NET 9 via sub-package architecture
- **Async-first** — all methods are async with `CancellationToken` support
- **Transparent authentication** — JWT token lifecycle managed automatically
- **Strongly-typed** — typed request/response models with IntelliSense
- **Testable** — `INginxProxyManagerClient` interface for easy mocking
- **DI-ready** — `IServiceCollection` extension methods for ASP.NET Core

## Installation

### Modern .NET (.NET 8 / .NET 9)

```bash
dotnet add package NginxApiClient
dotnet add package NginxApiClient.SystemTextJson
```

### .NET Framework 4.8

```powershell
Install-Package NginxApiClient
Install-Package NginxApiClient.NewtonsoftJson
```

## Quick Start

```csharp
using NginxApiClient;
using NginxApiClient.SystemTextJson;

// Create client
var serializer = new SystemTextJsonSerializer();
var options = new NginxProxyManagerClientOptions
{
    BaseUrl = "http://localhost:81",
    Credentials = new NginxCredentials("admin@example.com", "changeme"),
};

// Authentication is handled automatically
var client = NginxProxyManagerClientFactory.Create(options, serializer);

// List all proxy hosts
var hosts = await client.ProxyHosts.ListAsync();
foreach (var host in hosts)
{
    Console.WriteLine($"{host.Id}: {string.Join(", ", host.DomainNames)} -> {host.ForwardHost}:{host.ForwardPort}");
}
```

## ASP.NET Core DI Registration

```csharp
// In Program.cs or Startup.cs
services.AddNginxApiClient(options =>
{
    options.BaseUrl = "http://npm:81";
    options.Credentials = new NginxCredentials("admin@example.com", "changeme");
});

// Inject into your services
public class MyService
{
    private readonly INginxProxyManagerClient _npmClient;

    public MyService(INginxProxyManagerClient npmClient)
    {
        _npmClient = npmClient;
    }

    public async Task CreateProxyHostAsync(string domain, string backendHost, int backendPort)
    {
        await _npmClient.ProxyHosts.CreateAsync(new CreateProxyHostRequest
        {
            DomainNames = new List<string> { domain },
            ForwardScheme = "http",
            ForwardHost = backendHost,
            ForwardPort = backendPort,
            SslForced = true,
            Http2Support = true,
        });
    }
}
```

## Create a Proxy Host with SSL

```csharp
var proxyHost = await client.ProxyHosts.CreateAsync(new CreateProxyHostRequest
{
    DomainNames = new List<string> { "app.example.com" },
    ForwardScheme = "http",
    ForwardHost = "192.168.1.100",
    ForwardPort = 8080,
    SslForced = true,
    HstsEnabled = true,
    Http2Support = true,
    BlockExploits = true,
    AllowWebsocketUpgrade = true,
    CertificateId = 5,  // ID of an existing certificate
});

Console.WriteLine($"Created proxy host {proxyHost.Id} for {string.Join(", ", proxyHost.DomainNames)}");
```

## Certificate Management

```csharp
// List certificates and check expiry
var certs = await client.Certificates.ListAsync();
foreach (var cert in certs)
{
    var daysUntilExpiry = (cert.ExpiresOn - DateTime.UtcNow).Days;
    Console.WriteLine($"{cert.NiceName}: expires in {daysUntilExpiry} days");

    if (daysUntilExpiry < 30)
    {
        await client.Certificates.RenewAsync(cert.Id);
        Console.WriteLine($"  -> Renewed!");
    }
}

// Provision a new Let's Encrypt certificate
var newCert = await client.Certificates.CreateAsync(new CreateCertificateRequest
{
    DomainNames = new List<string> { "new.example.com" },
    Provider = "letsencrypt",
    Meta = new CertificateMeta { LetsencryptEmail = "admin@example.com", LetsencryptAgree = 1 },
});
```

## Error Handling

```csharp
using NginxApiClient.Exceptions;

try
{
    var host = await client.ProxyHosts.GetAsync(999);
}
catch (NginxNotFoundException ex)
{
    Console.WriteLine($"Proxy host not found: {ex.ErrorDetail}");
}
catch (NginxAuthenticationException ex)
{
    Console.WriteLine($"Authentication failed: {ex.ErrorDetail}");
}
catch (NginxApiException ex)
{
    Console.WriteLine($"API error {ex.StatusCode}: {ex.ErrorDetail}");
}
```

## Available Resource Clients

| Property | Interface | Description |
|----------|-----------|-------------|
| `client.ProxyHosts` | `IProxyHostClient` | Manage reverse proxy hosts |
| `client.Certificates` | `ICertificateClient` | Manage SSL certificates and Let's Encrypt |
| `client.RedirectionHosts` | `IRedirectionHostClient` | Manage URL redirections |
| `client.DeadHosts` | `IDeadHostClient` | Manage custom 404 pages |
| `client.Streams` | `IStreamClient` | Manage TCP/UDP stream forwarding |
| `client.AccessLists` | `IAccessListClient` | Manage IP and auth-based access restrictions |
| `client.Users` | `IUserClient` | Manage NPM users and permissions |
| `client.Settings` | `ISettingsClient` | Get/update NPM settings |
| `client.AuditLog` | `IAuditLogClient` | Read audit log entries |
| `client.Reports` | `IReportsClient` | Access host statistics |

## Package Architecture

| Package | Target | Purpose |
|---------|--------|---------|
| `NginxApiClient` | .NET Standard 2.0 | Core interfaces, models, exceptions |
| `NginxApiClient.SystemTextJson` | .NET 8, .NET 9 | System.Text.Json serialization + DI |
| `NginxApiClient.NewtonsoftJson` | .NET Standard 2.0 | Newtonsoft.Json serialization + DI |

## Examples

See the [`examples/`](examples/) folder for runnable console projects:

- **[BasicUsage](examples/BasicUsage/)** — Authenticate, list/create/delete proxy hosts
- **[CertificateManagement](examples/CertificateManagement/)** — Certificate lifecycle automation
- **[LegacyFrameworkUsage](examples/LegacyFrameworkUsage/)** — .NET Framework 4.8 example

## License

MIT License - see [LICENSE](LICENSE) for details.
