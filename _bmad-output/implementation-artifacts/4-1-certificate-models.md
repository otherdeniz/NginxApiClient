# Story 4.1: Certificate Models

Status: ready-for-dev

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

- [ ] Task 1: Create CertificateResponse model (AC: #1, #4)
  - [ ] All properties from AC #1
  - [ ] `DomainNames` as `IReadOnlyList<string>`
  - [ ] Date properties as `DateTime`
  - [ ] XML doc comments
- [ ] Task 2: Create CreateCertificateRequest model (AC: #2, #4)
  - [ ] `DomainNames` (string array), `Provider` (string — e.g., "letsencrypt")
  - [ ] Additional Let's Encrypt-specific fields as discovered
  - [ ] XML doc comments
- [ ] Task 3: Create UploadCertificateRequest model (AC: #3, #4)
  - [ ] Certificate content, private key content, intermediate certificate
  - [ ] XML doc comments
- [ ] Task 4: Write model tests
  - [ ] Test serialization round-trip with both serializers
  - [ ] Verify `snake_case` JSON mapping

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

### Debug Log References

### Completion Notes List

### File List

