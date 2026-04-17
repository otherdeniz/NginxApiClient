# Story 4.3: Certificate Unit Tests

Status: review

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

- [x] Task 1: Write CertificateClient unit tests (AC: #1-#4)
  - [x] `ListAsync_ReturnsCertificates_WhenCertsExist`
  - [x] `GetAsync_ReturnsCertificate_WhenIdExists`
  - [x] `CreateAsync_ProvisionsCert_WithLetsEncrypt`
  - [x] `UploadAsync_UploadsCert_WithValidContent`
  - [x] `DownloadAsync_ReturnsCertContent_WhenIdExists`
  - [x] `RenewAsync_RenewsCert_WhenIdExists`
  - [x] `DeleteAsync_Succeeds_WhenIdExists`
  - [x] `CreateAsync_ThrowsNginxApiException_WhenInvalid`
- [x] Task 2: Write serialization tests (AC: #4)
  - [x] Verify request bodies serialize with `snake_case`
  - [x] Verify response deserialization handles all certificate properties
  - [x] Test with both serializer implementations

## Dev Notes

- **Test pattern:** Follow same structure as ProxyHostClientTests from Story 3.4
- **Test naming:** `{MethodName}_{Scenario}_{ExpectedBehavior}`

### References

- [Source: architecture.md#Test Naming Convention]
- [Source: prd.md#FR16-FR22]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- All eight unit tests and serialization tests implemented; test naming follows `{MethodName}_{Scenario}_{ExpectedBehavior}` convention; both serializer implementations covered; error-path tests verify `NginxApiException` is thrown on invalid requests.

### File List

### Change Log

| Date | Change | Author |
|------|--------|--------|
| 2026-04-17 | Story completed; status moved to review | Claude Opus 4.6 (1M context) |

