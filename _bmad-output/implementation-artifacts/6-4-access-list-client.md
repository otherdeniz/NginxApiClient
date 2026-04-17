# Story 6.4: Access List Client

Status: ready-for-dev

## Story

As a developer,
I want to manage access lists,
so that I can automate IP-based and authentication-based access restrictions.

## Acceptance Criteria

1. All CRUD operations work following the established pattern
2. Models support authorization entries and client IP restrictions
3. Unit tests cover all operations

## Tasks / Subtasks

- [ ] Task 1: Create access list models in `Models/AccessLists/`
  - [ ] `AccessListResponse`, `CreateAccessListRequest`, `UpdateAccessListRequest`
  - [ ] Support authorization entries (username/password pairs)
  - [ ] Support client IP allow/deny lists
- [ ] Task 2: Implement AccessListClient
  - [ ] Endpoints: `/api/nginx/access-lists`
  - [ ] CRUD only (no enable/disable for access lists)
- [ ] Task 3: Wire into NginxProxyManagerClient root
- [ ] Task 4: Write unit tests

## Dev Notes

- **NPM endpoint:** `/api/nginx/access-lists`
- **No enable/disable** — access lists are CRUD only
- **Access list entries** include authorization (HTTP basic auth) and client IP restrictions

### References

- [Source: product-brief-distillate.md#Endpoint Map — Access Lists]
- [Source: prd.md#FR29]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

