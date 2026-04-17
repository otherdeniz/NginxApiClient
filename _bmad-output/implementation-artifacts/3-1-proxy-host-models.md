# Story 3.1: Proxy Host Models

Status: ready-for-dev

## Story

As a developer,
I want strongly-typed models for proxy host requests and responses,
so that I get IntelliSense and compile-time safety when working with proxy hosts.

## Acceptance Criteria

1. `ProxyHostResponse` includes all NPM proxy host properties: `Id`, `DomainNames`, `ForwardScheme`, `ForwardHost`, `ForwardPort`, `CachingEnabled`, `AllowWebsocketUpgrade`, `BlockExploits`, `AccessListId`, `CertificateId`, `SslForced`, `HstsEnabled`, `HstsSubdomains`, `Http2Support`, `Enabled`, `Locations`, `AdvancedConfig`
2. `CreateProxyHostRequest` includes all settable properties with XML doc comments
3. `UpdateProxyHostRequest` includes all updatable properties
4. `ProxyHostLocation` models the `locations[]` array
5. All models are POCOs with no serializer-specific attributes

## Tasks / Subtasks

- [ ] Task 1: Create ProxyHostResponse model (AC: #1, #5)
  - [ ] All properties listed in AC #1
  - [ ] `DomainNames` as `IReadOnlyList<string>`
  - [ ] `Locations` as `IReadOnlyList<ProxyHostLocation>`
  - [ ] XML doc comments on all properties
- [ ] Task 2: Create CreateProxyHostRequest model (AC: #2, #5)
  - [ ] Required: `DomainNames`, `ForwardScheme`, `ForwardHost`, `ForwardPort`
  - [ ] Optional: SSL settings, advanced options, locations, certificate/ACL IDs
  - [ ] XML doc comments explaining each property
- [ ] Task 3: Create UpdateProxyHostRequest model (AC: #3, #5)
  - [ ] All updatable proxy host properties
  - [ ] XML doc comments
- [ ] Task 4: Create ProxyHostLocation model (AC: #4, #5)
  - [ ] `Path`, `ForwardScheme`, `ForwardHost`, `ForwardPort`, `AdvancedConfig`
  - [ ] XML doc comments
- [ ] Task 5: Write model tests
  - [ ] Test model instantiation with all properties
  - [ ] Test serialization round-trip with both serializers (from Story 1.2)
  - [ ] Verify `snake_case` JSON mapping works correctly

## Dev Notes

- **Architecture reference:** [Source: architecture.md#Format Patterns]
- **NPM API properties from Terraform provider:** `domain_names`, `forward_scheme` (http/https), `forward_host`, `forward_port`, `caching_enabled`, `allow_websocket_upgrade`, `block_exploits`, `access_list_id`, `certificate_id`, `ssl_forced`, `hsts_enabled`, `hsts_subdomains`, `http2_support`, `locations[]`, `advanced_config`
- **Critical:** These replace the stub models from Story 1.4
- **All models are POCOs** — no `[JsonProperty]` or `[JsonPropertyName]` attributes
- **Namespace:** `NginxApiClient.Models.ProxyHosts`

### References

- [Source: product-brief-distillate.md#Key Proxy Host Properties]
- [Source: architecture.md#Format Patterns]
- [Source: prd.md#FR6-FR15]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

