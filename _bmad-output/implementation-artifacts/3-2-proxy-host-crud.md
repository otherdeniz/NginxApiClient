# Story 3.2: Proxy Host Client — CRUD Operations

Status: review

## Story

As a developer,
I want to list, get, create, update, and delete proxy hosts,
so that I can programmatically manage my NPM proxy configuration.

## Acceptance Criteria

1. `ListAsync()` returns `IReadOnlyList<ProxyHostResponse>` with all proxy hosts
2. `GetAsync(id)` returns the proxy host for that ID; throws `NginxNotFoundException` if not found
3. `CreateAsync(request)` creates a proxy host and returns the response with assigned ID
4. `UpdateAsync(id, request)` updates proxy host configuration
5. `DeleteAsync(id)` removes the proxy host

## Tasks / Subtasks

- [x] Task 1: Create ProxyHostClient internal implementation (AC: #1-#5)
  - [x] Internal class implementing `IProxyHostClient`
  - [x] Constructor takes `HttpClient` and `IJsonSerializer`
  - [x] Implement `ListAsync` — GET `/api/nginx/proxy-hosts`
  - [x] Implement `GetAsync` — GET `/api/nginx/proxy-hosts/{id}`
  - [x] Implement `CreateAsync` — POST `/api/nginx/proxy-hosts`
  - [x] Implement `UpdateAsync` — PUT `/api/nginx/proxy-hosts/{id}`
  - [x] Implement `DeleteAsync` — DELETE `/api/nginx/proxy-hosts/{id}`
  - [x] Validate non-null request parameters with `ArgumentNullException`
  - [x] All methods follow the async method pattern from architecture
- [x] Task 2: Write unit tests (AC: #1-#5)
  - [x] `ListAsync_ReturnsProxyHosts_WhenHostsExist`
  - [x] `GetAsync_ReturnsProxyHost_WhenIdExists`
  - [x] `GetAsync_ThrowsNotFoundException_WhenIdNotFound`
  - [x] `CreateAsync_ReturnsCreatedHost_WithValidRequest`
  - [x] `CreateAsync_ThrowsNginxApiException_WhenRequestInvalid`
  - [x] `UpdateAsync_UpdatesHost_WithValidRequest`
  - [x] `DeleteAsync_Succeeds_WhenIdExists`
  - [x] Verify correct HTTP method, URL, and request body for each operation

## Dev Notes

- **Architecture reference:** [Source: architecture.md#Resource Client Pattern]
- **NPM API endpoints:** `/api/nginx/proxy-hosts` (list/create), `/api/nginx/proxy-hosts/{id}` (get/update/delete)
- **Pattern:** Follow the async method pattern exactly: validate → build request → serialize → send → deserialize → return
- **Error handling** is done by DelegatingHandlers in the pipeline — client code does NOT check status codes
- **Anti-pattern:** Do NOT create a `BaseClient` — each resource client is standalone using composition
- **Testing:** Use `MockHttpMessageHandler` to mock HTTP responses

### Project Structure Notes

- `src/NginxApiClient/Internal/ProxyHostClient.cs`
- `tests/NginxApiClient.Tests/Clients/ProxyHostClientTests.cs`
- `tests/NginxApiClient.Tests/Helpers/MockHttpMessageHandler.cs` (if not already created)

### References

- [Source: architecture.md#Resource Client Pattern]
- [Source: architecture.md#Process Patterns — Async Method Pattern]
- [Source: product-brief-distillate.md#Endpoint Map — Proxy Hosts]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- `ProxyHostClient` implemented as an internal class with all five CRUD operations (List, Get, Create, Update, Delete). Error handling delegated to the pipeline handlers. All unit tests passing with `MockHttpMessageHandler`, verifying HTTP method, URL, and request body for each operation.

### Change Log

- 2026-04-17: Story completed and status set to review. All tasks finished and acceptance criteria met.

### File List

