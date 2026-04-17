# Story 7.4: CI/CD Pipeline & NuGet Publishing

Status: ready-for-dev

## Story

As a developer,
I want automated CI/CD that builds, tests, and publishes to NuGet on release,
so that every release is consistently built and published.

## Acceptance Criteria

1. GitHub Actions CI builds all projects across all target frameworks on PR/push
2. CI runs all unit tests and fails on test failure or XML doc warning
3. Release tag (e.g., `v1.0.0`) triggers NuGet publish for all three packages
4. `.snupkg` symbol packages are published alongside

## Tasks / Subtasks

- [ ] Task 1: Create CI workflow `.github/workflows/ci.yml` (AC: #1, #2)
  - [ ] Trigger on push and pull_request
  - [ ] Matrix build across target frameworks
  - [ ] `dotnet build` all projects
  - [ ] `dotnet test` all test projects
  - [ ] Fail on warnings (XML doc, code analysis)
- [ ] Task 2: Create publish workflow `.github/workflows/publish.yml` (AC: #3, #4)
  - [ ] Trigger on release tag push (`v*`)
  - [ ] Extract version from tag
  - [ ] `dotnet pack` all three source packages with version from tag
  - [ ] `dotnet nuget push` to NuGet.org (using `NUGET_API_KEY` secret)
  - [ ] Push `.snupkg` symbol packages
- [ ] Task 3: Verify workflows (AC: #1-#4)
  - [ ] Test CI workflow runs locally with `act` or verify YAML syntax
  - [ ] Verify pack generates correct package names and versions
  - [ ] Verify snupkg files are generated alongside nupkg

## Dev Notes

- **GitHub Actions secrets:** Will need `NUGET_API_KEY` configured in repo settings
- **Version strategy:** Tag-based versioning — `v1.0.0` tag → package version `1.0.0`
- **Matrix build** should cover: `netstandard2.0` (via building core), `net8.0`, `net10.0` targets
- **Do NOT set up integration tests in CI** for this story — that requires a Docker NPM instance (future enhancement)

### References

- [Source: architecture.md#Starter Template — CI/CD]
- [Source: prd.md#NFR19]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

