# Story 6.2: Dead Host (404) Client

Status: review

## Story

As a developer,
I want to manage dead hosts (custom 404 pages),
so that I can automate 404 page configuration.

## Acceptance Criteria

1. All CRUD + enable/disable operations work following the established pattern
2. Unit tests cover all operations

## Tasks / Subtasks

- [x] Task 1: Create dead host models in `Models/DeadHosts/`
  - [x] `DeadHostResponse`, `CreateDeadHostRequest`, `UpdateDeadHostRequest`
- [x] Task 2: Implement DeadHostClient
  - [x] Endpoints: `/api/nginx/dead-hosts`
  - [x] Follow established resource client pattern
- [x] Task 3: Wire into NginxProxyManagerClient root
- [x] Task 4: Write unit tests

## Dev Notes

- **NPM endpoint:** `/api/nginx/dead-hosts`
- **Follow ProxyHostClient pattern** exactly

### References

- [Source: product-brief-distillate.md#Endpoint Map — Dead Hosts]
- [Source: prd.md#FR25, FR26]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- Implemented DeadHostClient following the established resource client pattern; all CRUD and enable/disable operations covered; models and unit tests complete.

### Change Log

- 2026-04-17: Story completed and moved to review status.

### File List

