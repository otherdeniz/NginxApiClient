---
stepsCompleted: [step-01-init, step-02-discovery, step-02b-vision, step-02c-executive-summary, step-03-success, step-04-journeys, step-05-domain, step-06-innovation, step-07-project-type, step-08-scoping, step-09-functional, step-10-nonfunctional, step-11-polish, step-12-complete]
inputDocuments: [product-brief-NginxApiClient.md, product-brief-NginxApiClient-distillate.md]
documentCounts:
  briefs: 2
  research: 0
  brainstorming: 0
  projectDocs: 0
classification:
  projectType: developer_tool
  domain: general
  complexity: low
  projectContext: greenfield
workflowType: 'prd'
---

# Product Requirements Document - NginxApiClient

**Author:** Deniz Esen
**Date:** 2026-04-16

## Executive Summary

NginxApiClient is a comprehensive C# client library for the NGINX Proxy Manager REST API, distributed as a multi-package NuGet solution. It provides strongly-typed, async-first access to all 11 NPM API resource groups — from proxy host management and certificate provisioning to user administration and audit logging.

NGINX Proxy Manager (100M+ Docker pulls, 32,500+ GitHub stars) exposes a powerful but undocumented REST API. No client library exists for .NET — and existing clients in other languages (Python, Bash, PHP) are minimal, untyped, and poorly maintained. The most mature implementation is a Go-based Terraform provider locked to the Terraform ecosystem. NginxApiClient fills this gap completely: it is the first and only .NET SDK for NPM.

The library targets .NET Framework 4.8 through .NET 10 via a sub-package architecture that decouples the core API (interfaces, models, abstractions on .NET Standard 2.0) from serialization concerns (`NginxApiClient.SystemTextJson` for modern targets, `NginxApiClient.NewtonsoftJson` for legacy). It handles JWT authentication lifecycle transparently, exposes `INginxProxyManagerClient` for consumer testability, and provides `IServiceCollection` integration for ASP.NET Core. Comprehensive documentation — covering every endpoint with request/response examples — serves as the de facto NPM API reference, filling a void the official project has left open.

Built and dogfooded by the author for their own infrastructure automation needs. Licensed under MIT.

### What Makes This Special

- **First-mover monopoly:** Zero .NET clients exist for NPM on NuGet.org. NginxApiClient owns this niche backed by a 100M+ install-base upstream project.
- **Type safety as infrastructure risk reduction:** Compile-time guarantees for proxy configuration changes mean errors surface at build time, not at 2am when a certificate expires or a host disappears. This is the structural advantage over every Bash/Python/PHP alternative.
- **Documentation as product:** NPM's API is notoriously undocumented (incomplete swagger spec, missing endpoints). NginxApiClient's docs become the community reference, attracting developers across all ecosystems and serving as an organic discovery channel.
- **Broadest .NET coverage:** The sub-package architecture supports .NET Framework 4.8 (enterprise/legacy) through .NET 10 (modern) without forcing serializer dependencies on consumers.

## Project Classification

| Attribute | Value |
|-----------|-------|
| **Project Type** | Developer Tool (SDK/Library) |
| **Domain** | General (Infrastructure Tooling) |
| **Complexity** | Low |
| **Project Context** | Greenfield |

## Success Criteria

### User Success

- Developer installs NuGet package and makes their first API call (list proxy hosts) within 5 minutes using the quick-start guide
- All proxy host operations are fully typed with IntelliSense — no guessing property names or value formats
- JWT authentication is invisible to the consumer — no manual token management
- Library works identically across .NET Framework 4.8, .NET 8, and .NET 10 with the appropriate serialization package

### Business Success

| Metric | Target |
|--------|--------|
| Personal utility | Author uses NginxApiClient as primary integration for proxy host deployment and certificate rotation in own projects |
| NuGet publication | Package published, discoverable, with proper metadata and tags |
| Community signal | Organic NuGet downloads and GitHub engagement within 6 months of publish |
| Documentation completeness | Every implemented endpoint documented with request/response examples |

### Technical Success

- All targeted .NET frameworks build and pass tests in CI
- Integration tests run against a Dockerized NPM instance (pinned version)
- Sub-package architecture cleanly separates core from serialization with no leaky abstractions
- Zero runtime serializer dependencies in the core package

