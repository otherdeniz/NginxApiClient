# Story 2.4: Error Handling Delegating Handler

Status: ready-for-dev

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

- [ ] Task 1: Create ErrorHandlingDelegatingHandler (AC: #1, #2)
  - [ ] Extend `DelegatingHandler`
  - [ ] Internal class in `NginxApiClient.Internal`
  - [ ] Override `SendAsync` to inspect response status codes
  - [ ] Parse NPM error detail from response body JSON
  - [ ] Map 404 → `NginxNotFoundException`
  - [ ] Map other 4xx/5xx → `NginxApiException`
  - [ ] Pass through 2xx responses untouched
- [ ] Task 2: Implement network error handling (AC: #3, #4)
  - [ ] Catch `HttpRequestException`, `TaskCanceledException`
  - [ ] Wrap in `NginxApiException` with descriptive message
  - [ ] Set original exception as `InnerException`
  - [ ] Ensure no credentials leak in error messages
- [ ] Task 3: Write error handler tests (AC: #1, #2, #3, #4)
  - [ ] Test 404 → `NginxNotFoundException`
  - [ ] Test 422 → `NginxApiException` with error detail
  - [ ] Test 500 → `NginxApiException`
  - [ ] Test network timeout → `NginxApiException` with inner exception
  - [ ] Test 200 passes through without exception
  - [ ] Test error detail parsed from NPM JSON response body
  - [ ] Test credentials not present in any exception message

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

### Debug Log References

### Completion Notes List

### File List

