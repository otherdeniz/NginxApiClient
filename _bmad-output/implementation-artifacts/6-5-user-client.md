# Story 6.5: User Administration Client

Status: ready-for-dev

## Story

As a developer,
I want to manage NPM users and their permissions,
so that I can automate user provisioning and access control.

## Acceptance Criteria

1. All CRUD operations work following the established pattern
2. User permissions can be managed
3. Unit tests cover all operations

## Tasks / Subtasks

- [ ] Task 1: Create user models in `Models/Users/`
  - [ ] `UserResponse`, `CreateUserRequest`, `UpdateUserRequest`
  - [ ] Properties: `Id`, `Name`, `Nickname`, `Email`, `IsDisabled`, `Roles`, `Permissions`
- [ ] Task 2: Implement UserClient
  - [ ] Endpoints: `/api/users`
  - [ ] CRUD operations + permission management
- [ ] Task 3: Wire into NginxProxyManagerClient root
- [ ] Task 4: Write unit tests

## Dev Notes

- **NPM endpoint:** `/api/users` (NOT under `/api/nginx/`)
- **Permissions model** needs discovery from NPM source/API testing
- **Admin operations** — these endpoints may require admin-level authentication

### References

- [Source: product-brief-distillate.md#Endpoint Map — Users]
- [Source: prd.md#FR30, FR31]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

