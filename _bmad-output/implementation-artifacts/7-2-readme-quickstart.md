# Story 7.2: README & Quick-Start Guide

Status: ready-for-dev

## Story

As a developer,
I want a README with installation instructions and a quick-start guide,
so that I can make my first API call within 5 minutes.

## Acceptance Criteria

1. README includes installation instructions for modern .NET and .NET Framework 4.8
2. Quick-start code example: install → authenticate → list proxy hosts in under 10 lines
3. DI registration example for ASP.NET Core
4. Create proxy host with SSL example
5. Links to examples/ folder for more scenarios

## Tasks / Subtasks

- [ ] Task 1: Write installation section (AC: #1)
  - [ ] `dotnet add package` commands for modern .NET
  - [ ] `Install-Package` commands for .NET Framework 4.8
  - [ ] Explain which serializer package to choose
- [ ] Task 2: Write quick-start guide (AC: #2)
  - [ ] Under 10 lines: create client, authenticate, list proxy hosts
  - [ ] Show both DI and manual construction approaches
- [ ] Task 3: Write DI registration example (AC: #3)
  - [ ] ASP.NET Core `Program.cs` example with `AddNginxApiClient`
  - [ ] Controller injection example
- [ ] Task 4: Write create proxy host example (AC: #4)
  - [ ] Create proxy host with domain names, forward host/port, SSL settings
- [ ] Task 5: Add examples section with links (AC: #5)
  - [ ] Link to `examples/BasicUsage`
  - [ ] Link to `examples/CertificateManagement`
  - [ ] Link to `examples/LegacyFrameworkUsage`
- [ ] Task 6: Add badges, license, and project description
  - [ ] NuGet badge, build status badge
  - [ ] MIT license reference
  - [ ] Brief project description and feature highlights

## Dev Notes

- **Target:** Developer can go from zero to first API call in under 5 minutes
- **Tone:** Clear, concise, copy-paste friendly
- **Do NOT overwrite existing README** — replace current content with comprehensive version

### References

- [Source: prd.md#FR46]
- [Source: prd.md#Documentation Strategy]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

