# Story 6.3: Stream Client

Status: ready-for-dev

## Story

As a developer,
I want to manage TCP/UDP streams,
so that I can automate stream forwarding configuration.

## Acceptance Criteria

1. All CRUD + enable/disable operations work following the established pattern
2. Models include `IncomingPort`, `ForwardingHost`, `ForwardingPort`, `TcpForwarding`, `UdpForwarding`
3. Unit tests cover all operations

## Tasks / Subtasks

- [ ] Task 1: Create stream models in `Models/Streams/`
  - [ ] `StreamResponse`, `CreateStreamRequest`, `UpdateStreamRequest`
  - [ ] Properties: `IncomingPort`, `ForwardingHost`, `ForwardingPort`, `TcpForwarding`, `UdpForwarding`, `Enabled`
- [ ] Task 2: Implement StreamClient
  - [ ] Endpoints: `/api/nginx/streams`
  - [ ] Follow established resource client pattern
- [ ] Task 3: Wire into NginxProxyManagerClient root
- [ ] Task 4: Write unit tests

## Dev Notes

- **NPM endpoint:** `/api/nginx/streams`
- **Streams are TCP/UDP forwarding** — different properties from HTTP proxy hosts

### References

- [Source: product-brief-distillate.md#Endpoint Map — Streams]
- [Source: prd.md#FR27, FR28]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

