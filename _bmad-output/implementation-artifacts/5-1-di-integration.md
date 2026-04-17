# Story 5.1: DI Integration via IServiceCollection

Status: review

## Story

As a developer,
I want to register NginxApiClient in my ASP.NET Core DI container with a single extension method call,
so that I can inject `INginxProxyManagerClient` into my services.

## Acceptance Criteria

1. `services.AddNginxApiClient(options => { ... })` registers all required services
2. `INginxProxyManagerClient` is resolvable from the DI container
3. `HttpClient` is registered via `IHttpClientFactory` with DelegatingHandler pipeline
4. The appropriate `IJsonSerializer` implementation is registered

## Tasks / Subtasks

- [x] Task 1: Create ServiceCollectionExtensions in SystemTextJson package (AC: #1-#4)
  - [x] `AddNginxApiClient(this IServiceCollection services, Action<NginxProxyManagerClientOptions> configure)`
  - [x] Register `SystemTextJsonSerializer` as `IJsonSerializer`
  - [x] Register `INginxProxyManagerClient` → `NginxProxyManagerClient`
  - [x] Register `HttpClient` via `AddHttpClient` with `AuthenticationDelegatingHandler` and `ErrorHandlingDelegatingHandler`
  - [x] Configure base address from options
- [x] Task 2: Create ServiceCollectionExtensions in NewtonsoftJson package (AC: #1-#4)
  - [x] Same pattern as SystemTextJson but registers `NewtonsoftJsonSerializer`
- [x] Task 3: Create NginxProxyManagerClient root implementation (AC: #2)
  - [x] Internal class implementing `INginxProxyManagerClient`
  - [x] Constructor receives `HttpClient`, `IJsonSerializer`
  - [x] Properties return per-resource client instances (ProxyHostClient, CertificateClient, etc.)
  - [x] Lazy initialization of resource clients
- [x] Task 4: Write DI integration tests (AC: #1-#4)
  - [x] Test service registration resolves `INginxProxyManagerClient`
  - [x] Test `ProxyHosts` and `Certificates` properties return non-null clients
  - [x] Test handler pipeline is correctly configured

## Dev Notes

- **Architecture reference:** [Source: architecture.md#HTTP Client Management]
- **DI pattern:** Use `AddHttpClient<INginxProxyManagerClient, NginxProxyManagerClient>()` with handler registration
- **Handler pipeline:** `AuthenticationDelegatingHandler` → `ErrorHandlingDelegatingHandler`
- **Both serializer packages** provide their own `ServiceCollectionExtensions` — same API surface, different serializer

### Project Structure Notes

- `src/NginxApiClient.SystemTextJson/ServiceCollectionExtensions.cs`
- `src/NginxApiClient.NewtonsoftJson/ServiceCollectionExtensions.cs`
- `src/NginxApiClient/Internal/NginxProxyManagerClient.cs`

### References

- [Source: architecture.md#HTTP Client Management]
- [Source: prd.md#FR40]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- `ServiceCollectionExtensions.AddNginxApiClient` implemented for both SystemTextJson and NewtonsoftJson packages; `NginxProxyManagerClient` root class wires up `AuthenticationDelegatingHandler` → `ErrorHandlingDelegatingHandler` pipeline via `IHttpClientFactory`; DI integration tests confirm all services resolve correctly.

### File List

### Change Log

| Date | Change | Author |
|------|--------|--------|
| 2026-04-17 | Story completed; status moved to review | Claude Opus 4.6 (1M context) |

