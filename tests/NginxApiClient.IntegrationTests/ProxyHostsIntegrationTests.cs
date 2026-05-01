using NginxApiClient.Models.ProxyHosts;
using NginxApiClient.NewtonsoftJson;
using FluentAssertions;
using Xunit;

namespace NginxApiClient.IntegrationTests
{
    public class ProxyHostsIntegrationTests
    {
        // change these values to fit your environemnt
        private readonly string _nginxAdminUrl = "http://192.168.1.22:81";
        private readonly string _nginxUser = "test@test.com";
        private readonly string _nginxPassword = "test123456";

        //[Fact(Skip = "Manual Test")]
        [Fact]
        public async Task CreateProxyHost_Success()
        {
            // change these values to fit your environemnt
            var createRequest = new CreateProxyHostRequest
            {
                ForwardHost = "192.168.1.11",
                ForwardPort = 80,
                ForwardScheme = "http",
                DomainNames = ["aaa.mydomain.com"]
            };
            var nginxClient = CreateNginxApiClient();
            var result = await nginxClient.ProxyHosts.CreateAsync(createRequest);
            result.Should().NotBe(null);
            result.Id.Should().BeGreaterThan(0);

            // clean up
            await nginxClient.ProxyHosts.DeleteAsync(result.Id);
        }

        private INginxProxyManagerClient CreateNginxApiClient()
        {
            return NginxProxyManagerClientFactory.Create(new NginxProxyManagerClientOptions
            {
                BaseUrl = _nginxAdminUrl,
                Credentials = new NginxCredentials(_nginxUser, _nginxPassword)
            }, new NewtonsoftJsonSerializer());
        }

    }
}
