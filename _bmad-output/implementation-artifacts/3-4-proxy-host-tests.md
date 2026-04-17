# Story 3.4: Proxy Host Unit Tests

Status: review

## Story

As a developer,
I want comprehensive unit tests for the proxy host client,
so that I can verify the client behaves correctly without a live NPM instance.

## Acceptance Criteria

1. Tests verify deserialization of list response
2. Tests verify single host retrieval
3. Tests verify 404 handling
4. Tests verify creation with all properties
5. Tests verify 422 error handling
6. Tests verify deletion
7. Tests verify enable/disable operations
8. All tests verify correct HTTP method, URL path, and request body serialization

## Tasks / Subtasks

- [x] Task 1: Ensure MockHttpMessageHandler helper exists
  - [x] Create or verify `tests/NginxApiClient.Tests/Helpers/MockHttpMessageHandler.cs`
  - [x] Support configurable response status, body, headers
  - [x] Capture request for assertion (method, URL, body)
- [x] Task 2: Write comprehensive ProxyHostClient tests (AC: #1-#8)
  - [x] `ListAsync_ReturnsProxyHosts_WhenHostsExist` — verify deserialization of JSON array
  - [x] `ListAsync_ReturnsEmptyList_WhenNoHosts`
  - [x] `GetAsync_ReturnsProxyHost_WhenIdExists` — verify all properties deserialized
  - [x] `GetAsync_ThrowsNotFoundException_WhenIdNotFound` — verify 404 mapping
  - [x] `CreateAsync_ReturnsCreatedHost_WithValidRequest` — verify all request properties serialized
  - [x] `CreateAsync_ThrowsNginxApiException_WhenRequestInvalid` — verify 422 mapping
  - [x] `CreateAsync_ThrowsArgumentNullException_WhenRequestNull`
  - [x] `UpdateAsync_UpdatesHost_WithValidRequest`
  - [x] `DeleteAsync_Succeeds_WhenIdExists`
  - [x] `EnableAsync_Succeeds_WhenIdExists`
  - [x] `DisableAsync_Succeeds_WhenIdExists`
- [x] Task 3: Write serialization verification tests (AC: #8)
  - [x] Verify request body uses `snake_case` JSON
  - [x] Verify response deserialization handles `snake_case` → `PascalCase`
  - [x] Test with both SystemTextJson and NewtonsoftJson serializers

## Dev Notes

- **Test naming convention:** `{MethodName}_{Scenario}_{ExpectedBehavior}`
- **MockHttpMessageHandler:** Configure to return preset responses; capture sent requests for assertions
- **Error mapping:** Tests should verify that DelegatingHandler pipeline maps errors correctly — test through the full client+handler chain if possible, or test handlers separately
- **Both serializers:** Run key tests with both serializer implementations to catch mapping differences

### References

- [Source: architecture.md#Test Naming Convention]
- [Source: architecture.md#Process Patterns]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- `MockHttpMessageHandler` helper created and verified. All 11 proxy host client test cases implemented covering list, get, 404 handling, create, 422 handling, update, delete, enable, and disable. Serialization tests run against both SystemTextJson and NewtonsoftJson to confirm `snake_case` mapping consistency.

### Change Log

- 2026-04-17: Story completed and status set to review. All tasks finished and acceptance criteria met.

### File List

