# Story 4.2: Certificate Client — CRUD & Let's Encrypt

Status: ready-for-dev

## Story

As a developer,
I want to list, get, create, delete, upload, download, and renew certificates,
so that I can automate certificate lifecycle management.

## Acceptance Criteria

1. `ListAsync()` returns all certificates with expiry dates
2. `CreateAsync(request)` provisions a Let's Encrypt certificate
3. `UploadAsync(request)` uploads a custom certificate
4. `DownloadAsync(id)` returns certificate content
5. `RenewAsync(id)` renews a certificate
6. `DeleteAsync(id)` removes a certificate

## Tasks / Subtasks

- [ ] Task 1: Create CertificateClient internal implementation (AC: #1-#6)
  - [ ] Internal class implementing `ICertificateClient`
  - [ ] Constructor takes `HttpClient` and `IJsonSerializer`
  - [ ] `ListAsync` — GET `/api/nginx/certificates`
  - [ ] `GetAsync` — GET `/api/nginx/certificates/{id}`
  - [ ] `CreateAsync` — POST `/api/nginx/certificates`
  - [ ] `UploadAsync` — POST `/api/nginx/certificates/{id}/upload`
  - [ ] `DownloadAsync` — GET `/api/nginx/certificates/{id}/download`
  - [ ] `RenewAsync` — POST `/api/nginx/certificates/{id}/renew`
  - [ ] `DeleteAsync` — DELETE `/api/nginx/certificates/{id}`
  - [ ] Follow async method pattern from architecture
- [ ] Task 2: Write unit tests (AC: #1-#6)
  - [ ] Test all CRUD operations with MockHttpMessageHandler
  - [ ] Test Let's Encrypt creation
  - [ ] Test upload, download, renew flows
  - [ ] Verify correct HTTP methods and URLs

## Dev Notes

- **NPM API:** Certificate endpoints are MISSING from swagger spec — use Bash API script and Terraform provider as reference
- **Endpoint pattern:** `/api/nginx/certificates` (list/create), `/api/nginx/certificates/{id}` (get/delete)
- **Special endpoints:** Upload, download, renew have different URL patterns — verify against live NPM
- **Follow same pattern** as ProxyHostClient from Story 3.2

### References

- [Source: product-brief-distillate.md#Endpoint Map — Certificates]
- [Source: product-brief-distillate.md#API Documentation Gaps]
- [Source: prd.md#FR16-FR22]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

