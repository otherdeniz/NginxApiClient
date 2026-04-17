# Story 6.2: Dead Host (404) Client

Status: ready-for-dev

## Story

As a developer,
I want to manage dead hosts (custom 404 pages),
so that I can automate 404 page configuration.

## Acceptance Criteria

1. All CRUD + enable/disable operations work following the established pattern
2. Unit tests cover all operations

## Tasks / Subtasks

- [ ] Task 1: Create dead host models in `Models/DeadHosts/`
  - [ ] `DeadHostResponse`, `CreateDeadHostRequest`, `UpdateDeadHostRequest`
- [ ] Task 2: Implement DeadHostClient
  - [ ] Endpoints: `/api/nginx/dead-hosts`
  - [ ] Follow established resource client pattern
- [ ] Task 3: Wire into NginxProxyManagerClient root
- [ ] Task 4: Write unit tests

## Dev Notes

- **NPM endpoint:** `/api/nginx/dead-hosts`
- **Follow ProxyHostClient pattern** exactly

### References

- [Source: product-brief-distillate.md#Endpoint Map — Dead Hosts]
- [Source: prd.md#FR25, FR26]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

