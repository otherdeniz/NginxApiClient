---
stepsCompleted: [step-01-init, step-02-context, step-03-starter, step-04-decisions, step-05-patterns, step-06-structure, step-07-validation, step-08-complete]
status: 'complete'
completedAt: '2026-04-17'
inputDocuments: [prd.md, product-brief-NginxApiClient.md, product-brief-NginxApiClient-distillate.md]
workflowType: 'architecture'
project_name: 'NginxApiClient'
user_name: 'Deniz Esen'
date: '2026-04-16'
---

# Architecture Decision Document

_This document builds collaboratively through step-by-step discovery. Sections are appended as we work through each architectural decision together._

## Project Context Analysis

### Requirements Overview

**Functional Requirements:**
48 FRs organized across 10 capability areas. The core pattern is REST API wrapping — 22 FRs (FR1–FR22) cover the MVP resources (Tokens, Proxy Hosts, Certificates), 12 FRs (FR23–FR34) cover Phase 2 resources sharing the same CRUD pattern, and 14 FRs (FR35–FR48) cover cross-cutting concerns (error handling, package architecture, async API, documentation).

The repetitive CRUD pattern across resource groups (list, get, create, update, delete, enable/disable) is the strongest architectural signal — it demands a shared abstraction to prevent code duplication across 8+ resource types.

**Non-Functional Requirements:**
- **Performance:** <50ms overhead per call, no idle resource consumption
- **Security:** Credentials/tokens never logged or persisted; no HTTPS downgrade
- **Integration:** `IHttpClientFactory` compatible, zero transitive dependencies in core, CLS-compliant
- **Reliability:** Thread-safe API calls, single token refresh under concurrency, no built-in retry (consumer controls)
- **Maintainability:** SemVer 2.0, XML doc coverage, per-target CI validation

**Scale & Complexity:**

- Primary domain: SDK/Client Library
- Complexity level: Low-medium
- Estimated architectural components: 5 (core abstractions, serialization layer, HTTP/auth layer, resource clients, DI integration)

### Technical Constraints & Dependencies

- .NET Standard 2.0 for core package (broadest compatibility)
- System.Text.Json for .NET 8/10 targets
- Newtonsoft.Json for .NET Framework 4.8 targets
- No runtime dependencies in core package beyond .NET Standard 2.0 BCL
- NPM API is undocumented — models must be derived from source code, community scripts, and live testing
- NPM API has no versioning — client must be tested against pinned Docker image tags

### Cross-Cutting Concerns Identified

1. **Serialization abstraction** — every request/response flows through serialization; the abstraction boundary between core and implementation packages is the most critical architectural decision
2. **JWT token lifecycle** — affects every API call; must be thread-safe, transparent, and handle both proactive refresh and reactive re-auth
3. **Error mapping** — HTTP errors must be consistently mapped to typed exceptions across all resource clients
4. **Multi-target framework support** — conditional compilation for nullable annotations, API differences between .NET Standard 2.0 and modern .NET
5. **Async pattern consistency** — all public API methods must follow the same async/CancellationToken pattern

## Starter Template & Project Initialization

### Primary Technology Domain

C# / .NET SDK library — multi-project NuGet solution initialized via `dotnet` CLI.

### Project Initialization Approach

No third-party starter template needed. The .NET SDK provides all scaffolding via `dotnet new`:

**Solution Structure:**

```
NginxApiClient/
├── src/
│   ├── NginxApiClient/                          # Core library (.NET Standard 2.0)
│   ├── NginxApiClient.SystemTextJson/           # System.Text.Json impl (.NET 8/10)
│   └── NginxApiClient.NewtonsoftJson/           # Newtonsoft.Json impl (.NET Std 2.0)
├── tests/
│   ├── NginxApiClient.Tests/                    # Unit tests
│   └── NginxApiClient.IntegrationTests/         # Integration tests (Dockerized NPM)
├── examples/
│   ├── BasicUsage/                              # .NET 8 console example
│   ├── CertificateManagement/                   # Certificate rotation example
│   └── LegacyFrameworkUsage/                    # .NET Framework 4.8 example
├── NginxApiClient.sln
├── Directory.Build.props                        # Shared build properties
├── Directory.Packages.props                     # Central package management
├── README.md
├── LICENSE
└── .github/
    └── workflows/
        ├── ci.yml                               # Build + test on PR
        └── publish.yml                           # NuGet publish on release
```