### Measurable Outcomes

- Proxy host full lifecycle (create, read, update, delete, enable, disable) works end-to-end against live NPM
- Certificate provisioning (including Let's Encrypt) works end-to-end against live NPM
- Token refresh handles expiry transparently without consumer intervention
- All implemented API calls return strongly-typed results with meaningful error messages on failure

## Project Scoping & Phased Development

### MVP Strategy

**MVP Approach:** Problem-solving MVP — deliver the core proxy host and certificate automation that the author needs personally, with enough polish (docs, DI, typed models) to make it community-ready from day one.

**Resource Requirements:** Solo developer. No external contributors planned for v1.

### Phase 1 — MVP (Must-Have)

**Core User Journeys Supported:**
- Journey 1: Proxy host deployment automation
- Journey 2: Certificate rotation and error recovery
- Journey 4: Debugging with structured exceptions

**Capabilities:**
- Multi-package architecture: `NginxApiClient` (core), `NginxApiClient.SystemTextJson`, `NginxApiClient.NewtonsoftJson`
- Proxy Hosts — full CRUD + enable/disable
- Certificates — CRUD + Let's Encrypt provisioning
- Tokens — login, refresh, transparent lifecycle management
- `INginxProxyManagerClient` interface for testability
- `IServiceCollection` DI integration
- Custom exception hierarchy (`NginxApiException`, `NginxAuthenticationException`, `NginxNotFoundException`)
- Async API with CancellationToken
- XML doc comments on all public API
- README with quick-start guide and inline examples
- `examples/` folder with runnable console projects
- `.snupkg` symbol packages with SourceLink
- CI/CD with GitHub Actions (matrix build, integration tests, NuGet publish)

### Phase 2 — Growth (Best-Effort for v1)

Include if achievable without exceptional effort. Each resource group is independent and can be added incrementally:

- Redirection Hosts — full CRUD + enable/disable
- Dead Hosts (404) — full CRUD + enable/disable
- Streams — full CRUD + enable/disable
- Access Lists — CRUD
- Users — CRUD + permissions
- Settings — get/update
- Audit Log — list
- Reports — host statistics

### Phase 3 — Expansion (Future)

- Rich examples library (bulk migration, backup/restore, environment sync)
- NPM version compatibility matrix
- Community contributions and PR acceptance

### Risk Mitigation Strategy

| Risk | Mitigation |
|------|------------|
| **Technical: Undocumented API** | Use Bash API script (92 stars), Terraform provider source, and live NPM instance as reverse-engineering references. Document discovered endpoints as we go. |
| **Technical: Multi-target complexity** | Sub-package architecture isolates serializer differences. Matrix CI builds catch per-target issues early. |
| **Technical: API instability** | Pin integration tests to specific NPM Docker tags. Version NuGet packages aligned with tested NPM releases. |
| **Resource: Solo developer** | MVP scoped tightly to personal use cases. Phase 2 features are additive and independent. |
| **Market: Low initial adoption** | Primary goal is personal utility. Good docs and NuGet discoverability create passive discovery. |

## User Journeys

### Journey 1: Marco — Self-Hosting Developer Automates Proxy Deployment (Primary, Happy Path)

Marco runs a homelab with Docker Compose — 15 services behind NGINX Proxy Manager, each proxy host configured manually through the web UI. Every time he adds a new service, it's the same tedious clicks: domain name, forward host, forward port, SSL settings. He's building a C# deployment tool that spins up Docker containers programmatically and wants proxy host creation to be part of that pipeline.

He searches NuGet for "nginx proxy manager" and finds NginxApiClient. He installs `NginxApiClient` and `NginxApiClient.SystemTextJson`, adds `services.AddNginxApiClient(...)` to his DI container, and within 5 minutes has his first `ListProxyHostsAsync()` returning strongly-typed results with full IntelliSense. He builds a `CreateProxyHostAsync()` call into his deployment pipeline — new services now get proxy hosts automatically with SSL, websocket support, and custom nginx directives, all expressed as typed C# properties instead of raw JSON.

**Capabilities revealed:** NuGet package installation, DI integration, proxy host CRUD, typed models with IntelliSense, async API, quick-start documentation.

### Journey 2: Marco — Certificate Rotation and Error Recovery (Primary, Edge Case)

Marco's Let's Encrypt certificates are expiring across 15 hosts. He writes a C# script using NginxApiClient to enumerate all certificates via `ListCertificatesAsync()`, filter by expiry date, and trigger renewal via `RenewCertificateAsync()`. Midway through processing, his NPM Docker container restarts during a stack update. The JWT token expires.

The library detects the 401 response, transparently re-authenticates using the stored credentials, and retries the failed request. Marco's script completes without any retry logic on his side. When one certificate fails to renew (DNS challenge timeout), the library throws a `NginxApiException` with the NPM error detail — Marco catches it, logs the failure, and continues processing the remaining certificates.

**Capabilities revealed:** Certificate listing and renewal, transparent token refresh, structured exceptions with NPM error details, graceful partial-failure handling.

### Journey 3: Ayse — Enterprise Team Integrating NPM into Internal Tooling (Secondary, Admin/Integration)

Ayse leads a .NET team at a mid-size company. They run NPM behind the corporate firewall for internal microservice routing — 50+ proxy hosts across staging and production. Her team is on .NET Framework 4.8 with an existing ASP.NET application for internal DevOps tooling.

She installs `NginxApiClient` and `NginxApiClient.NewtonsoftJson` (matching their existing JSON stack). She wires `INginxProxyManagerClient` into their DI container and builds an admin dashboard page that lists all proxy hosts, shows their status, and allows bulk enable/disable operations. Her team writes unit tests by mocking `INginxProxyManagerClient` — no live NPM instance needed in CI. When a new microservice deploys, their pipeline calls NginxApiClient to create the proxy host automatically, matching the staging configuration.

**Capabilities revealed:** .NET Framework 4.8 support, NewtonsoftJson serialization, INginxProxyManagerClient interface for mocking, DI integration, bulk operations, cross-environment configuration.

### Journey 4: Dev Consumer — Debugging and Testing (Support/Troubleshooting)

A developer using NginxApiClient in their CI/CD pipeline gets a failed build. The error: `NginxApiException: 422 Unprocessable Entity — "domain_names" is required`. The exception type, HTTP status, and NPM error detail are all immediately visible. They check the `CreateProxyHostRequest` model in IntelliSense, see that `DomainNames` is a required `string[]` property with XML doc comments explaining the expected format, fix their request, and the pipeline passes.

Later, they write unit tests for their deployment logic. They mock `INginxProxyManagerClient` to return predictable proxy host data, verifying their code handles the happy path and error cases without needing a running NPM container or network access.

**Capabilities revealed:** Structured exceptions with HTTP status and NPM error detail, XML doc comments, typed request models with required/optional properties, interface-based testability.

### Journey Requirements Traceability

| Capability | Revealed By | FR Reference |
|-----------|-------------|-------------|
| Proxy Host CRUD + enable/disable | Journeys 1, 3 | FR6–FR15 |
| Certificate CRUD + Let's Encrypt renewal | Journey 2 | FR16–FR22 |
| Transparent JWT token lifecycle | Journeys 1, 2, 3 | FR1–FR5 |
| Structured exception hierarchy | Journeys 2, 4 | FR35–FR37 |
| `INginxProxyManagerClient` interface for mocking | Journeys 3, 4 | FR42 |
| `IServiceCollection` DI integration | Journeys 1, 3 | FR40 |
| .NET Framework 4.8 + NewtonsoftJson | Journey 3 | FR38–FR39 |
| .NET 8/10 + SystemTextJson | Journeys 1, 2 | FR38–FR39 |
| Strongly-typed models with IntelliSense | All journeys | FR45 |
| Quick-start documentation | Journey 1 | FR46 |
| Async API with CancellationToken | All journeys | FR43–FR44 |

## Developer Tool Specific Requirements

### Language & Platform Matrix

| Target | Package | Framework | JSON Serializer |
|--------|---------|-----------|-----------------|
| Core library | `NginxApiClient` | .NET Standard 2.0 | None (abstractions only) |
| Modern .NET | `NginxApiClient.SystemTextJson` | .NET 8, .NET 10 | System.Text.Json |
| Legacy .NET | `NginxApiClient.NewtonsoftJson` | .NET Framework 4.8, .NET Standard 2.0 | Newtonsoft.Json |

### Installation Methods

**Modern .NET (.NET 8/10):**
```
dotnet add package NginxApiClient
dotnet add package NginxApiClient.SystemTextJson
```

**Legacy .NET (.NET Framework 4.8):**
```
Install-Package NginxApiClient
Install-Package NginxApiClient.NewtonsoftJson
```

### NuGet Package Requirements

- All three packages published to NuGet.org
- `.snupkg` symbol packages published for all packages (debug stepping into library source)
- SourceLink enabled for source-level debugging
- NuGet metadata: description, tags (`nginx-proxy-manager`, `npm-api`, `reverse-proxy`, `nginx`, `docker`), project URL, license (MIT), repository URL
- Package icon and README embedded in NuGet package

### API Surface Design

- `INginxProxyManagerClient` — primary interface, one method group per resource
- Async-only API — all methods return `Task<T>` with `CancellationToken` parameter
- Strongly-typed request/response models per endpoint
- Custom exception hierarchy: `NginxApiException` (base), `NginxAuthenticationException`, `NginxNotFoundException`
- JWT token lifecycle managed internally (transparent to consumer)
- `IServiceCollection.AddNginxApiClient(...)` extension method for DI registration

### Documentation Strategy

| Channel | Content | Format |
|---------|---------|--------|
| README.md | Quick-start guide, installation, basic usage, API overview | GitHub Markdown |
| XML doc comments | Every public type, method, property documented with `<summary>`, `<param>`, `<returns>`, `<exception>` | IntelliSense-visible |
| `examples/` folder | Runnable console projects demonstrating real-world scenarios | .NET console apps |

### Code Examples Requirements

**Inline (README):**
- Install + authenticate + list proxy hosts in under 10 lines
- Create a proxy host with SSL
- DI registration in ASP.NET Core

**Runnable examples (`examples/` folder):**
- Basic CRUD operations on proxy hosts
- Certificate provisioning and renewal
- Bulk operations across multiple hosts
- Error handling patterns
- .NET Framework 4.8 example with NewtonsoftJson

### Implementation Considerations

- **HttpClient lifecycle:** Use `IHttpClientFactory` pattern via DI; provide manual `HttpClient` injection for non-DI scenarios
- **Thread safety:** Token refresh must be thread-safe for concurrent API calls
- **Nullable reference types:** Enabled on .NET 8/10 targets; not available on .NET Framework 4.8
- **Conditional compilation:** `#if` directives for target-specific behavior (e.g., nullable annotations)
- **CI/CD:** GitHub Actions with matrix build across all target frameworks; integration tests against Dockerized NPM; automated NuGet publish on release tags

## Functional Requirements

### Authentication & Token Management

- **FR1:** Consumer can authenticate with an NPM instance using email and password credentials
- **FR2:** Library can automatically obtain a JWT bearer token on first API call
- **FR3:** Library can proactively refresh a JWT token before expiry without consumer intervention
- **FR4:** Library can re-authenticate transparently when a token is rejected (401 response)
- **FR5:** Consumer can configure the NPM instance base URL and credentials at initialization

### Proxy Host Management

- **FR6:** Consumer can list all proxy hosts from an NPM instance
- **FR7:** Consumer can retrieve a single proxy host by ID
- **FR8:** Consumer can create a new proxy host with domain names, forward host/port, and scheme
- **FR9:** Consumer can update an existing proxy host's configuration
- **FR10:** Consumer can delete a proxy host by ID
- **FR11:** Consumer can enable or disable a proxy host
- **FR12:** Consumer can configure SSL settings on a proxy host (force SSL, HSTS, HTTP/2)
- **FR13:** Consumer can configure advanced options on a proxy host (websocket upgrade, block exploits, caching, custom nginx directives)
- **FR14:** Consumer can associate a certificate and access list with a proxy host
- **FR15:** Consumer can configure location-based routing on a proxy host

### Certificate Management

- **FR16:** Consumer can list all certificates from an NPM instance
- **FR17:** Consumer can retrieve a single certificate by ID
- **FR18:** Consumer can provision a new Let's Encrypt certificate via the API
- **FR19:** Consumer can upload a custom SSL certificate
- **FR20:** Consumer can download an existing certificate
- **FR21:** Consumer can delete a certificate by ID
- **FR22:** Consumer can renew an existing certificate

### Redirection Host Management (Phase 2)

- **FR23:** Consumer can perform full CRUD operations on redirection hosts
- **FR24:** Consumer can enable or disable a redirection host

### Dead Host Management (Phase 2)

- **FR25:** Consumer can perform full CRUD operations on dead hosts (404 pages)
- **FR26:** Consumer can enable or disable a dead host

### Stream Management (Phase 2)

- **FR27:** Consumer can perform full CRUD operations on TCP/UDP streams
- **FR28:** Consumer can enable or disable a stream

### Access List Management (Phase 2)

- **FR29:** Consumer can perform full CRUD operations on access lists

### User Administration (Phase 2)

- **FR30:** Consumer can perform full CRUD operations on NPM users
- **FR31:** Consumer can manage user permissions

### System Operations (Phase 2)

- **FR32:** Consumer can retrieve and update NPM settings (default site configuration)
- **FR33:** Consumer can retrieve audit log entries
- **FR34:** Consumer can retrieve host report statistics

### Error Handling & Diagnostics

- **FR35:** Library surfaces NPM API errors as typed exceptions with HTTP status, NPM error detail, and meaningful messages
- **FR36:** Library distinguishes authentication failures, not-found errors, and general API errors via distinct exception types
- **FR37:** Consumer can catch specific exception types to handle different error scenarios

### Package Architecture & Integration

- **FR38:** Consumer can install a core package with no serializer dependency
- **FR39:** Consumer can choose System.Text.Json or Newtonsoft.Json serialization via separate packages
- **FR40:** Consumer can register the client in an ASP.NET Core DI container via `IServiceCollection` extension method
- **FR41:** Consumer can instantiate the client manually without DI for non-ASP.NET scenarios
- **FR42:** Consumer can mock the client interface (`INginxProxyManagerClient`) in unit tests

### Async API & Cancellation

- **FR43:** Consumer can invoke all API operations asynchronously
- **FR44:** Consumer can pass a `CancellationToken` to any API operation to support cooperative cancellation

### Documentation & Developer Experience

- **FR45:** Consumer can discover all public types, methods, and properties via IntelliSense (XML doc comments)
- **FR46:** Consumer can follow a quick-start guide to make their first API call within 5 minutes
- **FR47:** Consumer can reference runnable example projects for common scenarios
- **FR48:** Consumer can step into library source code during debugging via symbol packages

## Non-Functional Requirements

### Performance

- All API method calls add less than 50ms overhead beyond network latency (serialization + deserialization)
- Token refresh adds at most one additional HTTP round-trip, not blocking concurrent calls
- Library introduces zero background threads or timers when idle — no resource consumption when not actively making API calls
- Memory allocation per API call is proportional to response size — no excessive buffering or copying

### Security

- Credentials (email/password) are never logged, serialized to disk, or exposed in exception messages
- JWT tokens are stored only in memory — never persisted to disk or included in diagnostic output
- All HTTP communication uses the scheme provided by the consumer (library does not downgrade HTTPS to HTTP)
- Exception messages include NPM error details but never include credentials or tokens

### Integration

- Library is compatible with `IHttpClientFactory` patterns and does not manage `HttpClient` lifetime internally when used via DI
- Library supports manual `HttpClient` injection for scenarios without DI
- Serialization abstraction allows consumers to configure custom JSON settings (e.g., custom converters, naming policies) on the chosen serializer
- Library does not impose transitive dependencies beyond the chosen serializer package — core package has zero external runtime dependencies
- All public API types are CLS-compliant across all target frameworks

### Reliability

- All API methods are safe to call concurrently from multiple threads
- Token refresh under concurrent access uses a single refresh call, not one per waiting thread
- Transient HTTP failures (network timeouts, 5xx responses) are surfaced immediately — library does not implement retry logic (consumer controls retry policy via Polly or similar)
- Library gracefully handles NPM instance unavailability with clear, actionable exception messages

### Maintainability

- All public API surfaces are covered by XML doc comments
- Library follows .NET naming conventions and design guidelines (Framework Design Guidelines)
- NuGet package versioning follows SemVer 2.0 — breaking API changes increment major version
- CI pipeline validates builds across all target frameworks on every commit
