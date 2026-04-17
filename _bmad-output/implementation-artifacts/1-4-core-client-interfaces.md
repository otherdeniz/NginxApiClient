# Story 1.4: Core Client Interfaces

Status: ready-for-dev

## Story

As a developer,
I want the `INginxProxyManagerClient` root interface with per-resource sub-interface properties,
so that I can access typed API methods with focused IntelliSense per resource group.

## Acceptance Criteria

1. `INginxProxyManagerClient` exposes properties returning per-resource sub-interfaces
2. MVP interfaces defined: `IProxyHostClient`, `ICertificateClient`
3. Phase 2 interfaces defined: `IRedirectionHostClient`, `IDeadHostClient`, `IStreamClient`, `IAccessListClient`, `IUserClient`, `ISettingsClient`, `IAuditLogClient`, `IReportsClient`
4. All methods return `Task<T>` with `CancellationToken cancellationToken = default`
5. `IProxyHostClient` defines: `ListAsync`, `GetAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `EnableAsync`, `DisableAsync`
6. `ICertificateClient` defines: `ListAsync`, `GetAsync`, `CreateAsync`, `DeleteAsync`, `UploadAsync`, `DownloadAsync`, `RenewAsync`

## Tasks / Subtasks

- [ ] Task 1: Create INginxProxyManagerClient root interface (AC: #1)
  - [ ] Property `IProxyHostClient ProxyHosts { get; }`
  - [ ] Property `ICertificateClient Certificates { get; }`
  - [ ] Phase 2 properties: `RedirectionHosts`, `DeadHosts`, `Streams`, `AccessLists`, `Users`, `Settings`, `AuditLog`, `Reports`
  - [ ] XML doc comments on interface and all properties
- [ ] Task 2: Create IProxyHostClient interface (AC: #4, #5)
  - [ ] `Task<IReadOnlyList<ProxyHostResponse>> ListAsync(CancellationToken ct = default)`
  - [ ] `Task<ProxyHostResponse> GetAsync(int id, CancellationToken ct = default)`
  - [ ] `Task<ProxyHostResponse> CreateAsync(CreateProxyHostRequest request, CancellationToken ct = default)`
  - [ ] `Task<ProxyHostResponse> UpdateAsync(int id, UpdateProxyHostRequest request, CancellationToken ct = default)`
  - [ ] `Task DeleteAsync(int id, CancellationToken ct = default)`
  - [ ] `Task EnableAsync(int id, CancellationToken ct = default)`
  - [ ] `Task DisableAsync(int id, CancellationToken ct = default)`
  - [ ] XML doc comments with `<exception>` tags
- [ ] Task 3: Create ICertificateClient interface (AC: #4, #6)
  - [ ] List, Get, Create, Delete, Upload, Download, Renew methods
  - [ ] XML doc comments with `<exception>` tags
- [ ] Task 4: Create Phase 2 resource interfaces (AC: #3, #4)
  - [ ] `IRedirectionHostClient` — CRUD + enable/disable
  - [ ] `IDeadHostClient` — CRUD + enable/disable
  - [ ] `IStreamClient` — CRUD + enable/disable
  - [ ] `IAccessListClient` — CRUD
  - [ ] `IUserClient` — CRUD + permissions
  - [ ] `ISettingsClient` — Get, Update
  - [ ] `IAuditLogClient` — List
  - [ ] `IReportsClient` — GetHosts
- [ ] Task 5: Create stub model classes for interface compilation (AC: #5, #6)
  - [ ] Empty `ProxyHostResponse`, `CreateProxyHostRequest`, `UpdateProxyHostRequest` classes
  - [ ] Empty `CertificateResponse`, `CreateCertificateRequest`, `UploadCertificateRequest` classes
  - [ ] Place in `Models/ProxyHosts/` and `Models/Certificates/` namespaces
  - [ ] Note: Full model properties will be implemented in Stories 3.1 and 4.1

## Dev Notes

- **Architecture reference:** [Source: architecture.md#Client Interface Design]
- **Pattern:** Per-resource sub-interfaces accessed via root client properties
- **Critical:** All models are POCOs — no serializer attributes in core package
- **Critical:** Stub models only need enough to compile interfaces — full properties come in Epic 3/4
- **Namespace:** Interfaces in `NginxApiClient.Clients`, root interface in `NginxApiClient`
- **Anti-pattern:** Do NOT create `BaseClient` or `AbstractResourceClient` — use composition

### Project Structure Notes

- `src/NginxApiClient/INginxProxyManagerClient.cs`
- `src/NginxApiClient/Clients/IProxyHostClient.cs`
- `src/NginxApiClient/Clients/ICertificateClient.cs`
- `src/NginxApiClient/Clients/I{Resource}Client.cs` (Phase 2)
- `src/NginxApiClient/Models/ProxyHosts/*.cs` (stubs)
- `src/NginxApiClient/Models/Certificates/*.cs` (stubs)

### References

- [Source: architecture.md#Client Interface Design]
- [Source: architecture.md#Resource Client Pattern]
- [Source: prd.md#FR43, FR44]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

