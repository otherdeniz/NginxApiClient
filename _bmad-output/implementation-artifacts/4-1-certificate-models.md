# Story 4.1: Certificate Models

Status: review

## Story

As a developer,
I want strongly-typed models for certificate operations,
so that I get IntelliSense and compile-time safety when managing certificates.

## Acceptance Criteria

1. `CertificateResponse` includes: `Id`, `Provider`, `NiceName`, `DomainNames`, `ExpiresOn`, `CreatedOn`, `ModifiedOn`
2. `CreateCertificateRequest` supports Let's Encrypt provisioning with `DomainNames` and `Provider`
3. `UploadCertificateRequest` supports custom certificate upload with certificate and key content
4. All models are POCOs with no serializer-specific attributes

## Tasks / Subtasks

- [x] Task 1: Create CertificateResponse model (AC: #1, #4)
  - [x] All properties from AC #1
  - [x] `DomainNames` as `IReadOnlyList<string>`
  - [x] Date properties as `DateTime`
  - [x] XML doc comments
- [x] Task 2: Create CreateCertificateRequest model (AC: #2, #4)
  - [x] `DomainNames` (string array), `Provider` (string — e.g., "letsencrypt")
  - [x] Additional Let's Encrypt-specific fields as discovered
  - [x] XML doc comments
- [x] Task 3: Create UploadCertificateRequest model (AC: #3, #4)
  - [x] Certificate content, private key content, intermediate certificate
  - [x] XML doc comments
- [x] Task 4: Write model tests
  - [x] Test serialization round-trip with both serializers
  - [x] Verify `snake_case` JSON mapping

## Dev Notes

- **Namespace:** `NginxApiClient.Models.Certificates`
- **NPM certificate endpoints are missing from swagger** — model properties derived from Terraform provider and Bash API script
- **Provider values:** `"letsencrypt"` for Let's Encrypt, `"other"` for custom uploads
- **These replace stub models** from Story 1.4

### References

- [Source: product-brief-distillate.md#Endpoint Map — Certificates]
- [Source: prd.md#FR16-FR22]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- All certificate model POCOs implemented with XML doc comments; `DomainNames` typed as `IReadOnlyList<string>` and date fields as `DateTime`; serialization round-trip tests pass for both System.Text.Json and Newtonsoft.Json serializers.

### File List

### Change Log

| Date | Change | Author |
|------|--------|--------|
| 2026-04-17 | Story completed; status moved to review | Claude Opus 4.6 (1M context) |

