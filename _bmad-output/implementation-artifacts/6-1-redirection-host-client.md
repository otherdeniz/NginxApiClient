# Story 6.1: Redirection Host Client

Status: review

## Story

As a developer,
I want to manage redirection hosts — list, get, create, update, delete, enable, disable,
so that I can automate URL redirection configuration.

## Acceptance Criteria

1. All CRUD + enable/disable operations work following the proxy host pattern
2. Models include `DomainNames`, `ForwardScheme`, `ForwardDomainName`, `PreservePath`, `HttpStatusCode`
3. Unit tests cover all operations

## Tasks / Subtasks

- [x] Task 1: Create redirection host models in `Models/RedirectionHosts/`
  - [x] `RedirectionHostResponse`, `CreateRedirectionHostRequest`, `UpdateRedirectionHostRequest`
  - [x] Properties: `DomainNames`, `ForwardScheme`, `ForwardDomainName`, `PreservePath`, `HttpStatusCode`, `Enabled`
- [x] Task 2: Implement RedirectionHostClient (AC: #1)
  - [x] Internal class implementing `IRedirectionHostClient`
  - [x] Endpoints: `/api/nginx/redirection-hosts`
  - [x] Follow ProxyHostClient pattern exactly
- [x] Task 3: Wire into NginxProxyManagerClient root
  - [x] Set `RedirectionHosts` property to return `RedirectionHostClient` instance
- [x] Task 4: Write unit tests (AC: #3)
  - [x] CRUD + enable/disable tests following ProxyHostClientTests pattern

## Dev Notes

- **Follow established pattern** from ProxyHostClient (Story 3.2)
- **NPM endpoint:** `/api/nginx/redirection-hosts`
- **Phase 2:** Include in v1 if achievable without exceptional effort

### References

- [Source: product-brief-distillate.md#Endpoint Map — Redirection Hosts]
- [Source: prd.md#FR23, FR24]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- Implemented RedirectionHostClient following ProxyHostClient pattern; all CRUD and enable/disable operations covered; models and unit tests complete.

### Change Log

- 2026-04-17: Story completed and moved to review status.

### File List

