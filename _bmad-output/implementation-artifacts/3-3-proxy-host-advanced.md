# Story 3.3: Proxy Host Client — Enable/Disable & Advanced Configuration

Status: ready-for-dev

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

- [ ] Task 1: Implement EnableAsync and DisableAsync (AC: #1, #2)
  - [ ] `EnableAsync` — POST/PUT to appropriate NPM endpoint
  - [ ] `DisableAsync` — POST/PUT to appropriate NPM endpoint
  - [ ] Determine exact NPM API mechanism for enable/disable (may be PUT with `enabled` field)
- [ ] Task 2: Write enable/disable tests (AC: #1, #2)
  - [ ] `EnableAsync_Succeeds_WhenIdExists`
  - [ ] `DisableAsync_Succeeds_WhenIdExists`
  - [ ] Verify correct HTTP call
- [ ] Task 3: Write integration-style tests for advanced config (AC: #3, #4, #5)
  - [ ] Test create with SSL settings verifies request body contains correct fields
  - [ ] Test create with locations verifies `locations[]` array in request
  - [ ] Test create with `AdvancedConfig` verifies custom directives in request
  - [ ] Verify all optional properties serialize correctly when null/empty

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

### Debug Log References

### Completion Notes List

### File List

