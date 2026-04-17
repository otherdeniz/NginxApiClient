# Story 1.1: Solution Scaffolding & Build Infrastructure

Status: ready-for-dev

## Story

As a developer,
I want a multi-project .NET solution with shared build properties,
so that all packages build consistently across target frameworks.

## Acceptance Criteria

1. Running `dotnet build` from repository root compiles all projects successfully
2. `NginxApiClient` targets `netstandard2.0`
3. `NginxApiClient.SystemTextJson` targets `net8.0;net10.0`
4. `NginxApiClient.NewtonsoftJson` targets `netstandard2.0`
5. `Directory.Build.props` contains shared package metadata (authors, license, repository URL, tags)
6. `Directory.Packages.props` enables Central Package Management
7. `.editorconfig` enforces consistent code style
8. Test projects target `net8.0`
9. All projects are added to the solution file

## Tasks / Subtasks

- [ ] Task 1: Create solution file and project structure (AC: #1, #9)
  - [ ] Create `NginxApiClient.sln`
  - [ ] Create `src/NginxApiClient/NginxApiClient.csproj` targeting `netstandard2.0`
  - [ ] Create `src/NginxApiClient.SystemTextJson/NginxApiClient.SystemTextJson.csproj` targeting `net8.0;net10.0`
  - [ ] Create `src/NginxApiClient.NewtonsoftJson/NginxApiClient.NewtonsoftJson.csproj` targeting `netstandard2.0`
  - [ ] Create `tests/NginxApiClient.Tests/NginxApiClient.Tests.csproj` targeting `net8.0`
  - [ ] Create `tests/NginxApiClient.IntegrationTests/NginxApiClient.IntegrationTests.csproj` targeting `net8.0`
  - [ ] Add all projects to the solution
- [ ] Task 2: Create Directory.Build.props with shared properties (AC: #5)
  - [ ] Package metadata: authors (Deniz Esen), license (MIT), repository URL, tags
  - [ ] SourceLink configuration for symbol packages
  - [ ] `.snupkg` symbol package generation enabled
  - [ ] `TreatWarningsAsErrors` for production code
  - [ ] XML documentation generation enabled
- [ ] Task 3: Create Directory.Packages.props for Central Package Management (AC: #6)
  - [ ] Define package versions for xUnit, Newtonsoft.Json, System.Text.Json
  - [ ] Define package versions for test dependencies (xunit.runner, FluentAssertions or similar)
- [ ] Task 4: Create .editorconfig for code style enforcement (AC: #7)
  - [ ] C# naming conventions (PascalCase for public, _camelCase for private fields)
  - [ ] Indentation and formatting rules
  - [ ] Nullable reference type settings per target
- [ ] Task 5: Create .gitignore for .NET projects
- [ ] Task 6: Add project references (AC: #2, #3, #4, #8)
  - [ ] `NginxApiClient.SystemTextJson` references `NginxApiClient`
  - [ ] `NginxApiClient.NewtonsoftJson` references `NginxApiClient`
  - [ ] Test projects reference all source projects
  - [ ] Add `InternalsVisibleTo` for test projects in core package
- [ ] Task 7: Verify full solution builds (AC: #1)
  - [ ] Run `dotnet build` and confirm zero errors
  - [ ] Run `dotnet test` and confirm test projects are discovered

## Dev Notes

- **Architecture reference:** [Source: architecture.md#Starter Template & Project Initialization]
- **Solution structure follows:** `src/` for production, `tests/` for tests, `examples/` for samples
- **Critical:** Core package (`NginxApiClient`) must have ZERO external runtime dependencies
- **Critical:** `NginxApiClient.SystemTextJson` depends only on core + System.Text.Json
- **Critical:** `NginxApiClient.NewtonsoftJson` depends only on core + Newtonsoft.Json
- **Naming conventions:** [Source: architecture.md#Naming Patterns] — PascalCase for classes, _camelCase for private fields
- **Package tags:** `nginx-proxy-manager`, `npm-api`, `reverse-proxy`, `nginx`, `docker`
- **MIT License** file already exists in repository root

### Project Structure Notes

```
NginxApiClient/
├── NginxApiClient.sln
├── Directory.Build.props
├── Directory.Packages.props
├── .editorconfig
├── .gitignore
├── src/
│   ├── NginxApiClient/NginxApiClient.csproj
│   ├── NginxApiClient.SystemTextJson/NginxApiClient.SystemTextJson.csproj
│   └── NginxApiClient.NewtonsoftJson/NginxApiClient.NewtonsoftJson.csproj
├── tests/
│   ├── NginxApiClient.Tests/NginxApiClient.Tests.csproj
│   └── NginxApiClient.IntegrationTests/NginxApiClient.IntegrationTests.csproj
└── examples/ (created in later stories)
```

### References

- [Source: architecture.md#Starter Template & Project Initialization]
- [Source: architecture.md#NuGet Multi-Targeting Strategy]
- [Source: architecture.md#NuGet Package Requirements]
- [Source: prd.md#Developer Tool Specific Requirements]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