**Initialization Commands:**

```bash
dotnet new sln -n NginxApiClient
dotnet new classlib -n NginxApiClient -f netstandard2.0 -o src/NginxApiClient
dotnet new classlib -n NginxApiClient.SystemTextJson -o src/NginxApiClient.SystemTextJson
dotnet new classlib -n NginxApiClient.NewtonsoftJson -o src/NginxApiClient.NewtonsoftJson
dotnet new xunit -n NginxApiClient.Tests -o tests/NginxApiClient.Tests
dotnet new xunit -n NginxApiClient.IntegrationTests -o tests/NginxApiClient.IntegrationTests
dotnet new console -n BasicUsage -o examples/BasicUsage
```

**Architectural Decisions Provided by Initialization:**

- **Language & Runtime:** C# with .NET Standard 2.0 / .NET 8 / .NET 10 multi-targeting
- **Build Tooling:** MSBuild via `dotnet` CLI, `Directory.Build.props` for shared settings
- **Package Management:** Central Package Management via `Directory.Packages.props`
- **Testing Framework:** xUnit (industry standard for .NET libraries)
- **Code Organization:** `src/` for production code, `tests/` for tests, `examples/` for runnable samples
- **Development Experience:** `dotnet build`, `dotnet test`, `dotnet pack` workflow

**Note:** Project initialization using these commands should be the first implementation story.

## Core Architectural Decisions

### Decision Priority Analysis

**Critical Decisions (Block Implementation):**
1. Serialization abstraction pattern — Interface-based (`IJsonSerializer`)
2. Client interface design — Per-resource sub-interfaces
3. HTTP client management — Accept `HttpClient` directly
4. Token management — `DelegatingHandler` in HTTP pipeline

**Important Decisions (Shape Architecture):**
5. Model design — Strongly-typed request/response DTOs per endpoint
6. Error mapping — `DelegatingHandler` for consistent HTTP-to-exception mapping
7. NuGet multi-targeting — `Directory.Build.props` with conditional `<TargetFrameworks>`

**Deferred Decisions (Post-MVP):**
- Retry/resilience policies (consumer-controlled via Polly)
- Response caching strategy
- Logging abstraction (`Microsoft.Extensions.Logging` integration)

### Serialization Architecture

**Decision:** Interface-based serializer abstraction
**Pattern:** Core defines `IJsonSerializer` interface; implementation packages provide concrete implementations

```
NginxApiClient (core)
  └── IJsonSerializer { T Deserialize<T>(string json); string Serialize<T>(T value); }

NginxApiClient.SystemTextJson
  └── SystemTextJsonSerializer : IJsonSerializer

NginxApiClient.NewtonsoftJson
  └── NewtonsoftJsonSerializer : IJsonSerializer
```

**Rationale:** Established .NET pattern (Refit, RestSharp). Zero serializer dependency in core. Consumer chooses implementation at construction. Testable — mock `IJsonSerializer` for unit tests.

**Affects:** All packages, DI registration, client construction

### Client Interface Design

**Decision:** Per-resource sub-interfaces accessed via root client properties

```csharp
INginxProxyManagerClient
  ├── .ProxyHosts      → IProxyHostClient
  ├── .Certificates    → ICertificateClient
  ├── .RedirectionHosts → IRedirectionHostClient
  ├── .DeadHosts       → IDeadHostClient
  ├── .Streams         → IStreamClient
  ├── .AccessLists     → IAccessListClient
  ├── .Users           → IUserClient
  ├── .Settings        → ISettingsClient
  ├── .AuditLog        → IAuditLogClient
  └── .Reports         → IReportsClient
```

