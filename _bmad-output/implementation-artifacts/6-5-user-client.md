# Story 6.5: User Administration Client

Status: review

## Story

As a developer,
I want to manage NPM users and their permissions,
so that I can automate user provisioning and access control.

## Acceptance Criteria

1. All CRUD operations work following the established pattern
2. User permissions can be managed
3. Unit tests cover all operations

## Tasks / Subtasks

- [x] Task 1: Create user models in `Models/Users/`
  - [x] `UserResponse`, `CreateUserRequest`, `UpdateUserRequest`
  - [x] Properties: `Id`, `Name`, `Nickname`, `Email`, `IsDisabled`, `Roles`, `Permissions`
- [x] Task 2: Implement UserClient
  - [x] Endpoints: `/api/users`
  - [x] CRUD operations + permission management
- [x] Task 3: Wire into NginxProxyManagerClient root
- [x] Task 4: Write unit tests

## Dev Notes

- **NPM endpoint:** `/api/users` (NOT under `/api/nginx/`)
- **Permissions model** needs discovery from NPM source/API testing
- **Admin operations** — these endpoints may require admin-level authentication

### References

- [Source: product-brief-distillate.md#Endpoint Map — Users]
- [Source: prd.md#FR30, FR31]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- Implemented UserClient targeting `/api/users`; CRUD and permission management operations complete; permissions model confirmed via API testing; unit tests complete.

### Change Log

- 2026-04-17: Story completed and moved to review status.

### File List

