# Story 3.3: Proxy Host Client — Enable/Disable & Advanced Configuration

Status: review

## Story

As a developer,
I want to enable/disable proxy hosts and configure SSL, websockets, locations, and custom nginx directives,
so that I have full control over proxy host behavior.

## Acceptance Criteria

1. `DisableAsync(id)` disables a proxy host
2. `EnableAsync(id)` re-enables a proxy host
3. Creating a proxy host with SSL settings (`SslForced`, `HstsEnabled`, `Http2Support`) applies them
4. Creating a proxy host with `Locations` enables path-based routing
5. Creating a proxy host with `AdvancedConfig` includes custom nginx directives

## Tasks / Subtasks

- [x] Task 1: Implement EnableAsync and DisableAsync (AC: #1, #2)
  - [x] `EnableAsync` — POST/PUT to appropriate NPM endpoint
  - [x] `DisableAsync` — POST/PUT to appropriate NPM endpoint
  - [x] Determine exact NPM API mechanism for enable/disable (may be PUT with `enabled` field)
- [x] Task 2: Write enable/disable tests (AC: #1, #2)
  - [x] `EnableAsync_Succeeds_WhenIdExists`
  - [x] `DisableAsync_Succeeds_WhenIdExists`
  - [x] Verify correct HTTP call
- [x] Task 3: Write integration-style tests for advanced config (AC: #3, #4, #5)
  - [x] Test create with SSL settings verifies request body contains correct fields
  - [x] Test create with locations verifies `locations[]` array in request
  - [x] Test create with `AdvancedConfig` verifies custom directives in request
  - [x] Verify all optional properties serialize correctly when null/empty

## Dev Notes

- **NPM enable/disable:** May use PUT with `{ "enabled": true/false }` — verify against live NPM or Bash API script
- **Reverse-engineering reference:** Bash API script (Erreur32) for exact enable/disable endpoint behavior
- **SSL properties:** `ssl_forced`, `hsts_enabled`, `hsts_subdomains`, `http2_support`
- **Locations:** Array of `{ path, forward_scheme, forward_host, forward_port, advanced_config }`
- **AdvancedConfig:** Raw string passed through to nginx config

### References

- [Source: product-brief-distillate.md#Key Proxy Host Properties]
- [Source: prd.md#FR11, FR12, FR13, FR14, FR15]
- [Source: product-brief-distillate.md#API Documentation Gaps — Bash API script as reference]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- `EnableAsync` and `DisableAsync` implemented via PUT with `{ "enabled": true/false }` after verifying against NPM API behavior. Integration-style tests added to cover SSL settings, location-based routing, and `AdvancedConfig` serialization. All optional properties verified to serialize correctly when null or empty.

### Change Log

- 2026-04-17: Story completed and status set to review. All tasks finished and acceptance criteria met.

### File List

