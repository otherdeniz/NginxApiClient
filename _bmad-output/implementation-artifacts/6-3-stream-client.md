# Story 6.3: Stream Client

Status: review

## Story

As a developer,
I want to manage TCP/UDP streams,
so that I can automate stream forwarding configuration.

## Acceptance Criteria

1. All CRUD + enable/disable operations work following the established pattern
2. Models include `IncomingPort`, `ForwardingHost`, `ForwardingPort`, `TcpForwarding`, `UdpForwarding`
3. Unit tests cover all operations

## Tasks / Subtasks

- [x] Task 1: Create stream models in `Models/Streams/`
  - [x] `StreamResponse`, `CreateStreamRequest`, `UpdateStreamRequest`
  - [x] Properties: `IncomingPort`, `ForwardingHost`, `ForwardingPort`, `TcpForwarding`, `UdpForwarding`, `Enabled`
- [x] Task 2: Implement StreamClient
  - [x] Endpoints: `/api/nginx/streams`
  - [x] Follow established resource client pattern
- [x] Task 3: Wire into NginxProxyManagerClient root
- [x] Task 4: Write unit tests

## Dev Notes

- **NPM endpoint:** `/api/nginx/streams`
- **Streams are TCP/UDP forwarding** — different properties from HTTP proxy hosts

### References

- [Source: product-brief-distillate.md#Endpoint Map — Streams]
- [Source: prd.md#FR27, FR28]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- Implemented StreamClient for TCP/UDP forwarding; stream-specific properties (IncomingPort, ForwardingHost, ForwardingPort, TcpForwarding, UdpForwarding) modelled correctly; all operations and unit tests complete.

### Change Log

- 2026-04-17: Story completed and moved to review status.

### File List

