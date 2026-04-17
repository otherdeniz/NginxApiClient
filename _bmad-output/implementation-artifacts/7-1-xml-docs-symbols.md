# Story 7.1: XML Doc Comments & Symbol Packages

Status: ready-for-dev

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

- [ ] Task 1: Audit and complete XML doc comments on all public types (AC: #1, #2)
  - [ ] All interfaces (`INginxProxyManagerClient`, `I*Client`, `IJsonSerializer`)
  - [ ] All model classes (request/response DTOs)
  - [ ] All exception classes
  - [ ] `NginxProxyManagerClientOptions`
  - [ ] `ServiceCollectionExtensions` methods
  - [ ] Ensure `<exception>` tags list thrown exception types
- [ ] Task 2: Enable TreatWarningsAsErrors for XML docs (AC: #3)
  - [ ] Verify `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` in Directory.Build.props
  - [ ] Verify `<GenerateDocumentationFile>true</GenerateDocumentationFile>`
  - [ ] Fix any remaining doc warnings
- [ ] Task 3: Configure SourceLink and symbol packages (AC: #4, #5)
  - [ ] Add `Microsoft.SourceLink.GitHub` package
  - [ ] Enable `<IncludeSymbols>true</IncludeSymbols>` and `<SymbolPackageFormat>snupkg</SymbolPackageFormat>`
  - [ ] Verify `dotnet pack` generates `.snupkg` files
  - [ ] Test source stepping works in a consuming project

## Dev Notes

- **Many XML docs should already exist** from earlier stories — this story audits for completeness
- **SourceLink** enables consumers to step into NginxApiClient source code during debugging
- **snupkg** is the modern symbol package format for NuGet.org

### References

- [Source: prd.md#FR45, FR48]
- [Source: architecture.md#NuGet Package Requirements]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

