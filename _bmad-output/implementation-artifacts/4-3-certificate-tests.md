# Story 4.3: Certificate Unit Tests

Status: ready-for-dev

## Story

As a developer,
I want comprehensive unit tests for the certificate client,
so that I can verify certificate operations work correctly.

## Acceptance Criteria

1. Tests cover list, get, create (Let's Encrypt), upload, download, renew, and delete
2. Tests verify correct HTTP method and URL path for each operation
3. Tests verify error handling for invalid certificate requests
4. Tests verify proper serialization of certificate models

## Tasks / Subtasks

- [ ] Task 1: Write CertificateClient unit tests (AC: #1-#4)
  - [ ] `ListAsync_ReturnsCertificates_WhenCertsExist`
  - [ ] `GetAsync_ReturnsCertificate_WhenIdExists`
  - [ ] `CreateAsync_ProvisionsCert_WithLetsEncrypt`
  - [ ] `UploadAsync_UploadsCert_WithValidContent`
  - [ ] `DownloadAsync_ReturnsCertContent_WhenIdExists`
  - [ ] `RenewAsync_RenewsCert_WhenIdExists`
  - [ ] `DeleteAsync_Succeeds_WhenIdExists`
  - [ ] `CreateAsync_ThrowsNginxApiException_WhenInvalid`
- [ ] Task 2: Write serialization tests (AC: #4)
  - [ ] Verify request bodies serialize with `snake_case`
  - [ ] Verify response deserialization handles all certificate properties
  - [ ] Test with both serializer implementations

## Dev Notes

- **Test pattern:** Follow same structure as ProxyHostClientTests from Story 3.4
- **Test naming:** `{MethodName}_{Scenario}_{ExpectedBehavior}`

### References

- [Source: architecture.md#Test Naming Convention]
- [Source: prd.md#FR16-FR22]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

