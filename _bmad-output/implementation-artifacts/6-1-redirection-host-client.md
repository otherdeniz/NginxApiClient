# Story 6.1: Redirection Host Client

Status: ready-for-dev

## Story

As a developer,
I want to manage redirection hosts — list, get, create, update, delete, enable, disable,
so that I can automate URL redirection configuration.

## Acceptance Criteria

1. All CRUD + enable/disable operations work following the proxy host pattern
2. Models include `DomainNames`, `ForwardScheme`, `ForwardDomainName`, `PreservePath`, `HttpStatusCode`
3. Unit tests cover all operations

## Tasks / Subtasks

- [ ] Task 1: Create redirection host models in `Models/RedirectionHosts/`
  - [ ] `RedirectionHostResponse`, `CreateRedirectionHostRequest`, `UpdateRedirectionHostRequest`
  - [ ] Properties: `DomainNames`, `ForwardScheme`, `ForwardDomainName`, `PreservePath`, `HttpStatusCode`, `Enabled`
- [ ] Task 2: Implement RedirectionHostClient (AC: #1)
  - [ ] Internal class implementing `IRedirectionHostClient`
  - [ ] Endpoints: `/api/nginx/redirection-hosts`
  - [ ] Follow ProxyHostClient pattern exactly
- [ ] Task 3: Wire into NginxProxyManagerClient root
  - [ ] Set `RedirectionHosts` property to return `RedirectionHostClient` instance
- [ ] Task 4: Write unit tests (AC: #3)
  - [ ] CRUD + enable/disable tests following ProxyHostClientTests pattern

## Dev Notes

- **Follow established pattern** from ProxyHostClient (Story 3.2)
- **NPM endpoint:** `/api/nginx/redirection-hosts`
- **Phase 2:** Include in v1 if achievable without exceptional effort

### References

- [Source: product-brief-distillate.md#Endpoint Map — Redirection Hosts]
- [Source: prd.md#FR23, FR24]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

