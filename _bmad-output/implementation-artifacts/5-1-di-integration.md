# Story 5.1: DI Integration via IServiceCollection

Status: ready-for-dev

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

- [ ] Task 1: Create ServiceCollectionExtensions in SystemTextJson package (AC: #1-#4)
  - [ ] `AddNginxApiClient(this IServiceCollection services, Action<NginxProxyManagerClientOptions> configure)`
  - [ ] Register `SystemTextJsonSerializer` as `IJsonSerializer`
  - [ ] Register `INginxProxyManagerClient` → `NginxProxyManagerClient`
  - [ ] Register `HttpClient` via `AddHttpClient` with `AuthenticationDelegatingHandler` and `ErrorHandlingDelegatingHandler`
  - [ ] Configure base address from options
- [ ] Task 2: Create ServiceCollectionExtensions in NewtonsoftJson package (AC: #1-#4)
  - [ ] Same pattern as SystemTextJson but registers `NewtonsoftJsonSerializer`
- [ ] Task 3: Create NginxProxyManagerClient root implementation (AC: #2)
  - [ ] Internal class implementing `INginxProxyManagerClient`
  - [ ] Constructor receives `HttpClient`, `IJsonSerializer`
  - [ ] Properties return per-resource client instances (ProxyHostClient, CertificateClient, etc.)
  - [ ] Lazy initialization of resource clients
- [ ] Task 4: Write DI integration tests (AC: #1-#4)
  - [ ] Test service registration resolves `INginxProxyManagerClient`
  - [ ] Test `ProxyHosts` and `Certificates` properties return non-null clients
  - [ ] Test handler pipeline is correctly configured

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

### Debug Log References

### Completion Notes List

### File List

