# Story 3.4: Proxy Host Unit Tests

Status: ready-for-dev

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

- [ ] Task 1: Ensure MockHttpMessageHandler helper exists
  - [ ] Create or verify `tests/NginxApiClient.Tests/Helpers/MockHttpMessageHandler.cs`
  - [ ] Support configurable response status, body, headers
  - [ ] Capture request for assertion (method, URL, body)
- [ ] Task 2: Write comprehensive ProxyHostClient tests (AC: #1-#8)
  - [ ] `ListAsync_ReturnsProxyHosts_WhenHostsExist` — verify deserialization of JSON array
  - [ ] `ListAsync_ReturnsEmptyList_WhenNoHosts`
  - [ ] `GetAsync_ReturnsProxyHost_WhenIdExists` — verify all properties deserialized
  - [ ] `GetAsync_ThrowsNotFoundException_WhenIdNotFound` — verify 404 mapping
  - [ ] `CreateAsync_ReturnsCreatedHost_WithValidRequest` — verify all request properties serialized
  - [ ] `CreateAsync_ThrowsNginxApiException_WhenRequestInvalid` — verify 422 mapping
  - [ ] `CreateAsync_ThrowsArgumentNullException_WhenRequestNull`
  - [ ] `UpdateAsync_UpdatesHost_WithValidRequest`
  - [ ] `DeleteAsync_Succeeds_WhenIdExists`
  - [ ] `EnableAsync_Succeeds_WhenIdExists`
  - [ ] `DisableAsync_Succeeds_WhenIdExists`
- [ ] Task 3: Write serialization verification tests (AC: #8)
  - [ ] Verify request body uses `snake_case` JSON
  - [ ] Verify response deserialization handles `snake_case` → `PascalCase`
  - [ ] Test with both SystemTextJson and NewtonsoftJson serializers

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

### Debug Log References

### Completion Notes List

### File List

