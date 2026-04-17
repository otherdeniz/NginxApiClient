# Story 2.1: Client Options & Token Models

Status: review

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

- [x] Task 1: Create NginxProxyManagerClientOptions (AC: #1, #4)
  - [x] `BaseUrl` property (string)
  - [x] `Credentials` property with `Email` and `Password`
  - [x] `Validate()` method throwing `ArgumentException` for invalid inputs
  - [x] XML doc comments
- [x] Task 2: Create TokenRequest model (AC: #2)
  - [x] `Identity` property (maps to email)
  - [x] `Secret` property (maps to password)
  - [x] Place in `Models/Tokens/` namespace
- [x] Task 3: Create TokenResponse model (AC: #3)
  - [x] `Token` property (string — JWT)
  - [x] `Expires` property (DateTime)
  - [x] Place in `Models/Tokens/` namespace
- [x] Task 4: Write options validation tests (AC: #4)
  - [x] Test valid options pass validation
  - [x] Test null/empty BaseUrl throws
  - [x] Test null/empty Email throws
  - [x] Test null/empty Password throws

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

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

All models and options class implemented as specified. Validation logic covers all required null/empty cases. Tests pass for all AC items.

### File List

### Change Log

| Date | Change |
|------|--------|
| 2026-04-17 | Implementation complete; status set to review |
