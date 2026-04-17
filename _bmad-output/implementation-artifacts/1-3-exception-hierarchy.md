# Story 1.3: Exception Hierarchy

Status: review

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

- [x] Task 1: Create NginxApiException base class (AC: #1, #4, #5)
  - [x] Properties: `StatusCode`, `ErrorDetail`, `RawResponse`
  - [x] Constructors: message-only, message+inner, statusCode+detail+raw, statusCode+detail+raw+inner
  - [x] Override `ToString()` to include status code and detail but never credentials
  - [x] Add XML doc comments
- [x] Task 2: Create NginxAuthenticationException (AC: #2)
  - [x] Extend `NginxApiException`
  - [x] Default status code 401
  - [x] Add XML doc comments
- [x] Task 3: Create NginxNotFoundException (AC: #3)
  - [x] Extend `NginxApiException`
  - [x] Default status code 404
  - [x] Add XML doc comments
- [x] Task 4: Write exception tests (AC: #1, #2, #3, #4)
  - [x] Test each exception type is catchable by base type
  - [x] Test properties are correctly set
  - [x] Test `ToString()` does not contain sensitive data
  - [x] Test null input handling

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Completion Notes List

- Exception hierarchy: NginxApiException → NginxAuthenticationException (401), NginxNotFoundException (404)
- ToString() includes status code and error detail, never credentials
- Null-safe: ErrorDetail and RawResponse default to empty string
- 11 exception tests passing, 26 total tests passing

### File List

- src/NginxApiClient/Exceptions/NginxApiException.cs (new)
- src/NginxApiClient/Exceptions/NginxAuthenticationException.cs (new)
- src/NginxApiClient/Exceptions/NginxNotFoundException.cs (new)
- tests/NginxApiClient.Tests/Exceptions/ExceptionTests.cs (new)

### Change Log

- 2026-04-17: Story 1.3 implemented — Exception hierarchy with 11 tests
