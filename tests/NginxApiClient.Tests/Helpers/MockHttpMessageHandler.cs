using System.Net;

namespace NginxApiClient.Tests.Helpers;

/// <summary>
/// Mock HTTP message handler for unit testing. Captures requests and returns configurable responses.
/// </summary>
public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage> _responses = new();
    private readonly List<HttpRequestMessage> _sentRequests = new();
    private readonly List<string?> _sentRequestBodies = new();

    /// <summary>All requests sent through this handler.</summary>
    public IReadOnlyList<HttpRequestMessage> SentRequests => _sentRequests;

    /// <summary>All request bodies captured before disposal.</summary>
    public IReadOnlyList<string?> SentRequestBodies => _sentRequestBodies;

    /// <summary>The last request sent through this handler.</summary>
    public HttpRequestMessage? LastRequest => _sentRequests.Count > 0 ? _sentRequests[^1] : null;

    /// <summary>Enqueues a response to return for the next request.</summary>
    public void EnqueueResponse(HttpStatusCode statusCode, string? content = null)
    {
        var response = new HttpResponseMessage(statusCode);
        if (content != null)
        {
            response.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        }
        _responses.Enqueue(response);
    }

    /// <summary>Enqueues a response message directly.</summary>
    public void EnqueueResponse(HttpResponseMessage response)
    {
        _responses.Enqueue(response);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _sentRequests.Add(request);

        // Capture body before it may be disposed
        string? body = request.Content != null ? await request.Content.ReadAsStringAsync().ConfigureAwait(false) : null;
        _sentRequestBodies.Add(body);

        if (_responses.Count == 0)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        return _responses.Dequeue();
    }
}
