# Story 2.3: Authentication Delegating Handler

Status: ready-for-dev

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

- [ ] Task 1: Create AuthenticationDelegatingHandler (AC: #1)
  - [ ] Extend `DelegatingHandler`
  - [ ] Internal class in `NginxApiClient.Internal`
  - [ ] Inject `TokenStore` and `NginxProxyManagerClientOptions`
  - [ ] Override `SendAsync` to add Bearer header from TokenStore
  - [ ] Skip auth header for token endpoint requests (`/api/tokens`)
- [ ] Task 2: Implement 401 retry logic (AC: #2, #3)
  - [ ] On 401 response, call `TokenStore.InvalidateTokenAsync()`
  - [ ] Re-acquire token via `TokenStore.GetTokenAsync()`
  - [ ] Retry original request with new token
  - [ ] If retry returns 401, throw `NginxAuthenticationException`
- [ ] Task 3: Implement token acquisition HTTP call (AC: #1)
  - [ ] POST to `/api/tokens` with `TokenRequest` body
  - [ ] Deserialize `TokenResponse` and store in `TokenStore`
  - [ ] Use `IJsonSerializer` for request/response serialization
- [ ] Task 4: Write handler tests (AC: #1, #2, #3, #4, #5)
  - [ ] Test Bearer header is added to requests
  - [ ] Test 401 triggers re-auth and retry
  - [ ] Test double-401 throws `NginxAuthenticationException`
  - [ ] Test concurrent 401s trigger single refresh
  - [ ] Test token endpoint requests skip auth header

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

### Debug Log References

### Completion Notes List

### File List

