# Story 2.3: Authentication Delegating Handler

Status: review

## Story

As a developer,
I want transparent JWT authentication on every API call,
so that I never manually manage tokens or deal with 401 errors from expired tokens.

## Acceptance Criteria

1. Handler adds `Authorization: Bearer {token}` header to every outgoing request
2. On 401 response, handler acquires new token and retries the request exactly once
3. If retry also fails with 401, `NginxAuthenticationException` is thrown
4. Concurrent 401 responses trigger only one token refresh (via SemaphoreSlim)
5. All waiting threads use the refreshed token

## Tasks / Subtasks

- [x] Task 1: Create AuthenticationDelegatingHandler (AC: #1)
  - [x] Extend `DelegatingHandler`
  - [x] Internal class in `NginxApiClient.Internal`
  - [x] Inject `TokenStore` and `NginxProxyManagerClientOptions`
  - [x] Override `SendAsync` to add Bearer header from TokenStore
  - [x] Skip auth header for token endpoint requests (`/api/tokens`)
- [x] Task 2: Implement 401 retry logic (AC: #2, #3)
  - [x] On 401 response, call `TokenStore.InvalidateTokenAsync()`
  - [x] Re-acquire token via `TokenStore.GetTokenAsync()`
  - [x] Retry original request with new token
  - [x] If retry returns 401, throw `NginxAuthenticationException`
- [x] Task 3: Implement token acquisition HTTP call (AC: #1)
  - [x] POST to `/api/tokens` with `TokenRequest` body
  - [x] Deserialize `TokenResponse` and store in `TokenStore`
  - [x] Use `IJsonSerializer` for request/response serialization
- [x] Task 4: Write handler tests (AC: #1, #2, #3, #4, #5)
  - [x] Test Bearer header is added to requests
  - [x] Test 401 triggers re-auth and retry
  - [x] Test double-401 throws `NginxAuthenticationException`
  - [x] Test concurrent 401s trigger single refresh
  - [x] Test token endpoint requests skip auth header

## Dev Notes

- **Architecture reference:** [Source: architecture.md#Token Management Architecture]
- **Pipeline order:** AuthenticationDelegatingHandler → ErrorHandlingDelegatingHandler → HttpClientHandler
- **NPM token endpoint:** `POST /api/tokens` with `{"identity": "email", "secret": "password"}`
- **Security:** Never log credentials or tokens in handler code
- **Testing:** Use `MockHttpMessageHandler` as inner handler

### Project Structure Notes

- `src/NginxApiClient/Internal/AuthenticationDelegatingHandler.cs`
- `tests/NginxApiClient.Tests/Handlers/AuthenticationDelegatingHandlerTests.cs`

### References

- [Source: architecture.md#Token Management Architecture]
- [Source: architecture.md#HTTP Pipeline Boundary]
- [Source: prd.md#FR1, FR2, FR3, FR4]
- [Source: product-brief-distillate.md#NPM API Surface — Auth]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

AuthenticationDelegatingHandler implemented with full 401 retry and token refresh logic. Concurrent 401 handling verified via tests using MockHttpMessageHandler. Token endpoint bypass confirmed working.

### File List

### Change Log

| Date | Change |
|------|--------|
| 2026-04-17 | Implementation complete; status set to review |
