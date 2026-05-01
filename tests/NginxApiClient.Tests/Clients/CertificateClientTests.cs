using System.Net;
using FluentAssertions;
using NginxApiClient.Internal;
using NginxApiClient.Models.Certificates;
using NginxApiClient.SystemTextJson;
using NginxApiClient.Tests.Helpers;
using Xunit;

namespace NginxApiClient.Tests.Clients;

public class CertificateClientTests
{
    private readonly SystemTextJsonSerializer _serializer = new();

    private (CertificateClient client, MockHttpMessageHandler mock) CreateClient()
    {
        var mock = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mock) { BaseAddress = new Uri("http://localhost:81") };
        var client = new CertificateClient(httpClient, _serializer);
        return (client, mock);
    }

    private static string SampleCertificateJson(int id = 1) => $$"""
        {
            "id": {{id}},
            "provider": "letsencrypt",
            "nice_name": "Example Cert",
            "domain_names": ["example.com", "www.example.com"],
            "expires_on": "2027-01-15T00:00:00.000Z",
            "created_on": "2026-01-15T00:00:00.000Z",
            "modified_on": "2026-01-15T00:00:00.000Z"
        }
        """;

    [Fact]
    public async Task ListAsync_ReturnsCertificates_WhenCertsExist()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.OK, $"[{SampleCertificateJson(1)}, {SampleCertificateJson(2)}]");

        var result = await client.ListAsync();

        result.Should().HaveCount(2);
        result[0].Id.Should().Be(1);
        result[0].Provider.Should().Be("letsencrypt");
        result[0].NiceName.Should().Be("Example Cert");
        result[0].DomainNames.Should().Contain("example.com");
        mock.SentRequests[0].Method.Should().Be(HttpMethod.Get);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/certificates");
    }

    [Fact]
    public async Task GetAsync_ReturnsCertificate_WhenIdExists()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.OK, SampleCertificateJson(42));

        var result = await client.GetAsync(42);

        result.Id.Should().Be(42);
        result.Provider.Should().Be("letsencrypt");
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/certificates/42");
    }

    [Fact]
    public async Task CreateAsync_ProvisionsCert_WithLetsEncrypt()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.Created, SampleCertificateJson(10));

        var request = new CreateCertificateRequest
        {
            DomainNames = new List<string> { "new.example.com" },
            Provider = "letsencrypt",
            Meta = new CertificateMeta { LetsencryptEmail = "admin@example.com", LetsencryptAgree = true },
        };

        var result = await client.CreateAsync(request);

        result.Id.Should().Be(10);
        mock.SentRequests[0].Method.Should().Be(HttpMethod.Post);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/certificates");
        mock.SentRequestBodies[0].Should().Contain("\"domain_names\"");
        mock.SentRequestBodies[0].Should().Contain("\"provider\"");
        mock.SentRequestBodies[0].Should().Contain("\"letsencrypt\"");
    }

    [Fact]
    public async Task UploadAsync_UploadsCert_WithValidContent()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.Created, SampleCertificateJson(20));

        var request = new UploadCertificateRequest
        {
            NiceName = "Custom Cert",
            Certificate = "-----BEGIN CERTIFICATE-----\nMIIB...\n-----END CERTIFICATE-----",
            CertificateKey = "-----BEGIN PRIVATE KEY-----\nMIIE...\n-----END PRIVATE KEY-----",
        };

        var result = await client.UploadAsync(request);

        result.Id.Should().Be(20);
        mock.SentRequests[0].Method.Should().Be(HttpMethod.Post);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/certificates/upload");
        mock.SentRequestBodies[0].Should().Contain("\"certificate\"");
        mock.SentRequestBodies[0].Should().Contain("\"certificate_key\"");
    }

    [Fact]
    public async Task DownloadAsync_ReturnsCertContent_WhenIdExists()
    {
        var (client, mock) = CreateClient();
        var certContent = "-----BEGIN CERTIFICATE-----\nMIIB...\n-----END CERTIFICATE-----"u8.ToArray();
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(certContent),
        };
        mock.EnqueueResponse(response);

        var result = await client.DownloadAsync(5);

        result.Should().BeEquivalentTo(certContent);
        mock.SentRequests[0].Method.Should().Be(HttpMethod.Get);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/certificates/5/download");
    }

    [Fact]
    public async Task RenewAsync_RenewsCert_WhenIdExists()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.OK, SampleCertificateJson(3));

        var result = await client.RenewAsync(3);

        result.Id.Should().Be(3);
        mock.SentRequests[0].Method.Should().Be(HttpMethod.Post);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/certificates/3/renew");
    }

    [Fact]
    public async Task DeleteAsync_Succeeds_WhenIdExists()
    {
        var (client, mock) = CreateClient();
        mock.EnqueueResponse(HttpStatusCode.OK);

        await client.DeleteAsync(8);

        mock.SentRequests[0].Method.Should().Be(HttpMethod.Delete);
        mock.SentRequests[0].RequestUri!.AbsolutePath.Should().Be("/api/nginx/certificates/8");
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentNull_WhenRequestNull()
    {
        var (client, _) = CreateClient();

        var act = () => client.CreateAsync(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UploadAsync_ThrowsArgumentNull_WhenRequestNull()
    {
        var (client, _) = CreateClient();

        var act = () => client.UploadAsync(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
