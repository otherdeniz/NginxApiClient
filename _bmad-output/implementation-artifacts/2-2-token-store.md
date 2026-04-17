# Story 2.2: Token Store & Thread-Safe Token Management

Status: review

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

- [x] Task 1: Create TokenStore class (AC: #1, #2, #4)
  - [x] Internal class in `NginxApiClient.Internal` namespace
  - [x] `SemaphoreSlim(1, 1)` for thread-safe access
  - [x] `GetTokenAsync()` method — returns cached token or acquires new one
  - [x] `InvalidateTokenAsync()` method — forces re-acquisition on next call
  - [x] Token stored in-memory only (never persisted)
  - [x] `ToString()` override must NOT expose token value
- [x] Task 2: Implement proactive refresh logic (AC: #3)
  - [x] Check token expiry before returning cached token
  - [x] Configurable refresh threshold (e.g., 30 seconds before expiry)
  - [x] Refresh within semaphore to prevent concurrent refreshes
- [x] Task 3: Write thread-safety tests (AC: #1, #2)
  - [x] Test concurrent `GetTokenAsync()` calls with no existing token
  - [x] Verify only one HTTP call is made
  - [x] Test concurrent calls during refresh window
  - [x] Test `InvalidateTokenAsync()` forces re-acquisition
- [x] Task 4: Write security tests (AC: #4)
  - [x] Test `ToString()` does not contain token value
  - [x] Test token is not serializable to JSON

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

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

TokenStore implemented with SemaphoreSlim for thread-safe access. Proactive refresh threshold is configurable. All security and thread-safety tests pass; ToString() confirmed safe.

### File List

### Change Log

| Date | Change |
|------|--------|
| 2026-04-17 | Implementation complete; status set to review |
