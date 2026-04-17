# Story 2.2: Token Store & Thread-Safe Token Management

Status: ready-for-dev

## Story

As a developer,
I want the library to store and manage JWT tokens thread-safely,
so that concurrent API calls share a single token and refresh doesn't race.

## Acceptance Criteria

1. Multiple threads requesting token simultaneously before any token exists trigger only one auth call
2. All threads receive the same token
3. Token approaching expiry (within refresh threshold) is refreshed proactively
4. Token never appears in logs or diagnostic output

## Tasks / Subtasks

- [ ] Task 1: Create TokenStore class (AC: #1, #2, #4)
  - [ ] Internal class in `NginxApiClient.Internal` namespace
  - [ ] `SemaphoreSlim(1, 1)` for thread-safe access
  - [ ] `GetTokenAsync()` method — returns cached token or acquires new one
  - [ ] `InvalidateTokenAsync()` method — forces re-acquisition on next call
  - [ ] Token stored in-memory only (never persisted)
  - [ ] `ToString()` override must NOT expose token value
- [ ] Task 2: Implement proactive refresh logic (AC: #3)
  - [ ] Check token expiry before returning cached token
  - [ ] Configurable refresh threshold (e.g., 30 seconds before expiry)
  - [ ] Refresh within semaphore to prevent concurrent refreshes
- [ ] Task 3: Write thread-safety tests (AC: #1, #2)
  - [ ] Test concurrent `GetTokenAsync()` calls with no existing token
  - [ ] Verify only one HTTP call is made
  - [ ] Test concurrent calls during refresh window
  - [ ] Test `InvalidateTokenAsync()` forces re-acquisition
- [ ] Task 4: Write security tests (AC: #4)
  - [ ] Test `ToString()` does not contain token value
  - [ ] Test token is not serializable to JSON

## Dev Notes

- **Architecture reference:** [Source: architecture.md#Token Management Architecture]
- **Thread safety:** `SemaphoreSlim(1,1)` — concurrent 401s trigger single refresh, others wait
- **NFR14:** Token refresh under concurrent access uses a single refresh call
- **NFR6:** JWT tokens stored only in memory, never persisted or in diagnostic output
- **Token acquisition** is delegated to a callback/func provided at construction (actual HTTP call made by AuthenticationDelegatingHandler)

### Project Structure Notes

- `src/NginxApiClient/Internal/TokenStore.cs`
- `tests/NginxApiClient.Tests/Handlers/TokenStoreTests.cs`

### References

- [Source: architecture.md#Token Management Architecture]
- [Source: prd.md#FR2, FR3]
- [Source: prd.md#NFR6, NFR14]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

