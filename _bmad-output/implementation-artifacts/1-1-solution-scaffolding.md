# Story 1.1: Solution Scaffolding & Build Infrastructure

Status: review

## Story

As a developer,
I want a multi-project .NET solution with shared build properties,
so that all packages build consistently across target frameworks.

## Acceptance Criteria

1. Running `dotnet build` from repository root compiles all projects successfully
2. `NginxApiClient` targets `netstandard2.0`
3. `NginxApiClient.SystemTextJson` targets `net8.0;net9.0`
4. `NginxApiClient.NewtonsoftJson` targets `netstandard2.0`
5. `Directory.Build.props` contains shared package metadata (authors, license, repository URL, tags)
6. `Directory.Packages.props` enables Central Package Management
7. `.editorconfig` enforces consistent code style
8. Test projects target `net8.0`
9. All projects are added to the solution file

## Tasks / Subtasks

- [x] Task 1: Create solution file and project structure (AC: #1, #9)
  - [x] Create `NginxApiClient.sln`
  - [x] Create `src/NginxApiClient/NginxApiClient.csproj` targeting `netstandard2.0`
  - [x] Create `src/NginxApiClient.SystemTextJson/NginxApiClient.SystemTextJson.csproj` targeting `net8.0;net9.0`
  - [x] Create `src/NginxApiClient.NewtonsoftJson/NginxApiClient.NewtonsoftJson.csproj` targeting `netstandard2.0`
  - [x] Create `tests/NginxApiClient.Tests/NginxApiClient.Tests.csproj` targeting `net8.0`
  - [x] Create `tests/NginxApiClient.IntegrationTests/NginxApiClient.IntegrationTests.csproj` targeting `net8.0`
  - [x] Add all projects to the solution
- [x] Task 2: Create Directory.Build.props with shared properties (AC: #5)
  - [x] Package metadata: authors (Deniz Esen), license (MIT), repository URL, tags
  - [x] SourceLink configuration for symbol packages
  - [x] `.snupkg` symbol package generation enabled
  - [x] `TreatWarningsAsErrors` for production code
  - [x] XML documentation generation enabled
- [x] Task 3: Create Directory.Packages.props for Central Package Management (AC: #6)
  - [x] Define package versions for xUnit, Newtonsoft.Json, System.Text.Json
  - [x] Define package versions for test dependencies (xunit.runner, FluentAssertions)
- [x] Task 4: Create .editorconfig for code style enforcement (AC: #7)
  - [x] C# naming conventions (PascalCase for public, _camelCase for private fields)
  - [x] Indentation and formatting rules
  - [x] Nullable reference type settings per target
- [x] Task 5: Verify .gitignore for .NET projects (already existed)
- [x] Task 6: Add project references (AC: #2, #3, #4, #8)
  - [x] `NginxApiClient.SystemTextJson` references `NginxApiClient`
  - [x] `NginxApiClient.NewtonsoftJson` references `NginxApiClient`
  - [x] Test projects reference all source projects
  - [x] Add `InternalsVisibleTo` for test projects in core package
- [x] Task 7: Verify full solution builds (AC: #1)
  - [x] Run `dotnet build` and confirm zero errors
  - [x] Run `dotnet test` and confirm test projects are discovered

## Dev Notes

- **Architecture reference:** [Source: architecture.md#Starter Template & Project Initialization]
- **Solution structure follows:** `src/` for production, `tests/` for tests, `examples/` for samples
- **Critical:** Core package (`NginxApiClient`) must have ZERO external runtime dependencies
- **Critical:** `NginxApiClient.SystemTextJson` depends only on core + System.Text.Json
- **Critical:** `NginxApiClient.NewtonsoftJson` depends only on core + Newtonsoft.Json
- **Naming conventions:** [Source: architecture.md#Naming Patterns] — PascalCase for classes, _camelCase for private fields
- **Package tags:** `nginx-proxy-manager`, `npm-api`, `reverse-proxy`, `nginx`, `docker`
- **MIT License** file already exists in repository root
- **.NET 10 adjustment:** Target changed from `net10.0` to `net9.0` because .NET 10 SDK is not yet available (current SDK is 9.0.312). Will add `net10.0` target when SDK releases.

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

Claude Opus 4.6 (1M context)

### Debug Log References

- .NET 10 SDK not available — adjusted SystemTextJson targets from `net8.0;net10.0` to `net8.0;net9.0`
- Directory.Build.props repository URL auto-corrected to `otherdeniz/NginxApiClient` by hook

### Completion Notes List

- Solution scaffolded with 5 projects across 3 target frameworks
- Directory.Build.props configured with SourceLink, .snupkg, TreatWarningsAsErrors, XML docs
- Directory.Packages.props centralizes all package versions
- .editorconfig enforces C# naming conventions and formatting
- All projects build with zero warnings/errors
- Test infrastructure (xUnit + FluentAssertions) configured and discoverable
- Core package has zero external runtime dependencies
- InternalsVisibleTo configured for test and serializer projects

### File List

- NginxApiClient.sln (new)
- Directory.Build.props (new)
- Directory.Packages.props (new)
- .editorconfig (new)
- src/NginxApiClient/NginxApiClient.csproj (new)
- src/NginxApiClient/Placeholder.cs (new — temporary)
- src/NginxApiClient.SystemTextJson/NginxApiClient.SystemTextJson.csproj (new)
- src/NginxApiClient.SystemTextJson/Placeholder.cs (new — temporary)
- src/NginxApiClient.NewtonsoftJson/NginxApiClient.NewtonsoftJson.csproj (new)
- src/NginxApiClient.NewtonsoftJson/Placeholder.cs (new — temporary)
- tests/NginxApiClient.Tests/NginxApiClient.Tests.csproj (new)
- tests/NginxApiClient.IntegrationTests/NginxApiClient.IntegrationTests.csproj (new)

### Change Log

- 2026-04-17: Story 1.1 implemented — Solution scaffolding with multi-project structure, shared build props, central package management, code style enforcement
