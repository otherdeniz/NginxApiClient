# Story 5.3: Client Mocking in Consumer Tests

Status: ready-for-dev

## Story

As a developer,
I want to mock `INginxProxyManagerClient` and its sub-interfaces in my unit tests,
so that I can test my code without a live NPM instance.

## Acceptance Criteria

1. `INginxProxyManagerClient` can be mocked with standard mocking frameworks (Moq, NSubstitute)
2. Per-resource sub-interfaces are independently mockable
3. Consumer can set up return values and verify method calls

## Tasks / Subtasks

- [ ] Task 1: Verify all interfaces are mockable (AC: #1, #2)
  - [ ] Confirm `INginxProxyManagerClient` has no sealed/static members preventing mocking
  - [ ] Confirm all per-resource interfaces are independently mockable
  - [ ] Verify property getters on root interface return mockable sub-interfaces
- [ ] Task 2: Write example mocking tests demonstrating consumer usage (AC: #1, #2, #3)
  - [ ] Example: Mock `IProxyHostClient.ListAsync()` to return preset data
  - [ ] Example: Mock `ICertificateClient.GetAsync()` to throw `NginxNotFoundException`
  - [ ] Example: Verify `CreateAsync` was called with expected arguments
  - [ ] Use a popular mocking framework (add to test dependencies if needed)
- [ ] Task 3: Document mocking patterns in code comments
  - [ ] Add XML doc comments on `INginxProxyManagerClient` explaining mocking approach
  - [ ] Ensure examples are clear for consumers

## Dev Notes

- **This story validates the design** — if mocking is difficult, the interface design needs revision
- **Framework choice:** Moq or NSubstitute — pick whichever is simpler for the test project
- **All interfaces are already public** — mocking should work out of the box

### References

- [Source: architecture.md#Client Interface Design]
- [Source: prd.md#FR42]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

