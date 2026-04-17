# Story 7.3: Runnable Example Projects

Status: review

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

- [x] Task 1: Create BasicUsage example project (AC: #1, #4)
  - [x] `examples/BasicUsage/BasicUsage.csproj` targeting net8.0
  - [x] `Program.cs` with authenticate, list, create, delete proxy host
  - [x] Inline comments explaining each step
  - [x] Add to solution file
- [x] Task 2: Create CertificateManagement example project (AC: #2, #4)
  - [x] `examples/CertificateManagement/CertificateManagement.csproj` targeting net8.0
  - [x] `Program.cs` with list certs, filter by expiry, provision, renew
  - [x] Inline comments
  - [x] Add to solution file
- [x] Task 3: Create LegacyFrameworkUsage example project (AC: #3, #4)
  - [x] `examples/LegacyFrameworkUsage/LegacyFrameworkUsage.csproj` targeting net48
  - [x] References `NginxApiClient.NewtonsoftJson` instead of SystemTextJson
  - [x] `Program.cs` with same basic usage as BasicUsage
  - [x] Inline comments
  - [x] Add to solution file
- [x] Task 4: Verify all examples compile
  - [x] `dotnet build` on all example projects
  - [x] Verify project references resolve correctly

## Dev Notes

- **Examples should use placeholder URLs** (e.g., `http://localhost:81`) — they won't run without a live NPM instance
- **Comments should explain** what each line does, not just what it calls
- **net48 example** demonstrates that the library works on .NET Framework 4.8

### References

- [Source: prd.md#FR47]
- [Source: prd.md#Code Examples Requirements]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- All three example projects (BasicUsage, CertificateManagement, LegacyFrameworkUsage) created, added to the solution, and verified to compile successfully. Inline comments explain each step.

### Change Log

- 2026-04-17: Story completed and moved to review status.

### File List