**Rationale:** 11 resource groups with 48+ methods would produce an unmanageable single interface. Per-resource interfaces keep IntelliSense focused, enable granular mocking, and allow Phase 2 resource groups to be added without touching existing interfaces.

**Affects:** All resource clients, consumer API, mocking patterns

### HTTP Client Management

**Decision:** Accept `HttpClient` directly

**Rationale:** Standard .NET pattern. Compatible with `IHttpClientFactory`. Consumer controls `HttpClient` lifetime and configuration. DI extension method registers via `IHttpClientFactory` with named client. Manual construction accepts `HttpClient` instance.

```csharp
// Via DI
services.AddNginxApiClient(options => {
    options.BaseUrl = "http://npm:81";
    options.Credentials = new("admin@example.com", "password");
});

// Manual construction
var client = new NginxProxyManagerClient(httpClient, serializer, options);
```

**Affects:** DI integration, client construction, `IHttpClientFactory` registration

### Token Management Architecture

**Decision:** `AuthenticationDelegatingHandler` in HTTP pipeline

**Design:**
- `AuthenticationDelegatingHandler : DelegatingHandler` intercepts all requests
- Adds `Authorization: Bearer {token}` header to outgoing requests
- On 401 response: acquires/refreshes token using `SemaphoreSlim(1,1)` for thread safety, retries the request once
- Token storage: in-memory only (NFR: never persisted)
- Proactive refresh: checks token expiry before sending request, refreshes if within threshold

**Rationale:** Separates authentication from business logic. Fully transparent to consumer code. Thread-safe via `SemaphoreSlim` — concurrent 401s trigger a single refresh, other callers wait and reuse the new token. Standard `DelegatingHandler` pattern integrates with `IHttpClientFactory`.

**Affects:** All API calls, `HttpClient` pipeline configuration, DI registration

### Error Handling Architecture

**Decision:** `ErrorHandlingDelegatingHandler` + typed exception hierarchy

**Pipeline order:** `AuthenticationDelegatingHandler` → `ErrorHandlingDelegatingHandler` → `HttpClientHandler`

**Exception mapping:**
- 401 (after re-auth attempt fails) → `NginxAuthenticationException`
- 404 → `NginxNotFoundException`
- 4xx/5xx → `NginxApiException` with HTTP status, NPM error detail parsed from response body
- Network failures → wrapped in `NginxApiException` with inner exception

**Affects:** All API calls, consumer error handling patterns

### NuGet Multi-Targeting Strategy

**Decision:** `Directory.Build.props` with per-project target frameworks

| Project | Target Frameworks |
|---------|-------------------|
| `NginxApiClient` | `netstandard2.0` |
| `NginxApiClient.SystemTextJson` | `net8.0;net10.0` |
| `NginxApiClient.NewtonsoftJson` | `netstandard2.0` |
| `NginxApiClient.Tests` | `net8.0` |
| `NginxApiClient.IntegrationTests` | `net8.0` |

**Shared properties via `Directory.Build.props`:**
- Package metadata (authors, license, repository URL, tags)
- SourceLink configuration
- Symbol package generation (`.snupkg`)
- Nullable reference types (enabled per-project where supported)
- `TreatWarningsAsErrors` for production code

### Decision Impact Analysis

