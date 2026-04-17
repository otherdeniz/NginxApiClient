# Story 5.3: Client Mocking in Consumer Tests

Status: review

## Story

As a developer,
I want to mock `INginxProxyManagerClient` and its sub-interfaces in my unit tests,
so that I can test my code without a live NPM instance.

## Acceptance Criteria

1. `INginxProxyManagerClient` can be mocked with standard mocking frameworks (Moq, NSubstitute)
2. Per-resource sub-interfaces are independently mockable
3. Consumer can set up return values and verify method calls

## Tasks / Subtasks

- [x] Task 1: Verify all interfaces are mockable (AC: #1, #2)
  - [x] Confirm `INginxProxyManagerClient` has no sealed/static members preventing mocking
  - [x] Confirm all per-resource interfaces are independently mockable
  - [x] Verify property getters on root interface return mockable sub-interfaces
- [x] Task 2: Write example mocking tests demonstrating consumer usage (AC: #1, #2, #3)
  - [x] Example: Mock `IProxyHostClient.ListAsync()` to return preset data
  - [x] Example: Mock `ICertificateClient.GetAsync()` to throw `NginxNotFoundException`
  - [x] Example: Verify `CreateAsync` was called with expected arguments
  - [x] Use a popular mocking framework (add to test dependencies if needed)
- [x] Task 3: Document mocking patterns in code comments
  - [x] Add XML doc comments on `INginxProxyManagerClient` explaining mocking approach
  - [x] Ensure examples are clear for consumers

## Dev Notes

- **This story validates the design** — if mocking is difficult, the interface design needs revision
- **Framework choice:** Moq or NSubstitute — pick whichever is simpler for the test project
- **All interfaces are already public** — mocking should work out of the box

### References

- [Source: architecture.md#Client Interface Design]
- [Source: prd.md#FR42]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- All interfaces confirmed mockable with Moq out of the box (no sealed or static members); example tests added demonstrating list mock, exception mock on `ICertificateClient.GetAsync`, and call-verification on `CreateAsync`; XML doc comments updated on `INginxProxyManagerClient` to document the mocking approach.

### File List

### Change Log

| Date | Change | Author |
|------|--------|--------|
| 2026-04-17 | Story completed; status moved to review | Claude Opus 4.6 (1M context) |

