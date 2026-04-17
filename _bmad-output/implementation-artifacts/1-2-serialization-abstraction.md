# Story 1.2: Serialization Abstraction & Implementations

Status: review

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

- [x] Task 1: Create IJsonSerializer interface in core package (AC: #1, #2)
  - [x] Define `T Deserialize<T>(string json)` method
  - [x] Define `string Serialize<T>(T value)` method
  - [x] Add XML doc comments on interface and methods
  - [x] Place in root namespace `NginxApiClient`
- [x] Task 2: Create SystemTextJsonSerializer (AC: #3, #6)
  - [x] Implement `IJsonSerializer` in `NginxApiClient.SystemTextJson` package
  - [x] Configure `JsonSerializerOptions` with `JsonNamingPolicy.SnakeCaseLower`
  - [x] Handle null deserialization gracefully
  - [x] Add XML doc comments
- [x] Task 3: Create NewtonsoftJsonSerializer (AC: #4, #6)
  - [x] Implement `IJsonSerializer` in `NginxApiClient.NewtonsoftJson` package
  - [x] Configure `JsonSerializerSettings` with `SnakeCaseNamingStrategy`
  - [x] Handle null deserialization gracefully
  - [x] Add XML doc comments
- [x] Task 4: Write serialization tests (AC: #5)
  - [x] Test round-trip serialization for both implementations
  - [x] Test `PascalCase` → `snake_case` property mapping
  - [x] Test that both serializers produce identical output
  - [x] Test null/empty input handling
  - [x] Test with a sample model class (SerializerTestDto)

## Dev Notes

- **Architecture reference:** [Source: architecture.md#Serialization Architecture]
- **Both serializers support custom options** via constructor overload for consumer flexibility
- **Both serializers omit null properties** by default (matching NPM API behavior)
- **Thread safety:** Both implementations are thread-safe (stateless after construction, options are immutable)

### References

- [Source: architecture.md#Serialization Architecture]
- [Source: architecture.md#Format Patterns]
- [Source: prd.md#FR38, FR39]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

- Fixed missing `using Xunit;` in test files (ImplicitUsings doesn't include xUnit)
- Fixed test project TreatWarningsAsErrors by adding explicit overrides in csproj

### Completion Notes List

- `IJsonSerializer` interface created with `Serialize<T>` and `Deserialize<T>` methods
- `SystemTextJsonSerializer` uses `JsonNamingPolicy.SnakeCaseLower` and `PropertyNameCaseInsensitive`
- `NewtonsoftJsonSerializer` uses `DefaultContractResolver` with `SnakeCaseNamingStrategy`
- Both omit null properties by default
- Both support custom options via constructor overload
- 15 tests passing: 6 per serializer + 3 cross-compatibility tests
- Verified both serializers produce cross-compatible JSON output

### File List

- src/NginxApiClient/IJsonSerializer.cs (new)
- src/NginxApiClient/Placeholder.cs (deleted)
- src/NginxApiClient.SystemTextJson/SystemTextJsonSerializer.cs (new)
- src/NginxApiClient.SystemTextJson/Placeholder.cs (deleted)
- src/NginxApiClient.NewtonsoftJson/NewtonsoftJsonSerializer.cs (new)
- src/NginxApiClient.NewtonsoftJson/Placeholder.cs (deleted)
- tests/NginxApiClient.Tests/Serialization/SerializerTestDto.cs (new)
- tests/NginxApiClient.Tests/Serialization/SystemTextJsonSerializerTests.cs (new)
- tests/NginxApiClient.Tests/Serialization/NewtonsoftJsonSerializerTests.cs (new)
- tests/NginxApiClient.Tests/Serialization/SerializerCompatibilityTests.cs (new)
- tests/NginxApiClient.Tests/NginxApiClient.Tests.csproj (modified — added TreatWarningsAsErrors override)
- tests/NginxApiClient.IntegrationTests/NginxApiClient.IntegrationTests.csproj (modified — added TreatWarningsAsErrors override)

### Change Log

- 2026-04-17: Story 1.2 implemented — IJsonSerializer interface + SystemTextJson and NewtonsoftJson implementations with 15 passing tests
