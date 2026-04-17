# Story 1.2: Serialization Abstraction & Implementations

Status: ready-for-dev

## Story

As a developer,
I want an `IJsonSerializer` interface with System.Text.Json and Newtonsoft.Json implementations,
so that I can choose my preferred serializer without the core package imposing a dependency.

## Acceptance Criteria

1. Core `NginxApiClient` package has zero external runtime dependencies
2. `IJsonSerializer` defines `T Deserialize<T>(string json)` and `string Serialize<T>(T value)` methods
3. `SystemTextJsonSerializer` implements `IJsonSerializer` with `snake_case` JSON property naming
4. `NewtonsoftJsonSerializer` implements `IJsonSerializer` with `snake_case` JSON property naming
5. Both serializers produce identical JSON output for the same input object
6. Both serializers handle `PascalCase` C# properties to `snake_case` JSON mapping

## Tasks / Subtasks

- [ ] Task 1: Create IJsonSerializer interface in core package (AC: #1, #2)
  - [ ] Define `T Deserialize<T>(string json)` method
  - [ ] Define `string Serialize<T>(T value)` method
  - [ ] Add XML doc comments on interface and methods
  - [ ] Place in root namespace `NginxApiClient`
- [ ] Task 2: Create SystemTextJsonSerializer (AC: #3, #6)
  - [ ] Implement `IJsonSerializer` in `NginxApiClient.SystemTextJson` package
  - [ ] Configure `JsonSerializerOptions` with `JsonNamingPolicy.SnakeCaseLower`
  - [ ] Handle null deserialization gracefully
  - [ ] Add XML doc comments
- [ ] Task 3: Create NewtonsoftJsonSerializer (AC: #4, #6)
  - [ ] Implement `IJsonSerializer` in `NginxApiClient.NewtonsoftJson` package
  - [ ] Configure `JsonSerializerSettings` with `SnakeCaseNamingStrategy`
  - [ ] Handle null deserialization gracefully
  - [ ] Add XML doc comments
- [ ] Task 4: Write serialization tests (AC: #5)
  - [ ] Test round-trip serialization for both implementations
  - [ ] Test `PascalCase` → `snake_case` property mapping
  - [ ] Test that both serializers produce identical output
  - [ ] Test null/empty input handling
  - [ ] Test with a sample model class (e.g., test DTO with various property types)

## Dev Notes

- **Architecture reference:** [Source: architecture.md#Serialization Architecture]
- **Pattern:** Core defines abstraction, implementation packages provide concrete serializers
- **Critical:** No serializer-specific attributes on core models — mapping handled by serializer configuration
- **snake_case mapping:** NPM API uses `snake_case` JSON (`domain_names`, `forward_host`), C# models use `PascalCase`
- **System.Text.Json:** Use `JsonNamingPolicy.SnakeCaseLower` (available in .NET 8+)
- **Newtonsoft.Json:** Use `DefaultContractResolver` with `SnakeCaseNamingStrategy`
- **Thread safety:** Both serializer implementations should be thread-safe (stateless after construction)

### Project Structure Notes

- `src/NginxApiClient/IJsonSerializer.cs`
- `src/NginxApiClient.SystemTextJson/SystemTextJsonSerializer.cs`
- `src/NginxApiClient.NewtonsoftJson/NewtonsoftJsonSerializer.cs`
- `tests/NginxApiClient.Tests/Serialization/SystemTextJsonSerializerTests.cs`
- `tests/NginxApiClient.Tests/Serialization/NewtonsoftJsonSerializerTests.cs`

### References

- [Source: architecture.md#Serialization Architecture]
- [Source: architecture.md#Format Patterns]
- [Source: prd.md#FR38, FR39]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