**Implementation Sequence:**
1. Solution scaffolding + `Directory.Build.props` + Central Package Management
2. `IJsonSerializer` interface + serializer implementations
3. `DelegatingHandler` pipeline (auth + error handling)
4. `INginxProxyManagerClient` + per-resource sub-interfaces
5. Proxy Host client implementation (MVP resource #1)
6. Certificate client implementation (MVP resource #2)
7. DI extension methods
8. Phase 2 resource clients (follow established pattern)

**Cross-Component Dependencies:**
- All resource clients depend on `IJsonSerializer` and the `DelegatingHandler` pipeline
- DI registration depends on all client interfaces being defined
- Phase 2 clients follow the exact pattern established by Proxy Host + Certificate clients — no new architectural decisions needed

## Implementation Patterns & Consistency Rules

### Pattern Categories Defined

**Critical Conflict Points Identified:** 6 areas where AI agents could make inconsistent choices across resource clients, serialization implementations, and test code.

### Naming Patterns

**C# Code Naming (Framework Design Guidelines):**
- Classes/interfaces: `PascalCase` — `ProxyHostClient`, `INginxProxyManagerClient`
- Methods: `PascalCase` + `Async` suffix — `ListProxyHostsAsync`, `CreateCertificateAsync`
- Properties: `PascalCase` — `DomainNames`, `ForwardHost`, `ForwardPort`
- Parameters: `camelCase` — `proxyHostId`, `cancellationToken`
- Private fields: `_camelCase` — `_httpClient`, `_serializer`
- Constants: `PascalCase` — `DefaultBaseUrl`, `TokenRefreshThreshold`

**API Model Naming:**
- Request models: `Create{Resource}Request`, `Update{Resource}Request`
- Response models: `{Resource}Response` (maps to NPM JSON response)
- List responses: `IReadOnlyList<{Resource}Response>` (no wrapper type)
- NPM JSON properties use `snake_case` — models use `PascalCase` C# properties with serializer-specific attributes handled in implementation packages

**File Naming:**
- One public type per file, file name matches type name: `ProxyHostClient.cs`, `IProxyHostClient.cs`
- Interfaces prefixed with `I`: `IJsonSerializer.cs`, `ICertificateClient.cs`
- Exception classes: `NginxApiException.cs`, `NginxAuthenticationException.cs`

### Structure Patterns

**Project Organization:**
- `src/{PackageName}/` — production code only
- `tests/{TestProjectName}/` — test code only
- `examples/{ExampleName}/` — runnable examples
- No shared `Utils` or `Helpers` folders — domain-specific placement

**Namespace Structure:**
```
NginxApiClient                           # Root: interfaces, options, exceptions
NginxApiClient.Models                    # Request/response DTOs
NginxApiClient.Models.ProxyHosts         # Per-resource model subfolder
NginxApiClient.Models.Certificates       # Per-resource model subfolder
NginxApiClient.Internal                  # Internal implementation (not public API)
NginxApiClient.SystemTextJson            # Serializer implementation
NginxApiClient.NewtonsoftJson            # Serializer implementation
```

**Resource Client Pattern (template for all 11 resources):**
```csharp
// Interface in core package
public interface IProxyHostClient
{
    Task<IReadOnlyList<ProxyHostResponse>> ListAsync(CancellationToken ct = default);
    Task<ProxyHostResponse> GetAsync(int id, CancellationToken ct = default);
    Task<ProxyHostResponse> CreateAsync(CreateProxyHostRequest request, CancellationToken ct = default);
    Task<ProxyHostResponse> UpdateAsync(int id, UpdateProxyHostRequest request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task EnableAsync(int id, CancellationToken ct = default);
    Task DisableAsync(int id, CancellationToken ct = default);
}

// Implementation in core package (internal)
internal class ProxyHostClient : IProxyHostClient { ... }
```

### Format Patterns

**NPM API JSON ↔ C# Model Mapping:**
- NPM uses `snake_case` JSON (`domain_names`, `forward_host`)
- C# models use `PascalCase` properties (`DomainNames`, `ForwardHost`)
- Mapping is handled by serializer configuration in implementation packages (not per-property attributes in core)
- Core models have zero serializer attributes — they are POCOs

**Error Response Parsing:**
- NPM error responses parsed into `NginxApiException.ErrorDetail` property
- Error detail includes: `Message` (string), `StatusCode` (int), `RawResponse` (string, for debugging)
- Exception `ToString()` includes status code and message, never credentials

**ID Types:**
- All NPM resource IDs are `int` (NPM uses integer primary keys)
- Consistent across all resource clients: `GetAsync(int id, ...)`, `DeleteAsync(int id, ...)`

### Process Patterns

**Async Method Pattern (all public methods follow this):**
```csharp
public async Task<TResponse> MethodAsync(TRequest request, CancellationToken cancellationToken = default)
{
    // 1. Validate arguments (ArgumentNullException for null request)
    // 2. Build HttpRequestMessage
    // 3. Send via _httpClient (delegating handlers handle auth + errors)
    // 4. Deserialize response via _serializer
    // 5. Return typed result
}
```

**Constructor Pattern (all resource clients):**
```csharp
internal ProxyHostClient(HttpClient httpClient, IJsonSerializer serializer)
{
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
}
```

**Test Naming Convention:**
```
{MethodName}_{Scenario}_{ExpectedBehavior}
```
Examples: `ListAsync_ReturnsProxyHosts_WhenHostsExist`, `CreateAsync_ThrowsNginxApiException_WhenDomainNamesEmpty`

**Test Organization:**
- One test class per resource client: `ProxyHostClientTests.cs`, `CertificateClientTests.cs`
- Integration tests tagged with `[Trait("Category", "Integration")]`
- Unit tests use mocked `HttpClient` via `MockHttpMessageHandler`

### Enforcement Guidelines

**All AI Agents MUST:**

- Follow the resource client template pattern exactly when implementing new resource clients
- Use `PascalCase` for all C# identifiers, `_camelCase` for private fields
- Place all model types in `NginxApiClient.Models.{ResourceName}` namespace
- End all async methods with `Async` suffix and accept `CancellationToken cancellationToken = default`
- Never add serializer-specific attributes to core model types
- Use `internal` access for implementation classes, `public` only for interfaces and models
- Validate non-nullable parameters with `ArgumentNullException` in constructors

**Anti-Patterns (DO NOT):**

- Do not create `BaseClient` or `AbstractResourceClient` — use composition, not inheritance
- Do not add `Newtonsoft.Json` or `System.Text.Json` attributes to models in the core package
- Do not create `Task<ApiResponse<T>>` wrapper types — return `Task<T>` directly, errors are exceptions
- Do not implement retry logic in the library — consumers control retry via Polly
- Do not create `async void` methods — always `async Task` or `async Task<T>`

## Project Structure & Boundaries

### Complete Project Directory Structure

```
NginxApiClient/
├── NginxApiClient.sln
├── Directory.Build.props                              # Shared MSBuild properties
├── Directory.Packages.props                           # Central Package Management
├── .editorconfig                                      # Code style enforcement
├── .gitignore
├── README.md
├── LICENSE
│
├── .github/
│   └── workflows/
│       ├── ci.yml                                     # Build + test on PR/push
│       └── publish.yml                                # NuGet publish on release tag
│
├── src/
│   ├── NginxApiClient/                                # Core package (netstandard2.0)
│   │   ├── NginxApiClient.csproj
│   │   │
│   │   ├── INginxProxyManagerClient.cs                # Root client interface
│   │   ├── NginxProxyManagerClientOptions.cs           # Configuration options
│   │   ├── IJsonSerializer.cs                         # Serialization abstraction
│   │   │
│   │   ├── Clients/                                   # Per-resource client interfaces
│   │   │   ├── IProxyHostClient.cs                    # FR6–FR15
│   │   │   ├── ICertificateClient.cs                  # FR16–FR22
│   │   │   ├── IRedirectionHostClient.cs              # FR23–FR24
│   │   │   ├── IDeadHostClient.cs                     # FR25–FR26
│   │   │   ├── IStreamClient.cs                       # FR27–FR28
│   │   │   ├── IAccessListClient.cs                   # FR29
│   │   │   ├── IUserClient.cs                         # FR30–FR31
│   │   │   ├── ISettingsClient.cs                     # FR32
│   │   │   ├── IAuditLogClient.cs                     # FR33
│   │   │   └── IReportsClient.cs                      # FR34
│   │   │
│   │   ├── Models/
│   │   │   ├── ProxyHosts/
│   │   │   │   ├── ProxyHostResponse.cs
│   │   │   │   ├── CreateProxyHostRequest.cs
│   │   │   │   ├── UpdateProxyHostRequest.cs
│   │   │   │   └── ProxyHostLocation.cs
│   │   │   ├── Certificates/
│   │   │   │   ├── CertificateResponse.cs
│   │   │   │   ├── CreateCertificateRequest.cs
│   │   │   │   └── UploadCertificateRequest.cs
│   │   │   ├── Tokens/
│   │   │   │   ├── TokenRequest.cs
│   │   │   │   └── TokenResponse.cs
│   │   │   ├── RedirectionHosts/
│   │   │   ├── DeadHosts/
│   │   │   ├── Streams/
│   │   │   ├── AccessLists/
│   │   │   ├── Users/
│   │   │   ├── Settings/
│   │   │   ├── AuditLog/
│   │   │   └── Reports/
│   │   │
│   │   ├── Exceptions/                                # FR35–FR37
│   │   │   ├── NginxApiException.cs
│   │   │   ├── NginxAuthenticationException.cs
│   │   │   └── NginxNotFoundException.cs
│   │   │
│   │   └── Internal/                                  # Internal implementation
│   │       ├── NginxProxyManagerClient.cs              # Root client implementation
│   │       ├── ProxyHostClient.cs
│   │       ├── CertificateClient.cs
│   │       ├── RedirectionHostClient.cs
│   │       ├── DeadHostClient.cs
│   │       ├── StreamClient.cs
│   │       ├── AccessListClient.cs
│   │       ├── UserClient.cs
│   │       ├── SettingsClient.cs
│   │       ├── AuditLogClient.cs
│   │       ├── ReportsClient.cs
│   │       ├── AuthenticationDelegatingHandler.cs      # FR1–FR4
│   │       ├── ErrorHandlingDelegatingHandler.cs       # FR35–FR37
│   │       └── TokenStore.cs                          # Thread-safe token storage
│   │
│   ├── NginxApiClient.SystemTextJson/                 # STJ package (net8.0;net10.0)
│   │   ├── NginxApiClient.SystemTextJson.csproj
│   │   ├── SystemTextJsonSerializer.cs                # FR39
│   │   └── ServiceCollectionExtensions.cs             # FR40
│   │
│   └── NginxApiClient.NewtonsoftJson/                 # Newtonsoft package (netstandard2.0)
│       ├── NginxApiClient.NewtonsoftJson.csproj
│       ├── NewtonsoftJsonSerializer.cs                # FR39
│       └── ServiceCollectionExtensions.cs             # FR40
│
├── tests/
│   ├── NginxApiClient.Tests/                          # Unit tests (net8.0)
│   │   ├── NginxApiClient.Tests.csproj
│   │   ├── Clients/
│   │   │   ├── ProxyHostClientTests.cs
│   │   │   ├── CertificateClientTests.cs
│   │   │   └── ...
│   │   ├── Handlers/
│   │   │   ├── AuthenticationDelegatingHandlerTests.cs
│   │   │   └── ErrorHandlingDelegatingHandlerTests.cs
│   │   ├── Serialization/
│   │   │   ├── SystemTextJsonSerializerTests.cs
│   │   │   └── NewtonsoftJsonSerializerTests.cs
│   │   └── Helpers/
│   │       └── MockHttpMessageHandler.cs
│   │
│   └── NginxApiClient.IntegrationTests/               # Integration tests (net8.0)
│       ├── NginxApiClient.IntegrationTests.csproj
│       ├── docker-compose.yml                         # NPM instance for testing
│       ├── NpmFixture.cs                              # Shared NPM test fixture
│       ├── ProxyHostIntegrationTests.cs
│       └── CertificateIntegrationTests.cs
│
└── examples/
    ├── BasicUsage/                                    # FR46, FR47
    │   ├── BasicUsage.csproj                          # net8.0 console app
    │   └── Program.cs
    ├── CertificateManagement/
    │   ├── CertificateManagement.csproj
    │   └── Program.cs
    └── LegacyFrameworkUsage/
        ├── LegacyFrameworkUsage.csproj                # net48 console app
        └── Program.cs
```

### Architectural Boundaries

**Package Boundaries (NuGet packages = hard boundaries):**

| Package | Depends On | Public Surface |
|---------|-----------|----------------|
| `NginxApiClient` | Nothing (zero deps) | Interfaces, models, exceptions, options |
| `NginxApiClient.SystemTextJson` | `NginxApiClient`, `System.Text.Json` | `SystemTextJsonSerializer`, `ServiceCollectionExtensions` |
| `NginxApiClient.NewtonsoftJson` | `NginxApiClient`, `Newtonsoft.Json` | `NewtonsoftJsonSerializer`, `ServiceCollectionExtensions` |

**Internal vs Public Boundary:**
- `public`: All interfaces (`I*Client`), all models (`*Request`, `*Response`), exceptions, options
- `internal`: All implementation classes (`*Client` concrete, `*DelegatingHandler`, `TokenStore`)
- Implementation packages use `[InternalsVisibleTo]` for test projects only

**HTTP Pipeline Boundary:**
```
Consumer Code → INginxProxyManagerClient
                    ↓
              Internal Client → HttpClient
                                   ↓
                         AuthenticationDelegatingHandler (adds Bearer token)
                                   ↓
                         ErrorHandlingDelegatingHandler (maps errors to exceptions)
                                   ↓
                              HttpClientHandler (actual HTTP call)
```

### Requirements to Structure Mapping

| FR Range | Capability | Directory |
|----------|-----------|-----------|
| FR1–FR5 | Authentication & tokens | `Internal/AuthenticationDelegatingHandler.cs`, `Internal/TokenStore.cs`, `Models/Tokens/` |
| FR6–FR15 | Proxy host management | `Clients/IProxyHostClient.cs`, `Internal/ProxyHostClient.cs`, `Models/ProxyHosts/` |
| FR16–FR22 | Certificate management | `Clients/ICertificateClient.cs`, `Internal/CertificateClient.cs`, `Models/Certificates/` |
| FR23–FR34 | Phase 2 resources | `Clients/I{Resource}Client.cs`, `Internal/{Resource}Client.cs`, `Models/{Resource}/` |
| FR35–FR37 | Error handling | `Exceptions/`, `Internal/ErrorHandlingDelegatingHandler.cs` |
| FR38–FR39 | Package architecture | Solution structure, `.csproj` files |
| FR40–FR41 | DI integration | `ServiceCollectionExtensions.cs` in serializer packages |
| FR42 | Testability | `INginxProxyManagerClient.cs` + per-resource interfaces |
| FR43–FR44 | Async API | All `I*Client` interfaces (pattern enforcement) |
| FR45 | XML docs | All public types (enforced by `TreatWarningsAsErrors`) |
| FR46–FR47 | Documentation & examples | `examples/`, `README.md` |
| FR48 | Symbol packages | `.csproj` SourceLink config, `Directory.Build.props` |

### Data Flow

```
Consumer → NginxProxyManagerClient.ProxyHosts.CreateAsync(request)
    → ProxyHostClient.CreateAsync(request)
        → Validate request (ArgumentNullException)
        → Build HttpRequestMessage (POST /api/nginx/proxy-hosts)
        → Serialize request body via IJsonSerializer
        → Send via HttpClient
            → AuthenticationDelegatingHandler adds Bearer header
            → ErrorHandlingDelegatingHandler checks response
                → 2xx: return HttpResponseMessage
                → 401: re-auth, retry once
                → 4xx/5xx: throw NginxApiException
        → Deserialize response body via IJsonSerializer
    → Return ProxyHostResponse to consumer
```

## Architecture Validation Results

### Coherence Validation

**Decision Compatibility:** All decisions are mutually consistent. Interface-based serializer + per-resource sub-interfaces + DelegatingHandler pipeline form a clean, layered architecture with no conflicts. .NET Standard 2.0 core + multi-target serializer packages is a well-established pattern.

**Pattern Consistency:** All patterns align. PascalCase naming throughout, resource client template is consistent across all 11 resources, async/CancellationToken pattern enforced on all public methods, test naming convention is uniform.

**Structure Alignment:** Directory structure maps cleanly to architectural decisions. Package boundaries match NuGet package structure, internal vs public boundary is clear, FR-to-directory mapping is complete.

### Requirements Coverage Validation

**All 48 Functional Requirements are architecturally supported:**

| FR Range | Coverage | Architecture Component |
|----------|----------|----------------------|
| FR1–FR5 | Covered | `AuthenticationDelegatingHandler` + `TokenStore` |
| FR6–FR15 | Covered | `IProxyHostClient` + models + internal impl |
| FR16–FR22 | Covered | `ICertificateClient` + models + internal impl |
| FR23–FR34 | Covered | Per-resource interfaces (Phase 2, same pattern) |
| FR35–FR37 | Covered | `ErrorHandlingDelegatingHandler` + exception hierarchy |
| FR38–FR42 | Covered | Multi-project solution + interfaces |
| FR43–FR44 | Covered | Async pattern enforced on all methods |
| FR45–FR48 | Covered | XML docs, examples, .snupkg |

**All Non-Functional Requirements addressed:**
- Performance: no idle threads, minimal serialization overhead
- Security: credentials in-memory only, never in exceptions
- Integration: `IHttpClientFactory` compatible, zero core deps
- Reliability: `SemaphoreSlim` for thread-safe token refresh
- Maintainability: SemVer, XML docs, CI matrix builds

### Implementation Readiness Validation

**Decision Completeness:** All critical decisions documented with rationale and code examples. No ambiguity for implementing agents.

**Structure Completeness:** Every file and directory specified with FR traceability. No undefined integration points.

**Pattern Completeness:** All conflict points addressed. Anti-patterns explicitly listed. Resource client template provides copy-paste consistency.

### Gap Analysis Results

**No critical gaps found.**

**Minor gaps (non-blocking, resolved during implementation):**
- Integration test NPM Docker image version — pinned during first integration test setup
- Exact NPM API JSON property names — discovered via reverse-engineering during model implementation

### Architecture Completeness Checklist

- [x] Project context thoroughly analyzed
- [x] Scale and complexity assessed
- [x] Technical constraints identified
- [x] Cross-cutting concerns mapped
- [x] Critical decisions documented with versions
- [x] Technology stack fully specified
- [x] Integration patterns defined
- [x] Performance considerations addressed
- [x] Naming conventions established
- [x] Structure patterns defined
- [x] Process patterns documented
- [x] Complete directory structure defined
- [x] Component boundaries established
- [x] Requirements to structure mapping complete

### Architecture Readiness Assessment

**Overall Status:** READY FOR IMPLEMENTATION

**Confidence Level:** High

**Key Strengths:**
- Clean separation of concerns via multi-package architecture
- Zero dependencies in core package
- Well-defined resource client template enables rapid Phase 2 expansion
- DelegatingHandler pipeline provides transparent cross-cutting concerns
- Complete FR-to-directory traceability

**Areas for Future Enhancement:**
- Logging abstraction (`Microsoft.Extensions.Logging`) — post-MVP
- Retry/resilience integration guidance (Polly) — post-MVP
- NPM version compatibility matrix — built during testing

### Implementation Handoff

**AI Agent Guidelines:**
- Follow all architectural decisions exactly as documented
- Use the resource client template pattern for all resource implementations
- Respect package boundaries — no serializer references in core
- Use `internal` for all implementation classes
- Follow naming conventions and anti-pattern list strictly

**First Implementation Priority:**
```bash
dotnet new sln -n NginxApiClient
dotnet new classlib -n NginxApiClient -f netstandard2.0 -o src/NginxApiClient
dotnet new classlib -n NginxApiClient.SystemTextJson -o src/NginxApiClient.SystemTextJson
dotnet new classlib -n NginxApiClient.NewtonsoftJson -o src/NginxApiClient.NewtonsoftJson
dotnet new xunit -n NginxApiClient.Tests -o tests/NginxApiClient.Tests
dotnet new xunit -n NginxApiClient.IntegrationTests -o tests/NginxApiClient.IntegrationTests
```
