# Story 5.2: Manual Client Construction

Status: review

## Story

As a developer,
I want to create a client instance manually without DI,
so that I can use NginxApiClient in console apps, scripts, and non-ASP.NET scenarios.

## Acceptance Criteria

1. `new NginxProxyManagerClient(httpClient, serializer, options)` creates a functional client
2. The DelegatingHandler pipeline is configured correctly
3. Consumer controls `HttpClient` lifetime

## Tasks / Subtasks

- [x] Task 1: Add static factory method or public constructor for manual creation (AC: #1, #2)
  - [x] Factory method or builder that wires up DelegatingHandler pipeline
  - [x] Accept `HttpClient` (or `HttpMessageHandler`), `IJsonSerializer`, `NginxProxyManagerClientOptions`
  - [x] Ensure auth and error handlers are in the pipeline
- [x] Task 2: Write manual construction tests (AC: #1, #2, #3)
  - [x] Test client creation without DI
  - [x] Test that API calls work through manually constructed client
  - [x] Test that consumer can dispose their own HttpClient

## Dev Notes

- **Challenge:** DelegatingHandlers are normally wired up by `IHttpClientFactory`. For manual construction, need to build the handler chain manually: `new HttpClient(new AuthHandler(new ErrorHandler(new HttpClientHandler())))`
- **Architecture reference:** [Source: architecture.md#HTTP Client Management — Manual construction]

### References

- [Source: architecture.md#HTTP Client Management]
- [Source: prd.md#FR41]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- Static factory method added to `NginxProxyManagerClient` for DI-free construction; handler chain built manually as `AuthHandler(ErrorHandler(HttpClientHandler))`; tests confirm API calls succeed and consumer retains full control over `HttpClient` lifetime.

### File List

### Change Log

| Date | Change | Author |
|------|--------|--------|
| 2026-04-17 | Story completed; status moved to review | Claude Opus 4.6 (1M context) |

