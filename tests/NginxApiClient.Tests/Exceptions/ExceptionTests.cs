using FluentAssertions;
using NginxApiClient.Exceptions;
using Xunit;

namespace NginxApiClient.Tests.Exceptions;

public class ExceptionTests
{
    [Fact]
    public void NginxApiException_SetsProperties_FromStatusCodeConstructor()
    {
        var ex = new NginxApiException(422, "domain_names is required", "{\"error\":\"domain_names is required\"}");

        ex.StatusCode.Should().Be(422);
        ex.ErrorDetail.Should().Be("domain_names is required");
        ex.RawResponse.Should().Contain("domain_names is required");
        ex.Message.Should().Contain("422");
        ex.Message.Should().Contain("domain_names is required");
    }

    [Fact]
    public void NginxApiException_SetsProperties_FromMessageConstructor()
    {
        var ex = new NginxApiException("Something went wrong");

        ex.StatusCode.Should().Be(0);
        ex.ErrorDetail.Should().BeEmpty();
        ex.RawResponse.Should().BeEmpty();
        ex.Message.Should().Be("Something went wrong");
    }

    [Fact]
    public void NginxApiException_PreservesInnerException()
    {
        var inner = new InvalidOperationException("connection refused");
        var ex = new NginxApiException("Network error", inner);

        ex.InnerException.Should().BeSameAs(inner);
        ex.Message.Should().Be("Network error");
    }

    [Fact]
    public void NginxAuthenticationException_HasStatusCode401()
    {
        var ex = new NginxAuthenticationException("Invalid credentials");

        ex.StatusCode.Should().Be(401);
        ex.ErrorDetail.Should().Be("Invalid credentials");
    }

    [Fact]
    public void NginxAuthenticationException_IsCatchableAsNginxApiException()
    {
        NginxApiException ex = new NginxAuthenticationException("auth failed");

        ex.Should().BeOfType<NginxAuthenticationException>();
        ex.StatusCode.Should().Be(401);
    }

    [Fact]
    public void NginxNotFoundException_HasStatusCode404()
    {
        var ex = new NginxNotFoundException("Resource not found");

        ex.StatusCode.Should().Be(404);
        ex.ErrorDetail.Should().Be("Resource not found");
    }

    [Fact]
    public void NginxNotFoundException_IsCatchableAsNginxApiException()
    {
        NginxApiException ex = new NginxNotFoundException("not found");

        ex.Should().BeOfType<NginxNotFoundException>();
        ex.StatusCode.Should().Be(404);
    }

    [Fact]
    public void NginxApiException_ToString_DoesNotContainSensitiveData()
    {
        var ex = new NginxApiException(401, "Unauthorized", "{\"error\":\"bad credentials\"}");

        string output = ex.ToString();

        output.Should().NotContain("password");
        output.Should().NotContain("secret");
        output.Should().NotContain("Bearer");
        output.Should().Contain("401");
        output.Should().Contain("Unauthorized");
    }

    [Fact]
    public void NginxApiException_ToString_IncludesInnerException()
    {
        var inner = new Exception("timeout");
        var ex = new NginxApiException(0, "Connection failed", "", inner);

        string output = ex.ToString();

        output.Should().Contain("Connection failed");
        output.Should().Contain("timeout");
    }

    [Fact]
    public void NginxApiException_HandlesNullErrorDetail()
    {
        var ex = new NginxApiException(500, null!, null!);

        ex.ErrorDetail.Should().BeEmpty();
        ex.RawResponse.Should().BeEmpty();
    }

    [Fact]
    public void AllExceptionTypes_AreCatchableByBaseType()
    {
        var exceptions = new NginxApiException[]
        {
            new NginxApiException("base"),
            new NginxAuthenticationException("auth"),
            new NginxNotFoundException("not found"),
        };

        foreach (var ex in exceptions)
        {
            ex.Should().BeAssignableTo<NginxApiException>();
            ex.Should().BeAssignableTo<Exception>();
        }
    }
}
