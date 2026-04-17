# Story 2.1: Client Options & Token Models

Status: ready-for-dev

## Story

As a developer,
I want to configure the NPM instance URL and credentials at initialization,
so that the library knows which NPM instance to communicate with.

## Acceptance Criteria

1. `NginxProxyManagerClientOptions` has `BaseUrl` (string) and `Credentials` with `Email`/`Password` properties
2. `TokenRequest` model maps to NPM's `identity`/`secret` JSON fields
3. `TokenResponse` model has `Token` (string) and `Expires` (DateTime) properties
4. Options validation throws `ArgumentException` for null/empty `BaseUrl`, `Email`, or `Password`

## Tasks / Subtasks

- [ ] Task 1: Create NginxProxyManagerClientOptions (AC: #1, #4)
  - [ ] `BaseUrl` property (string)
  - [ ] `Credentials` property with `Email` and `Password`
  - [ ] `Validate()` method throwing `ArgumentException` for invalid inputs
  - [ ] XML doc comments
- [ ] Task 2: Create TokenRequest model (AC: #2)
  - [ ] `Identity` property (maps to email)
  - [ ] `Secret` property (maps to password)
  - [ ] Place in `Models/Tokens/` namespace
- [ ] Task 3: Create TokenResponse model (AC: #3)
  - [ ] `Token` property (string — JWT)
  - [ ] `Expires` property (DateTime)
  - [ ] Place in `Models/Tokens/` namespace
- [ ] Task 4: Write options validation tests (AC: #4)
  - [ ] Test valid options pass validation
  - [ ] Test null/empty BaseUrl throws
  - [ ] Test null/empty Email throws
  - [ ] Test null/empty Password throws

## Dev Notes

- **Architecture reference:** [Source: architecture.md#HTTP Client Management]
- **NPM API auth endpoint:** `POST /api/tokens` with `{"identity": "email", "secret": "password"}`
- **Security NFR:** Credentials must never be logged or serialized to disk (NFR5)
- **Models are POCOs** — no serializer attributes in core

### Project Structure Notes

- `src/NginxApiClient/NginxProxyManagerClientOptions.cs`
- `src/NginxApiClient/Models/Tokens/TokenRequest.cs`
- `src/NginxApiClient/Models/Tokens/TokenResponse.cs`

### References

- [Source: architecture.md#HTTP Client Management]
- [Source: product-brief-distillate.md#NPM API Surface — Auth]
- [Source: prd.md#FR1, FR5]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

