namespace NginxApiClient.Models.Streams;

/// <summary>Response model for a TCP/UDP stream from the NPM API.</summary>
public class StreamResponse
{
    /// <summary>The unique identifier.</summary>
    public int Id { get; set; }
    /// <summary>The incoming port to listen on.</summary>
    public int IncomingPort { get; set; }
    /// <summary>The hostname or IP to forward to.</summary>
    public string ForwardingHost { get; set; } = string.Empty;
    /// <summary>The port to forward to.</summary>
    public int ForwardingPort { get; set; }
    /// <summary>Whether TCP forwarding is enabled.</summary>
    public bool TcpForwarding { get; set; }
    /// <summary>Whether UDP forwarding is enabled.</summary>
    public bool UdpForwarding { get; set; }
    /// <summary>Whether this stream is enabled.</summary>
    public bool Enabled { get; set; }
    /// <summary>Creation timestamp.</summary>
    public DateTime CreatedOn { get; set; }
    /// <summary>Last modification timestamp.</summary>
    public DateTime ModifiedOn { get; set; }
}

/// <summary>Request model for creating a stream.</summary>
public class CreateStreamRequest
{
    /// <summary>The incoming port to listen on. Required.</summary>
    public int IncomingPort { get; set; }
    /// <summary>The hostname or IP to forward to. Required.</summary>
    public string ForwardingHost { get; set; } = string.Empty;
    /// <summary>The port to forward to. Required.</summary>
    public int ForwardingPort { get; set; }
    /// <summary>Whether to enable TCP forwarding. Default: true.</summary>
    public bool TcpForwarding { get; set; } = true;
    /// <summary>Whether to enable UDP forwarding. Default: false.</summary>
    public bool UdpForwarding { get; set; }
}

/// <summary>Request model for updating a stream.</summary>
public class UpdateStreamRequest
{
    /// <summary>The incoming port.</summary>
    public int? IncomingPort { get; set; }
    /// <summary>The forwarding host.</summary>
    public string? ForwardingHost { get; set; }
    /// <summary>The forwarding port.</summary>
    public int? ForwardingPort { get; set; }
    /// <summary>Whether TCP forwarding is enabled.</summary>
    public bool? TcpForwarding { get; set; }
    /// <summary>Whether UDP forwarding is enabled.</summary>
    public bool? UdpForwarding { get; set; }
}
