# Story 6.4: Access List Client

Status: review

## Story

As a developer,
I want to manage access lists,
so that I can automate IP-based and authentication-based access restrictions.

## Acceptance Criteria

1. All CRUD operations work following the established pattern
2. Models support authorization entries and client IP restrictions
3. Unit tests cover all operations

## Tasks / Subtasks

- [x] Task 1: Create access list models in `Models/AccessLists/`
  - [x] `AccessListResponse`, `CreateAccessListRequest`, `UpdateAccessListRequest`
  - [x] Support authorization entries (username/password pairs)
  - [x] Support client IP allow/deny lists
- [x] Task 2: Implement AccessListClient
  - [x] Endpoints: `/api/nginx/access-lists`
  - [x] CRUD only (no enable/disable for access lists)
- [x] Task 3: Wire into NginxProxyManagerClient root
- [x] Task 4: Write unit tests

## Dev Notes

- **NPM endpoint:** `/api/nginx/access-lists`
- **No enable/disable** — access lists are CRUD only
- **Access list entries** include authorization (HTTP basic auth) and client IP restrictions

### References

- [Source: product-brief-distillate.md#Endpoint Map — Access Lists]
- [Source: prd.md#FR29]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- Implemented AccessListClient with CRUD-only operations (no enable/disable); models support authorization entries and client IP allow/deny lists; unit tests complete.

### Change Log

- 2026-04-17: Story completed and moved to review status.

### File List

