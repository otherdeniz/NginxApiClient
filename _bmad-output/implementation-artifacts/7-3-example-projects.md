# Story 7.3: Runnable Example Projects

Status: ready-for-dev

## Story

As a developer,
I want runnable console project examples,
so that I can copy and adapt real-world scenarios.

## Acceptance Criteria

1. `examples/BasicUsage` (net8.0) demonstrates: authenticate, list proxy hosts, create, delete
2. `examples/CertificateManagement` (net8.0) demonstrates: list certs, check expiry, provision Let's Encrypt, renew
3. `examples/LegacyFrameworkUsage` (net48) demonstrates basic usage with NewtonsoftJson
4. All examples compile and include inline comments

## Tasks / Subtasks

- [ ] Task 1: Create BasicUsage example project (AC: #1, #4)
  - [ ] `examples/BasicUsage/BasicUsage.csproj` targeting net8.0
  - [ ] `Program.cs` with authenticate, list, create, delete proxy host
  - [ ] Inline comments explaining each step
  - [ ] Add to solution file
- [ ] Task 2: Create CertificateManagement example project (AC: #2, #4)
  - [ ] `examples/CertificateManagement/CertificateManagement.csproj` targeting net8.0
  - [ ] `Program.cs` with list certs, filter by expiry, provision, renew
  - [ ] Inline comments
  - [ ] Add to solution file
- [ ] Task 3: Create LegacyFrameworkUsage example project (AC: #3, #4)
  - [ ] `examples/LegacyFrameworkUsage/LegacyFrameworkUsage.csproj` targeting net48
  - [ ] References `NginxApiClient.NewtonsoftJson` instead of SystemTextJson
  - [ ] `Program.cs` with same basic usage as BasicUsage
  - [ ] Inline comments
  - [ ] Add to solution file
- [ ] Task 4: Verify all examples compile
  - [ ] `dotnet build` on all example projects
  - [ ] Verify project references resolve correctly

## Dev Notes

- **Examples should use placeholder URLs** (e.g., `http://localhost:81`) — they won't run without a live NPM instance
- **Comments should explain** what each line does, not just what it calls
- **net48 example** demonstrates that the library works on .NET Framework 4.8

### References

- [Source: prd.md#FR47]
- [Source: prd.md#Code Examples Requirements]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

