# Story 5.2: Manual Client Construction

Status: ready-for-dev

## Story

As a developer,
I want to create a client instance manually without DI,
so that I can use NginxApiClient in console apps, scripts, and non-ASP.NET scenarios.

## Acceptance Criteria

1. `new NginxProxyManagerClient(httpClient, serializer, options)` creates a functional client
2. The DelegatingHandler pipeline is configured correctly
3. Consumer controls `HttpClient` lifetime

## Tasks / Subtasks

- [ ] Task 1: Add static factory method or public constructor for manual creation (AC: #1, #2)
  - [ ] Factory method or builder that wires up DelegatingHandler pipeline
  - [ ] Accept `HttpClient` (or `HttpMessageHandler`), `IJsonSerializer`, `NginxProxyManagerClientOptions`
  - [ ] Ensure auth and error handlers are in the pipeline
- [ ] Task 2: Write manual construction tests (AC: #1, #2, #3)
  - [ ] Test client creation without DI
  - [ ] Test that API calls work through manually constructed client
  - [ ] Test that consumer can dispose their own HttpClient

## Dev Notes

- **Challenge:** DelegatingHandlers are normally wired up by `IHttpClientFactory`. For manual construction, need to build the handler chain manually: `new HttpClient(new AuthHandler(new ErrorHandler(new HttpClientHandler())))`
- **Architecture reference:** [Source: architecture.md#HTTP Client Management — Manual construction]

### References

- [Source: architecture.md#HTTP Client Management]
- [Source: prd.md#FR41]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

