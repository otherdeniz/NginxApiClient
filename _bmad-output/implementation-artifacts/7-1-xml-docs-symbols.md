# Story 7.1: XML Doc Comments & Symbol Packages

Status: review

## Story

As a developer,
I want IntelliSense documentation on every public type, method, and property, and symbol packages for debugging,
so that I can discover and understand the API without leaving my IDE.

## Acceptance Criteria

1. Every public type has `<summary>` XML doc comments
2. Every public method has `<param>`, `<returns>`, and `<exception>` doc comments
3. `TreatWarningsAsErrors` causes build failures for missing docs
4. `.snupkg` symbol packages are generated with SourceLink configured
5. Consumers can step into library source code during debugging

## Tasks / Subtasks

- [x] Task 1: Audit and complete XML doc comments on all public types (AC: #1, #2)
  - [x] All interfaces (`INginxProxyManagerClient`, `I*Client`, `IJsonSerializer`)
  - [x] All model classes (request/response DTOs)
  - [x] All exception classes
  - [x] `NginxProxyManagerClientOptions`
  - [x] `ServiceCollectionExtensions` methods
  - [x] Ensure `<exception>` tags list thrown exception types
- [x] Task 2: Enable TreatWarningsAsErrors for XML docs (AC: #3)
  - [x] Verify `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` in Directory.Build.props
  - [x] Verify `<GenerateDocumentationFile>true</GenerateDocumentationFile>`
  - [x] Fix any remaining doc warnings
- [x] Task 3: Configure SourceLink and symbol packages (AC: #4, #5)
  - [x] Add `Microsoft.SourceLink.GitHub` package
  - [x] Enable `<IncludeSymbols>true</IncludeSymbols>` and `<SymbolPackageFormat>snupkg</SymbolPackageFormat>`
  - [x] Verify `dotnet pack` generates `.snupkg` files
  - [x] Test source stepping works in a consuming project

## Dev Notes

- **Many XML docs should already exist** from earlier stories — this story audits for completeness
- **SourceLink** enables consumers to step into NginxApiClient source code during debugging
- **snupkg** is the modern symbol package format for NuGet.org

### References

- [Source: prd.md#FR45, FR48]
- [Source: architecture.md#NuGet Package Requirements]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- All XML doc comments audited and completed across all public types, methods, and properties. TreatWarningsAsErrors verified in Directory.Build.props. SourceLink and snupkg symbol packages configured and validated.

### Change Log

- 2026-04-17: Story completed and moved to review status.

### File List

