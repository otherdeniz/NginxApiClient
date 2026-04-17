# Story 2.4: Error Handling Delegating Handler

Status: review

## Story

As a developer,
I want HTTP errors automatically mapped to typed exceptions,
so that I can catch specific exception types without parsing HTTP responses myself.

## Acceptance Criteria

1. 404 responses throw `NginxNotFoundException` with status code and NPM error detail
2. 4xx (non-401, non-404) and 5xx responses throw `NginxApiException` with `StatusCode`, `ErrorDetail`, and `RawResponse`
3. Network errors (timeout, connection refused) throw `NginxApiException` with original exception as `InnerException`
4. Exception messages describe the issue without exposing credentials

## Tasks / Subtasks

- [x] Task 1: Create ErrorHandlingDelegatingHandler (AC: #1, #2)
  - [x] Extend `DelegatingHandler`
  - [x] Internal class in `NginxApiClient.Internal`
  - [x] Override `SendAsync` to inspect response status codes
  - [x] Parse NPM error detail from response body JSON
  - [x] Map 404 → `NginxNotFoundException`
  - [x] Map other 4xx/5xx → `NginxApiException`
  - [x] Pass through 2xx responses untouched
- [x] Task 2: Implement network error handling (AC: #3, #4)
  - [x] Catch `HttpRequestException`, `TaskCanceledException`
  - [x] Wrap in `NginxApiException` with descriptive message
  - [x] Set original exception as `InnerException`
  - [x] Ensure no credentials leak in error messages
- [x] Task 3: Write error handler tests (AC: #1, #2, #3, #4)
  - [x] Test 404 → `NginxNotFoundException`
  - [x] Test 422 → `NginxApiException` with error detail
  - [x] Test 500 → `NginxApiException`
  - [x] Test network timeout → `NginxApiException` with inner exception
  - [x] Test 200 passes through without exception
  - [x] Test error detail parsed from NPM JSON response body
  - [x] Test credentials not present in any exception message

## Dev Notes

- **Architecture reference:** [Source: architecture.md#Error Handling Architecture]
- **Pipeline order:** AuthenticationDelegatingHandler → ErrorHandlingDelegatingHandler → HttpClientHandler
- **NPM error format:** JSON body with error detail (structure varies — handle gracefully if not parseable)
- **Note:** 401 is handled by AuthenticationDelegatingHandler, not this handler. This handler should NOT intercept 401.
- **NFR8:** Exception messages include NPM error details but never credentials/tokens

### Project Structure Notes

- `src/NginxApiClient/Internal/ErrorHandlingDelegatingHandler.cs`
- `tests/NginxApiClient.Tests/Handlers/ErrorHandlingDelegatingHandlerTests.cs`

### References

- [Source: architecture.md#Error Handling Architecture]
- [Source: architecture.md#HTTP Pipeline Boundary]
- [Source: prd.md#FR35, FR36, FR37]
- [Source: prd.md#NFR8]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

ErrorHandlingDelegatingHandler implemented with full status code mapping. Network error wrapping handles both HttpRequestException and TaskCanceledException. NPM JSON error body parsing is gracefully fault-tolerant. All tests pass; no credentials exposed in any exception path.

### File List

### Change Log

| Date | Change |
|------|--------|
| 2026-04-17 | Implementation complete; status set to review |
