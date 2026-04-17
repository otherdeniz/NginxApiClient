using NginxApiClient;
using NginxApiClient.Exceptions;
using NginxApiClient.Models.ProxyHosts;
using NginxApiClient.SystemTextJson;

// ============================================================
// NginxApiClient - Basic Usage Example
// ============================================================
// This example demonstrates:
//   1. Creating a client with manual construction (no DI)
//   2. Listing existing proxy hosts
//   3. Creating a new proxy host
//   4. Updating a proxy host
//   5. Deleting a proxy host
//   6. Error handling
// ============================================================

// Configure the NPM connection
var options = new NginxProxyManagerClientOptions
{
    BaseUrl = "http://localhost:81",  // Your NPM instance URL
    Credentials = new NginxCredentials("admin@example.com", "changeme"),
};

// Create the client — authentication is handled automatically
var serializer = new SystemTextJsonSerializer();
var client = NginxProxyManagerClientFactory.Create(options, serializer);

try
{
    // List all proxy hosts
    Console.WriteLine("=== Listing Proxy Hosts ===");
    var hosts = await client.ProxyHosts.ListAsync();
    Console.WriteLine($"Found {hosts.Count} proxy host(s)");
    foreach (var host in hosts)
    {
        Console.WriteLine($"  [{host.Id}] {string.Join(", ", host.DomainNames)} -> {host.ForwardScheme}://{host.ForwardHost}:{host.ForwardPort}");
    }

    // Create a new proxy host
    Console.WriteLine("\n=== Creating Proxy Host ===");
    var newHost = await client.ProxyHosts.CreateAsync(new CreateProxyHostRequest
    {
        DomainNames = new List<string> { "myapp.example.com" },
        ForwardScheme = "http",
        ForwardHost = "192.168.1.100",
        ForwardPort = 8080,
        BlockExploits = true,
        AllowWebsocketUpgrade = true,
    });
    Console.WriteLine($"Created proxy host {newHost.Id} for {string.Join(", ", newHost.DomainNames)}");

    // Update the proxy host — enable SSL
    Console.WriteLine("\n=== Updating Proxy Host (enable SSL) ===");
    var updated = await client.ProxyHosts.UpdateAsync(newHost.Id, new UpdateProxyHostRequest
    {
        SslForced = true,
        HstsEnabled = true,
        Http2Support = true,
    });
    Console.WriteLine($"Updated proxy host {updated.Id}: SSL forced={updated.SslForced}");

    // Disable and re-enable
    Console.WriteLine("\n=== Disable/Enable ===");
    await client.ProxyHosts.DisableAsync(newHost.Id);
    Console.WriteLine($"Disabled proxy host {newHost.Id}");

    await client.ProxyHosts.EnableAsync(newHost.Id);
    Console.WriteLine($"Re-enabled proxy host {newHost.Id}");

    // Delete the proxy host
    Console.WriteLine("\n=== Deleting Proxy Host ===");
    await client.ProxyHosts.DeleteAsync(newHost.Id);
    Console.WriteLine($"Deleted proxy host {newHost.Id}");
}
catch (NginxAuthenticationException ex)
{
    Console.WriteLine($"Authentication failed: {ex.ErrorDetail}");
    Console.WriteLine("Check your NPM URL and credentials.");
}
catch (NginxApiException ex)
{
    Console.WriteLine($"NPM API error {ex.StatusCode}: {ex.ErrorDetail}");
}
