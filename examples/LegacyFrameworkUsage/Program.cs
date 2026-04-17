using NginxApiClient;
using NginxApiClient.Exceptions;
using NginxApiClient.Models.ProxyHosts;
using NginxApiClient.NewtonsoftJson;

// ============================================================
// NginxApiClient - Legacy .NET Framework Usage Example
// ============================================================
// This example demonstrates using NginxApiClient with
// Newtonsoft.Json serialization, which is the recommended
// approach for .NET Framework 4.8 projects.
//
// In a real .NET Framework 4.8 project:
//   Install-Package NginxApiClient
//   Install-Package NginxApiClient.NewtonsoftJson
// ============================================================

// The only difference from the SystemTextJson example is the serializer
var options = new NginxProxyManagerClientOptions
{
    BaseUrl = "http://localhost:81",
    Credentials = new NginxCredentials("admin@example.com", "changeme"),
};

// Use NewtonsoftJsonSerializer instead of SystemTextJsonSerializer
var serializer = new NewtonsoftJsonSerializer();
var client = NginxProxyManagerClientFactory.Create(options, serializer);

try
{
    // List all proxy hosts — same API regardless of serializer choice
    Console.WriteLine("=== Listing Proxy Hosts (Newtonsoft.Json) ===");
    var hosts = await client.ProxyHosts.ListAsync();
    Console.WriteLine($"Found {hosts.Count} proxy host(s)");

    foreach (var host in hosts)
    {
        Console.WriteLine($"  [{host.Id}] {string.Join(", ", host.DomainNames)} -> {host.ForwardHost}:{host.ForwardPort}");
    }

    // Create a proxy host — works identically with either serializer
    Console.WriteLine("\n=== Creating Proxy Host ===");
    var newHost = await client.ProxyHosts.CreateAsync(new CreateProxyHostRequest
    {
        DomainNames = new List<string> { "legacy-app.example.com" },
        ForwardScheme = "http",
        ForwardHost = "10.0.0.50",
        ForwardPort = 5000,
    });
    Console.WriteLine($"Created proxy host {newHost.Id}");

    // Clean up
    await client.ProxyHosts.DeleteAsync(newHost.Id);
    Console.WriteLine($"Deleted proxy host {newHost.Id}");
}
catch (NginxApiException ex)
{
    Console.WriteLine($"NPM API error {ex.StatusCode}: {ex.ErrorDetail}");
}
