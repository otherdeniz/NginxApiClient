using FluentAssertions;
using Xunit;

namespace NginxApiClient.Tests.Options;

public class NginxProxyManagerClientOptionsTests
{
    [Fact]
    public void Validate_Succeeds_WithValidOptions()
    {
        var options = new NginxProxyManagerClientOptions
        {
            BaseUrl = "http://localhost:81",
            Credentials = new NginxCredentials("admin@example.com", "changeme"),
        };

        var act = () => options.Validate();

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_Throws_WhenBaseUrlIsInvalid(string? baseUrl)
    {
        var options = new NginxProxyManagerClientOptions
        {
            BaseUrl = baseUrl!,
            Credentials = new NginxCredentials("admin@example.com", "changeme"),
        };

        var act = () => options.Validate();

        act.Should().Throw<ArgumentException>().Which.ParamName.Should().Be("BaseUrl");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_Throws_WhenEmailIsInvalid(string? email)
    {
        var options = new NginxProxyManagerClientOptions
        {
            BaseUrl = "http://localhost:81",
            Credentials = new NginxCredentials(email!, "changeme"),
        };

        var act = () => options.Validate();

        act.Should().Throw<ArgumentException>().Which.ParamName.Should().Contain("Email");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_Throws_WhenPasswordIsInvalid(string? password)
    {
        var options = new NginxProxyManagerClientOptions
        {
            BaseUrl = "http://localhost:81",
            Credentials = new NginxCredentials("admin@example.com", password!),
        };

        var act = () => options.Validate();

        act.Should().Throw<ArgumentException>().Which.ParamName.Should().Contain("Password");
    }

    [Fact]
    public void NginxCredentials_ToString_DoesNotExposePassword()
    {
        var creds = new NginxCredentials("admin@example.com", "supersecret");

        string output = creds.ToString();

        output.Should().Contain("admin@example.com");
        output.Should().NotContain("supersecret");
    }

    [Fact]
    public void TokenRequest_FromCredentials_MapsCorrectly()
    {
        var creds = new NginxCredentials("admin@example.com", "mypassword");

        var request = Models.Tokens.TokenRequest.FromCredentials(creds);

        request.Identity.Should().Be("admin@example.com");
        request.Secret.Should().Be("mypassword");
    }
}
