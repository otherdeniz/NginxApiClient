# Story 1.4: Core Client Interfaces

Status: review

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

- [x] Task 1: Create INginxProxyManagerClient root interface (AC: #1)
  - [x] All 10 per-resource properties
  - [x] XML doc comments on interface and all properties
- [x] Task 2: Create IProxyHostClient interface (AC: #4, #5)
  - [x] All 7 methods with CancellationToken
  - [x] XML doc comments with `<exception>` tags
- [x] Task 3: Create ICertificateClient interface (AC: #4, #6)
  - [x] All 7 methods with CancellationToken
  - [x] XML doc comments with `<exception>` tags
- [x] Task 4: Create Phase 2 resource interfaces (AC: #3, #4)
  - [x] 8 stub interfaces for Phase 2 resources
- [x] Task 5: Create stub model classes for interface compilation (AC: #5, #6)
  - [x] ProxyHostResponse, CreateProxyHostRequest, UpdateProxyHostRequest
  - [x] CertificateResponse, CreateCertificateRequest, UploadCertificateRequest
- [x] Task 6: Verify build and all existing tests pass

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Completion Notes List

- INginxProxyManagerClient root interface with 10 per-resource sub-interface properties
- IProxyHostClient: 7 async methods (List, Get, Create, Update, Delete, Enable, Disable)
- ICertificateClient: 7 async methods (List, Get, Create, Upload, Download, Renew, Delete)
- 8 Phase 2 stub interfaces (methods to be added in Epic 6)
- 6 stub model classes (full properties in Stories 3.1, 4.1)
- All 26 existing tests still passing, zero warnings

### File List

- src/NginxApiClient/INginxProxyManagerClient.cs (new)
- src/NginxApiClient/Clients/IProxyHostClient.cs (new)
- src/NginxApiClient/Clients/ICertificateClient.cs (new)
- src/NginxApiClient/Clients/IRedirectionHostClient.cs (new)
- src/NginxApiClient/Clients/IDeadHostClient.cs (new)
- src/NginxApiClient/Clients/IStreamClient.cs (new)
- src/NginxApiClient/Clients/IAccessListClient.cs (new)
- src/NginxApiClient/Clients/IUserClient.cs (new)
- src/NginxApiClient/Clients/ISettingsClient.cs (new)
- src/NginxApiClient/Clients/IAuditLogClient.cs (new)
- src/NginxApiClient/Clients/IReportsClient.cs (new)
- src/NginxApiClient/Models/ProxyHosts/ProxyHostResponse.cs (new — stub)
- src/NginxApiClient/Models/ProxyHosts/CreateProxyHostRequest.cs (new — stub)
- src/NginxApiClient/Models/ProxyHosts/UpdateProxyHostRequest.cs (new — stub)
- src/NginxApiClient/Models/Certificates/CertificateResponse.cs (new — stub)
- src/NginxApiClient/Models/Certificates/CreateCertificateRequest.cs (new — stub)
- src/NginxApiClient/Models/Certificates/UploadCertificateRequest.cs (new — stub)

### Change Log

- 2026-04-17: Story 1.4 implemented — Root client interface + 10 per-resource sub-interfaces + 6 stub models
