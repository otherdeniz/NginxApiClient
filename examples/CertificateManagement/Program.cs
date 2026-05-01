using NginxApiClient;
using NginxApiClient.Exceptions;
using NginxApiClient.Models.Certificates;
using NginxApiClient.SystemTextJson;

// ============================================================
// NginxApiClient - Certificate Management Example
// ============================================================
// This example demonstrates:
//   1. Listing all certificates
//   2. Checking certificate expiry dates
//   3. Renewing certificates nearing expiry
//   4. Provisioning a new Let's Encrypt certificate
// ============================================================

var options = new NginxProxyManagerClientOptions
{
    BaseUrl = "http://localhost:81",
    Credentials = new NginxCredentials("admin@example.com", "changeme"),
};

var client = NginxProxyManagerClientFactory.Create(options, new SystemTextJsonSerializer());

try
{
    // List all certificates and check expiry
    Console.WriteLine("=== Certificate Expiry Check ===");
    var certs = await client.Certificates.ListAsync();
    Console.WriteLine($"Found {certs.Count} certificate(s)\n");

    foreach (var cert in certs)
    {
        var daysUntilExpiry = (cert.ExpiresOn - DateTime.UtcNow).Days;
        var status = daysUntilExpiry switch
        {
            < 0 => "EXPIRED",
            < 7 => "CRITICAL",
            < 30 => "WARNING",
            _ => "OK",
        };

        Console.WriteLine($"  [{cert.Id}] {cert.NiceName}");
        Console.WriteLine($"       Domains: {string.Join(", ", cert.DomainNames)}");
        Console.WriteLine($"       Provider: {cert.Provider}");
        Console.WriteLine($"       Expires: {cert.ExpiresOn:yyyy-MM-dd} ({daysUntilExpiry} days) [{status}]");

        // Auto-renew certificates expiring within 30 days
        if (daysUntilExpiry < 30 && daysUntilExpiry >= 0)
        {
            Console.WriteLine($"       -> Renewing...");
            try
            {
                await client.Certificates.RenewAsync(cert.Id);
                Console.WriteLine($"       -> Renewed successfully!");
            }
            catch (NginxApiException ex)
            {
                Console.WriteLine($"       -> Renewal failed: {ex.ErrorDetail}");
            }
        }

        Console.WriteLine();
    }

    // Provision a new Let's Encrypt certificate
    Console.WriteLine("=== Provision New Let's Encrypt Certificate ===");
    var newCert = await client.Certificates.CreateAsync(new CreateCertificateRequest
    {
        DomainNames = new List<string> { "new-service.example.com" },
        Provider = "letsencrypt",
        Meta = new CertificateMeta
        {
            LetsencryptEmail = "admin@example.com",
            LetsencryptAgree = true,
        },
    });
    Console.WriteLine($"Provisioned certificate {newCert.Id} for {string.Join(", ", newCert.DomainNames)}");
}
catch (NginxAuthenticationException ex)
{
    Console.WriteLine($"Authentication failed: {ex.ErrorDetail}");
}
catch (NginxApiException ex)
{
    Console.WriteLine($"NPM API error {ex.StatusCode}: {ex.ErrorDetail}");
}
