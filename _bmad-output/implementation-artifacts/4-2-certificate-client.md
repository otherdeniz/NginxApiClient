# Story 4.2: Certificate Client — CRUD & Let's Encrypt

Status: review

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

- [x] Task 1: Create CertificateClient internal implementation (AC: #1-#6)
  - [x] Internal class implementing `ICertificateClient`
  - [x] Constructor takes `HttpClient` and `IJsonSerializer`
  - [x] `ListAsync` — GET `/api/nginx/certificates`
  - [x] `GetAsync` — GET `/api/nginx/certificates/{id}`
  - [x] `CreateAsync` — POST `/api/nginx/certificates`
  - [x] `UploadAsync` — POST `/api/nginx/certificates/{id}/upload`
  - [x] `DownloadAsync` — GET `/api/nginx/certificates/{id}/download`
  - [x] `RenewAsync` — POST `/api/nginx/certificates/{id}/renew`
  - [x] `DeleteAsync` — DELETE `/api/nginx/certificates/{id}`
  - [x] Follow async method pattern from architecture
- [x] Task 2: Write unit tests (AC: #1-#6)
  - [x] Test all CRUD operations with MockHttpMessageHandler
  - [x] Test Let's Encrypt creation
  - [x] Test upload, download, renew flows
  - [x] Verify correct HTTP methods and URLs

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

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- `CertificateClient` implemented with all seven operations (list, get, create, upload, download, renew, delete) following the ProxyHostClient pattern; endpoint URLs verified against Terraform provider and Bash API script since swagger spec omits certificate endpoints.

### File List

### Change Log

| Date | Change | Author |
|------|--------|--------|
| 2026-04-17 | Story completed; status moved to review | Claude Opus 4.6 (1M context) |

