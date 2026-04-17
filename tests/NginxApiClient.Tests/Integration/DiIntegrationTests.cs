using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NginxApiClient.SystemTextJson;
using Xunit;

namespace NginxApiClient.Tests.Integration;

public class DiIntegrationTests
{
    [Fact]
    public void AddNginxApiClient_RegistersClient_InServiceCollection()
    {
        var services = new ServiceCollection();

        services.AddNginxApiClient(options =>
        {
            options.BaseUrl = "http://localhost:81";
            options.Credentials = new NginxCredentials("admin@example.com", "changeme");
        });

        var provider = services.BuildServiceProvider();
        var clientFactory = provider.GetService<IHttpClientFactory>();

        clientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddNginxApiClient_RegistersSerializer()
    {
        var services = new ServiceCollection();

        services.AddNginxApiClient(options =>
        {
            options.BaseUrl = "http://localhost:81";
            options.Credentials = new NginxCredentials("admin@example.com", "changeme");
        });

        var provider = services.BuildServiceProvider();
        var serializer = provider.GetService<IJsonSerializer>();

        serializer.Should().NotBeNull();
        serializer.Should().BeOfType<SystemTextJsonSerializer>();
    }

    [Fact]
    public void AddNginxApiClient_RegistersOptions()
    {
        var services = new ServiceCollection();

        services.AddNginxApiClient(options =>
        {
            options.BaseUrl = "http://localhost:81";
            options.Credentials = new NginxCredentials("admin@example.com", "changeme");
        });

        var provider = services.BuildServiceProvider();
        var options = provider.GetService<NginxProxyManagerClientOptions>();

        options.Should().NotBeNull();
        options!.BaseUrl.Should().Be("http://localhost:81");
    }

    [Fact]
    public void AddNginxApiClient_ThrowsOnInvalidOptions()
    {
        var services = new ServiceCollection();

        var act = () => services.AddNginxApiClient(options =>
        {
            options.BaseUrl = "";
        });

        act.Should().Throw<ArgumentException>();
    }
}
