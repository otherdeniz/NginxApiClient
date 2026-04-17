# Story 1.3: Exception Hierarchy

Status: ready-for-dev

## Story

As a developer,
I want typed exceptions for NPM API errors,
so that I can catch and handle specific error scenarios in my code.

## Acceptance Criteria

1. `NginxApiException` is the base exception with `StatusCode` (int), `ErrorDetail` (string), `RawResponse` (string) properties
2. `NginxAuthenticationException` extends `NginxApiException` for 401 errors
3. `NginxNotFoundException` extends `NginxApiException` for 404 errors
4. No exception `Message` or `ToString()` output contains credentials or JWT tokens
5. All exceptions are serializable

## Tasks / Subtasks

- [ ] Task 1: Create NginxApiException base class (AC: #1, #4, #5)
  - [ ] Properties: `StatusCode`, `ErrorDetail`, `RawResponse`
  - [ ] Constructors: message-only, message+inner, statusCode+detail+raw
  - [ ] Override `ToString()` to include status code and detail but never credentials
  - [ ] Add XML doc comments
- [ ] Task 2: Create NginxAuthenticationException (AC: #2)
  - [ ] Extend `NginxApiException`
  - [ ] Default status code 401
  - [ ] Add XML doc comments
- [ ] Task 3: Create NginxNotFoundException (AC: #3)
  - [ ] Extend `NginxApiException`
  - [ ] Default status code 404
  - [ ] Add XML doc comments
- [ ] Task 4: Write exception tests (AC: #1, #2, #3, #4)
  - [ ] Test each exception type is catchable by base type
  - [ ] Test properties are correctly set
  - [ ] Test `ToString()` does not contain sensitive data
  - [ ] Test serialization round-trip

## Dev Notes

- **Architecture reference:** [Source: architecture.md#Error Handling Architecture]
- **Namespace:** `NginxApiClient.Exceptions`
- **Security NFR:** Credentials and JWT tokens must NEVER appear in exception messages (NFR5, NFR8)
- **Used by:** `ErrorHandlingDelegatingHandler` (Story 2.4) maps HTTP responses to these exceptions

### Project Structure Notes

- `src/NginxApiClient/Exceptions/NginxApiException.cs`
- `src/NginxApiClient/Exceptions/NginxAuthenticationException.cs`
- `src/NginxApiClient/Exceptions/NginxNotFoundException.cs`
- `tests/NginxApiClient.Tests/Exceptions/` (test files)

### References

- [Source: architecture.md#Error Handling Architecture]
- [Source: prd.md#FR35, FR36, FR37]
- [Source: prd.md#NFR5, NFR8 — Security]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

