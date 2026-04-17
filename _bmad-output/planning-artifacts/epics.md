---
stepsCompleted: [step-01-validate-prerequisites, step-02-design-epics, step-03-create-stories, step-04-final-validation]
status: 'complete'
completedAt: '2026-04-17'
inputDocuments: [prd.md, architecture.md]
---

# NginxApiClient - Epic Breakdown

## Overview

This document provides the complete epic and story breakdown for NginxApiClient, decomposing the requirements from the PRD and Architecture into implementable stories.

## Requirements Inventory

### Functional Requirements

FR1: Consumer can authenticate with an NPM instance using email and password credentials
FR2: Library can automatically obtain a JWT bearer token on first API call
FR3: Library can proactively refresh a JWT token before expiry without consumer intervention
FR4: Library can re-authenticate transparently when a token is rejected (401 response)
FR5: Consumer can configure the NPM instance base URL and credentials at initialization
FR6: Consumer can list all proxy hosts from an NPM instance
FR7: Consumer can retrieve a single proxy host by ID
FR8: Consumer can create a new proxy host with domain names, forward host/port, and scheme
FR9: Consumer can update an existing proxy host's configuration
FR10: Consumer can delete a proxy host by ID
FR11: Consumer can enable or disable a proxy host
FR12: Consumer can configure SSL settings on a proxy host (force SSL, HSTS, HTTP/2)
FR13: Consumer can configure advanced options on a proxy host (websocket upgrade, block exploits, caching, custom nginx directives)
FR14: Consumer can associate a certificate and access list with a proxy host
FR15: Consumer can configure location-based routing on a proxy host
FR16: Consumer can list all certificates from an NPM instance
FR17: Consumer can retrieve a single certificate by ID
FR18: Consumer can provision a new Let's Encrypt certificate via the API
FR19: Consumer can upload a custom SSL certificate
FR20: Consumer can download an existing certificate
FR21: Consumer can delete a certificate by ID
FR22: Consumer can renew an existing certificate
FR23: Consumer can perform full CRUD operations on redirection hosts
FR24: Consumer can enable or disable a redirection host
FR25: Consumer can perform full CRUD operations on dead hosts (404 pages)
FR26: Consumer can enable or disable a dead host
FR27: Consumer can perform full CRUD operations on TCP/UDP streams
FR28: Consumer can enable or disable a stream
FR29: Consumer can perform full CRUD operations on access lists
FR30: Consumer can perform full CRUD operations on NPM users
FR31: Consumer can manage user permissions
FR32: Consumer can retrieve and update NPM settings (default site configuration)
FR33: Consumer can retrieve audit log entries
FR34: Consumer can retrieve host report statistics
FR35: Library surfaces NPM API errors as typed exceptions with HTTP status, NPM error detail, and meaningful messages
FR36: Library distinguishes authentication failures, not-found errors, and general API errors via distinct exception types
FR37: Consumer can catch specific exception types to handle different error scenarios
FR38: Consumer can install a core package with no serializer dependency
FR39: Consumer can choose System.Text.Json or Newtonsoft.Json serialization via separate packages
FR40: Consumer can register the client in an ASP.NET Core DI container via IServiceCollection extension method
FR41: Consumer can instantiate the client manually without DI for non-ASP.NET scenarios
FR42: Consumer can mock the client interface (INginxProxyManagerClient) in unit tests
FR43: Consumer can invoke all API operations asynchronously
FR44: Consumer can pass a CancellationToken to any API operation to support cooperative cancellation
FR45: Consumer can discover all public types, methods, and properties via IntelliSense (XML doc comments)
FR46: Consumer can follow a quick-start guide to make their first API call within 5 minutes
FR47: Consumer can reference runnable example projects for common scenarios
FR48: Consumer can step into library source code during debugging via symbol packages

### NonFunctional Requirements

NFR1: All API method calls add less than 50ms overhead beyond network latency (serialization + deserialization)
NFR2: Token refresh adds at most one additional HTTP round-trip, not blocking concurrent calls
NFR3: Library introduces zero background threads or timers when idle
NFR4: Memory allocation per API call is proportional to response size
NFR5: Credentials (email/password) are never logged, serialized to disk, or exposed in exception messages
NFR6: JWT tokens are stored only in memory — never persisted to disk or included in diagnostic output
NFR7: All HTTP communication uses the scheme provided by the consumer (no HTTPS downgrade)
NFR8: Exception messages include NPM error details but never include credentials or tokens
NFR9: Library is compatible with IHttpClientFactory patterns
NFR10: Library supports manual HttpClient injection for scenarios without DI
NFR11: Core package has zero external runtime dependencies
NFR12: All public API types are CLS-compliant across all target frameworks
NFR13: All API methods are safe to call concurrently from multiple threads
NFR14: Token refresh under concurrent access uses a single refresh call, not one per waiting thread
NFR15: Transient HTTP failures are surfaced immediately — no built-in retry logic
NFR16: Library gracefully handles NPM instance unavailability with clear exception messages
NFR17: All public API surfaces are covered by XML doc comments
NFR18: NuGet package versioning follows SemVer 2.0
NFR19: CI pipeline validates builds across all target frameworks on every commit

### Additional Requirements

- Architecture specifies solution scaffolding via `dotnet new` CLI commands as first implementation step
- Multi-package solution: NginxApiClient (core, netstandard2.0), NginxApiClient.SystemTextJson (net8.0;net10.0), NginxApiClient.NewtonsoftJson (netstandard2.0)
- Directory.Build.props for shared build properties (SourceLink, .snupkg, metadata)
- Directory.Packages.props for Central Package Management
- IJsonSerializer interface in core — SystemTextJsonSerializer and NewtonsoftJsonSerializer in implementation packages
- AuthenticationDelegatingHandler for transparent JWT token lifecycle
- ErrorHandlingDelegatingHandler for HTTP-to-exception mapping
- Per-resource sub-interfaces (IProxyHostClient, ICertificateClient, etc.) accessed via INginxProxyManagerClient properties
- TokenStore with SemaphoreSlim(1,1) for thread-safe token management
- Internal implementation classes, public interfaces and models only
- GitHub Actions CI with matrix build across all target frameworks
- Integration tests against Dockerized NPM instance
- .editorconfig for code style enforcement

### UX Design Requirements

N/A — No UI. This is a library/SDK.

### FR Coverage Map

| FR | Epic | Description |
|----|------|-------------|
| FR1 | Epic 2 | Authenticate with email/password |
| FR2 | Epic 2 | Auto-obtain JWT on first call |
| FR3 | Epic 2 | Proactive token refresh |
| FR4 | Epic 2 | Re-auth on 401 |
| FR5 | Epic 2 | Configure base URL and credentials |
| FR6 | Epic 3 | List proxy hosts |
| FR7 | Epic 3 | Get proxy host by ID |
| FR8 | Epic 3 | Create proxy host |
| FR9 | Epic 3 | Update proxy host |
| FR10 | Epic 3 | Delete proxy host |
| FR11 | Epic 3 | Enable/disable proxy host |
| FR12 | Epic 3 | SSL settings on proxy host |
| FR13 | Epic 3 | Advanced options on proxy host |
| FR14 | Epic 3 | Associate cert/ACL with proxy host |
| FR15 | Epic 3 | Location-based routing |
| FR16 | Epic 4 | List certificates |
| FR17 | Epic 4 | Get certificate by ID |
| FR18 | Epic 4 | Provision Let's Encrypt cert |
| FR19 | Epic 4 | Upload custom cert |
| FR20 | Epic 4 | Download certificate |
| FR21 | Epic 4 | Delete certificate |
| FR22 | Epic 4 | Renew certificate |
| FR23–FR24 | Epic 6 | Redirection hosts CRUD + enable/disable |
| FR25–FR26 | Epic 6 | Dead hosts CRUD + enable/disable |
| FR27–FR28 | Epic 6 | Streams CRUD + enable/disable |
| FR29 | Epic 6 | Access lists CRUD |
| FR30–FR31 | Epic 6 | Users CRUD + permissions |
| FR32 | Epic 6 | Settings get/update |
| FR33 | Epic 6 | Audit log list |
| FR34 | Epic 6 | Reports/host statistics |
| FR35 | Epic 2 | Typed exceptions with NPM error detail |
| FR36 | Epic 2 | Distinct exception types |
| FR37 | Epic 2 | Catchable specific exceptions |
| FR38 | Epic 1 | Core package with no serializer dep |
| FR39 | Epic 1 | STJ/Newtonsoft via separate packages |
| FR40 | Epic 5 | IServiceCollection DI registration |
| FR41 | Epic 5 | Manual construction without DI |
| FR42 | Epic 5 | Mock INginxProxyManagerClient |
| FR43 | Epic 1 | All operations async |
| FR44 | Epic 1 | CancellationToken on all operations |
| FR45 | Epic 7 | XML doc comments / IntelliSense |
| FR46 | Epic 7 | Quick-start guide |
| FR47 | Epic 7 | Runnable example projects |
| FR48 | Epic 7 | Symbol packages for debugging |

## Epic List

### Epic 1: Project Foundation & Core Abstractions
Developer can install the multi-package solution; all projects build across all target frameworks. Core interfaces (IJsonSerializer, INginxProxyManagerClient), serialization implementations, exception hierarchy, and async patterns are in place.
**FRs covered:** FR38, FR39, FR43, FR44

### Epic 2: Authentication & HTTP Pipeline
Developer can authenticate with an NPM instance. The DelegatingHandler pipeline transparently manages JWT tokens (obtain, refresh, re-auth on 401) and maps HTTP errors to typed exceptions.
**FRs covered:** FR1, FR2, FR3, FR4, FR5, FR35, FR36, FR37

### Epic 3: Proxy Host Management
Developer can fully manage proxy hosts — list, get, create, update, delete, enable/disable — with all configuration options (SSL, advanced settings, locations).
**FRs covered:** FR6, FR7, FR8, FR9, FR10, FR11, FR12, FR13, FR14, FR15

### Epic 4: Certificate Management
Developer can manage certificates — list, get, create, delete, upload, download, renew, and provision Let's Encrypt certificates.
**FRs covered:** FR16, FR17, FR18, FR19, FR20, FR21, FR22

### Epic 5: Consumer Integration & Testability
Developer can integrate the client into their application via IServiceCollection DI or manual construction, and mock the client interface in unit tests.
**FRs covered:** FR40, FR41, FR42

### Epic 6: Phase 2 Resource Groups
Developer can manage all remaining NPM resources — redirection hosts, dead hosts, streams, access lists, users, settings, audit log, and reports.
**FRs covered:** FR23, FR24, FR25, FR26, FR27, FR28, FR29, FR30, FR31, FR32, FR33, FR34

### Epic 7: Documentation, Examples & Publishing
Library is published on NuGet.org with .snupkg symbol packages, comprehensive README, XML doc comments, and runnable example projects. Developer can discover and start using the library within 5 minutes.
**FRs covered:** FR45, FR46, FR47, FR48

## Epic 1: Project Foundation & Core Abstractions

Developer can install the multi-package solution; all projects build across all target frameworks. Core interfaces, serialization implementations, exception hierarchy, and async patterns are in place.

### Story 1.1: Solution Scaffolding & Build Infrastructure

As a developer,
I want a multi-project .NET solution with shared build properties,
So that all packages build consistently across target frameworks.

**Acceptance Criteria:**

**Given** a fresh clone of the repository
**When** I run `dotnet build`
**Then** all projects compile successfully
**And** `NginxApiClient` targets `netstandard2.0`
**And** `NginxApiClient.SystemTextJson` targets `net8.0;net10.0`
**And** `NginxApiClient.NewtonsoftJson` targets `netstandard2.0`
**And** `Directory.Build.props` contains shared package metadata (authors, license, repository URL, tags)
**And** `Directory.Packages.props` enables Central Package Management
**And** `.editorconfig` enforces consistent code style
**And** test projects target `net8.0`

### Story 1.2: Serialization Abstraction & Implementations

As a developer,
I want an `IJsonSerializer` interface with System.Text.Json and Newtonsoft.Json implementations,
So that I can choose my preferred serializer without the core package imposing a dependency.

**Acceptance Criteria:**

**Given** the core `NginxApiClient` package
**When** I inspect its dependencies
**Then** it has zero external runtime dependencies
**And** `IJsonSerializer` defines `T Deserialize<T>(string json)` and `string Serialize<T>(T value)` methods

**Given** the `NginxApiClient.SystemTextJson` package
**When** I create a `SystemTextJsonSerializer` instance
**Then** it implements `IJsonSerializer`
**And** it serializes/deserializes using `snake_case` JSON property naming
**And** it handles `PascalCase` C# properties to `snake_case` JSON mapping

**Given** the `NginxApiClient.NewtonsoftJson` package
**When** I create a `NewtonsoftJsonSerializer` instance
**Then** it implements `IJsonSerializer`
**And** it serializes/deserializes using `snake_case` JSON property naming
**And** it produces identical JSON output as `SystemTextJsonSerializer` for the same input

### Story 1.3: Exception Hierarchy

As a developer,
I want typed exceptions for NPM API errors,
So that I can catch and handle specific error scenarios in my code.

**Acceptance Criteria:**

**Given** the exception hierarchy in the core package
**Then** `NginxApiException` is the base exception class with properties: `StatusCode` (int), `ErrorDetail` (string), `RawResponse` (string)
**And** `NginxAuthenticationException` extends `NginxApiException` for 401 errors
**And** `NginxNotFoundException` extends `NginxApiException` for 404 errors
**And** no exception's `Message` or `ToString()` output contains credentials or JWT tokens
**And** all exceptions are serializable

### Story 1.4: Core Client Interfaces

As a developer,
I want the `INginxProxyManagerClient` root interface with per-resource sub-interface properties,
So that I can access typed API methods with focused IntelliSense per resource group.

**Acceptance Criteria:**

**Given** the `INginxProxyManagerClient` interface
**Then** it exposes properties: `ProxyHosts` (returns `IProxyHostClient`), `Certificates` (returns `ICertificateClient`)
**And** Phase 2 resource properties are defined: `RedirectionHosts`, `DeadHosts`, `Streams`, `AccessLists`, `Users`, `Settings`, `AuditLog`, `Reports`
**And** all per-resource interfaces define async methods returning `Task<T>` with `CancellationToken cancellationToken = default`
**And** `IProxyHostClient` defines: `ListAsync`, `GetAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `EnableAsync`, `DisableAsync`
**And** `ICertificateClient` defines: `ListAsync`, `GetAsync`, `CreateAsync`, `DeleteAsync`, `UploadAsync`, `DownloadAsync`, `RenewAsync`

## Epic 2: Authentication & HTTP Pipeline

Developer can authenticate with an NPM instance with transparent token lifecycle and typed error handling.

### Story 2.1: Client Options & Token Models

As a developer,
I want to configure the NPM instance URL and credentials at initialization,
So that the library knows which NPM instance to communicate with.

**Acceptance Criteria:**

**Given** `NginxProxyManagerClientOptions`
**Then** it has properties: `BaseUrl` (string), `Credentials` with `Email` (string) and `Password` (string)
**And** `TokenRequest` model has `Identity` and `Secret` properties mapping to NPM's `identity`/`secret` JSON fields
**And** `TokenResponse` model has `Token` (string) and `Expires` (DateTime) properties
**And** options validation throws `ArgumentException` for null/empty `BaseUrl`, `Email`, or `Password`

### Story 2.2: Token Store & Thread-Safe Token Management

As a developer,
I want the library to store and manage JWT tokens thread-safely,
So that concurrent API calls share a single token and refresh doesn't race.

**Acceptance Criteria:**

**Given** a `TokenStore` instance
**When** multiple threads request the token simultaneously before any token exists
**Then** only one authentication call is made to NPM
**And** all threads receive the same token

**Given** a stored token that is about to expire (within refresh threshold)
**When** an API call is made
**Then** the token is refreshed proactively before sending the request
**And** the stored token never appears in logs or diagnostic output

### Story 2.3: Authentication Delegating Handler

As a developer,
I want transparent JWT authentication on every API call,
So that I never manually manage tokens or deal with 401 errors from expired tokens.

**Acceptance Criteria:**

**Given** an `AuthenticationDelegatingHandler` in the HTTP pipeline
**When** any HTTP request is sent through the pipeline
**Then** the handler adds an `Authorization: Bearer {token}` header

**Given** a request that receives a 401 response
**When** the handler intercepts the 401
**Then** it acquires a new token via the token endpoint
**And** retries the original request exactly once with the new token
**And** if the retry also fails with 401, throws `NginxAuthenticationException`

**Given** concurrent requests that all receive 401
**When** multiple threads trigger re-authentication simultaneously
**Then** only one token refresh is performed (via `SemaphoreSlim`)
**And** all waiting threads use the refreshed token

### Story 2.4: Error Handling Delegating Handler

As a developer,
I want HTTP errors automatically mapped to typed exceptions,
So that I can catch specific exception types without parsing HTTP responses myself.

**Acceptance Criteria:**

**Given** an `ErrorHandlingDelegatingHandler` in the HTTP pipeline
**When** a response with status 404 is received
**Then** a `NginxNotFoundException` is thrown with the status code and NPM error detail

**When** a response with status 4xx (non-401, non-404) or 5xx is received
**Then** a `NginxApiException` is thrown with `StatusCode`, `ErrorDetail` parsed from response body, and `RawResponse`

**When** a network error occurs (timeout, connection refused)
**Then** a `NginxApiException` is thrown with the original exception as `InnerException`
**And** the exception message describes the connectivity issue without exposing credentials

## Epic 3: Proxy Host Management

Developer can fully manage proxy hosts with all configuration options.

### Story 3.1: Proxy Host Models

As a developer,
I want strongly-typed models for proxy host requests and responses,
So that I get IntelliSense and compile-time safety when working with proxy hosts.

**Acceptance Criteria:**

**Given** proxy host models in `NginxApiClient.Models.ProxyHosts`
**Then** `ProxyHostResponse` includes: `Id`, `DomainNames`, `ForwardScheme`, `ForwardHost`, `ForwardPort`, `CachingEnabled`, `AllowWebsocketUpgrade`, `BlockExploits`, `AccessListId`, `CertificateId`, `SslForced`, `HstsEnabled`, `HstsSubdomains`, `Http2Support`, `Enabled`, `Locations`, `AdvancedConfig`
**And** `CreateProxyHostRequest` includes all settable properties with XML doc comments
**And** `UpdateProxyHostRequest` includes all updatable properties
**And** `ProxyHostLocation` models the `locations[]` array with `Path`, `ForwardScheme`, `ForwardHost`, `ForwardPort`, `AdvancedConfig`
**And** all models are POCOs with no serializer-specific attributes

### Story 3.2: Proxy Host Client — CRUD Operations

As a developer,
I want to list, get, create, update, and delete proxy hosts,
So that I can programmatically manage my NPM proxy configuration.

**Acceptance Criteria:**

**Given** an authenticated `INginxProxyManagerClient`
**When** I call `client.ProxyHosts.ListAsync()`
**Then** I receive an `IReadOnlyList<ProxyHostResponse>` with all proxy hosts

**When** I call `client.ProxyHosts.GetAsync(id)`
**Then** I receive the `ProxyHostResponse` for that ID
**And** if the ID doesn't exist, `NginxNotFoundException` is thrown

**When** I call `client.ProxyHosts.CreateAsync(request)` with valid domain names, forward host, and port
**Then** the proxy host is created and the response includes the assigned ID

**When** I call `client.ProxyHosts.UpdateAsync(id, request)`
**Then** the proxy host configuration is updated

**When** I call `client.ProxyHosts.DeleteAsync(id)`
**Then** the proxy host is removed

### Story 3.3: Proxy Host Client — Enable/Disable & Advanced Configuration

As a developer,
I want to enable/disable proxy hosts and configure SSL, websockets, locations, and custom nginx directives,
So that I have full control over proxy host behavior.

**Acceptance Criteria:**

**Given** an existing proxy host
**When** I call `client.ProxyHosts.DisableAsync(id)`
**Then** the proxy host is disabled and traffic stops flowing through it

**When** I call `client.ProxyHosts.EnableAsync(id)`
**Then** the proxy host is re-enabled

**When** I create a proxy host with `SslForced = true`, `HstsEnabled = true`, `Http2Support = true`
**Then** the proxy host is created with those SSL settings applied

**When** I create a proxy host with `Locations` containing path-based routing entries
**Then** the proxy host routes requests to different backends based on URL path

**When** I create a proxy host with `AdvancedConfig` containing custom nginx directives
**Then** the custom directives are included in the generated nginx configuration

### Story 3.4: Proxy Host Unit Tests

As a developer,
I want comprehensive unit tests for the proxy host client,
So that I can verify the client behaves correctly without a live NPM instance.

**Acceptance Criteria:**

**Given** unit tests using `MockHttpMessageHandler`
**Then** `ListAsync_ReturnsProxyHosts_WhenHostsExist` verifies deserialization of list response
**And** `GetAsync_ReturnsProxyHost_WhenIdExists` verifies single host retrieval
**And** `GetAsync_ThrowsNotFoundException_WhenIdNotFound` verifies 404 handling
**And** `CreateAsync_ReturnsCreatedHost_WithValidRequest` verifies creation with all properties
**And** `CreateAsync_ThrowsNginxApiException_WhenRequestInvalid` verifies 422 error handling
**And** `DeleteAsync_Succeeds_WhenIdExists` verifies deletion
**And** `EnableAsync_Succeeds_WhenIdExists` verifies enable operation
**And** all tests verify correct HTTP method, URL path, and request body serialization

## Epic 4: Certificate Management

Developer can manage certificates including Let's Encrypt provisioning.

### Story 4.1: Certificate Models

As a developer,
I want strongly-typed models for certificate operations,
So that I get IntelliSense and compile-time safety when managing certificates.

**Acceptance Criteria:**

**Given** certificate models in `NginxApiClient.Models.Certificates`
**Then** `CertificateResponse` includes: `Id`, `Provider`, `NiceName`, `DomainNames`, `ExpiresOn`, `CreatedOn`, `ModifiedOn`
**And** `CreateCertificateRequest` supports Let's Encrypt provisioning with `DomainNames` and `Provider` fields
**And** `UploadCertificateRequest` supports custom certificate upload with certificate and key content
**And** all models are POCOs with no serializer-specific attributes

### Story 4.2: Certificate Client — CRUD & Let's Encrypt

As a developer,
I want to list, get, create, delete, upload, download, and renew certificates,
So that I can automate certificate lifecycle management.

**Acceptance Criteria:**

**Given** an authenticated `INginxProxyManagerClient`
**When** I call `client.Certificates.ListAsync()`
**Then** I receive all certificates with their expiry dates

**When** I call `client.Certificates.CreateAsync(request)` with Let's Encrypt provider and domain names
**Then** a Let's Encrypt certificate is provisioned for those domains

**When** I call `client.Certificates.UploadAsync(request)` with certificate and key content
**Then** a custom certificate is uploaded and available for use

**When** I call `client.Certificates.DownloadAsync(id)`
**Then** the certificate content is returned

**When** I call `client.Certificates.RenewAsync(id)`
**Then** the certificate is renewed

**When** I call `client.Certificates.DeleteAsync(id)`
**Then** the certificate is removed

### Story 4.3: Certificate Unit Tests

As a developer,
I want comprehensive unit tests for the certificate client,
So that I can verify certificate operations work correctly.

**Acceptance Criteria:**

**Given** unit tests using `MockHttpMessageHandler`
**Then** tests cover list, get, create (Let's Encrypt), upload, download, renew, and delete operations
**And** tests verify correct HTTP method and URL path for each operation
**And** tests verify error handling for invalid certificate requests
**And** tests verify proper serialization of certificate request/response models

## Epic 5: Consumer Integration & Testability

Developer can integrate the client via DI or manual construction and mock it in tests.

### Story 5.1: DI Integration via IServiceCollection

As a developer,
I want to register NginxApiClient in my ASP.NET Core DI container with a single extension method call,
So that I can inject `INginxProxyManagerClient` into my services.

**Acceptance Criteria:**

**Given** the `NginxApiClient.SystemTextJson` package (or `NewtonsoftJson`)
**When** I call `services.AddNginxApiClient(options => { options.BaseUrl = "..."; options.Credentials = new(...); })`
**Then** `INginxProxyManagerClient` is registered as a singleton in the DI container
**And** `HttpClient` is registered via `IHttpClientFactory` with the DelegatingHandler pipeline configured
**And** the appropriate `IJsonSerializer` implementation is registered
**And** I can inject `INginxProxyManagerClient` into my controllers/services

### Story 5.2: Manual Client Construction

As a developer,
I want to create a client instance manually without DI,
So that I can use NginxApiClient in console apps, scripts, and non-ASP.NET scenarios.

**Acceptance Criteria:**

**Given** a manually created `HttpClient` and `IJsonSerializer`
**When** I construct `new NginxProxyManagerClient(httpClient, serializer, options)`
**Then** the client is fully functional with authentication and error handling
**And** the DelegatingHandler pipeline is configured correctly
**And** the consumer controls `HttpClient` lifetime

### Story 5.3: Client Mocking in Consumer Tests

As a developer,
I want to mock `INginxProxyManagerClient` and its sub-interfaces in my unit tests,
So that I can test my code without a live NPM instance.

**Acceptance Criteria:**

**Given** a consumer project with a dependency on `INginxProxyManagerClient`
**When** I mock `INginxProxyManagerClient` using a mocking framework (e.g., Moq, NSubstitute)
**Then** I can set up return values for `client.ProxyHosts.ListAsync()` and other methods
**And** I can verify that my code called the expected client methods
**And** all per-resource sub-interfaces are independently mockable

## Epic 6: Phase 2 Resource Groups

Developer can manage all remaining NPM resources.

### Story 6.1: Redirection Host Client

As a developer,
I want to manage redirection hosts — list, get, create, update, delete, enable, disable,
So that I can automate URL redirection configuration.

**Acceptance Criteria:**

**Given** an authenticated client
**When** I use `client.RedirectionHosts` methods
**Then** all CRUD + enable/disable operations work following the same pattern as proxy hosts
**And** models include `DomainNames`, `ForwardScheme`, `ForwardDomainName`, `PreservePath`, `HttpStatusCode`
**And** unit tests cover all operations

### Story 6.2: Dead Host (404) Client

As a developer,
I want to manage dead hosts (custom 404 pages),
So that I can automate 404 page configuration.

**Acceptance Criteria:**

**Given** an authenticated client
**When** I use `client.DeadHosts` methods
**Then** all CRUD + enable/disable operations work following the established pattern
**And** unit tests cover all operations

### Story 6.3: Stream Client

As a developer,
I want to manage TCP/UDP streams,
So that I can automate stream forwarding configuration.

**Acceptance Criteria:**

**Given** an authenticated client
**When** I use `client.Streams` methods
**Then** all CRUD + enable/disable operations work following the established pattern
**And** models include `IncomingPort`, `ForwardingHost`, `ForwardingPort`, `TcpForwarding`, `UdpForwarding`
**And** unit tests cover all operations

### Story 6.4: Access List Client

As a developer,
I want to manage access lists,
So that I can automate IP-based and authentication-based access restrictions.

**Acceptance Criteria:**

**Given** an authenticated client
**When** I use `client.AccessLists` methods
**Then** all CRUD operations work following the established pattern
**And** models support authorization entries and client IP restrictions
**And** unit tests cover all operations

### Story 6.5: User Administration Client

As a developer,
I want to manage NPM users and their permissions,
So that I can automate user provisioning and access control.

**Acceptance Criteria:**

**Given** an authenticated client
**When** I use `client.Users` methods
**Then** all CRUD operations work following the established pattern
**And** I can manage user permissions
**And** unit tests cover all operations

### Story 6.6: Settings, Audit Log & Reports Clients

As a developer,
I want to retrieve/update settings, view audit logs, and access host reports,
So that I have complete visibility into my NPM instance.

**Acceptance Criteria:**

**Given** an authenticated client
**When** I call `client.Settings.GetAsync()` and `client.Settings.UpdateAsync(request)`
**Then** I can read and modify the default site settings

**When** I call `client.AuditLog.ListAsync()`
**Then** I receive audit log entries

**When** I call `client.Reports.GetHostsAsync()`
**Then** I receive host statistics

**And** unit tests cover all three clients

## Epic 7: Documentation, Examples & Publishing

Library is published on NuGet with comprehensive docs and runnable examples.

### Story 7.1: XML Doc Comments & Symbol Packages

As a developer,
I want IntelliSense documentation on every public type, method, and property, and symbol packages for debugging,
So that I can discover and understand the API without leaving my IDE.

**Acceptance Criteria:**

**Given** any public type in the library
**Then** it has `<summary>`, `<param>`, `<returns>`, and `<exception>` XML doc comments
**And** `TreatWarningsAsErrors` is enabled so missing docs cause build failures
**And** `.snupkg` symbol packages are generated with SourceLink configured
**And** consumers can step into library source code during debugging

### Story 7.2: README & Quick-Start Guide

As a developer,
I want a README with installation instructions and a quick-start guide,
So that I can make my first API call within 5 minutes.

**Acceptance Criteria:**

**Given** the README.md in the repository root
**Then** it includes installation instructions for both modern .NET and .NET Framework 4.8
**And** a quick-start code example showing: install, authenticate, list proxy hosts in under 10 lines
**And** a DI registration example for ASP.NET Core
**And** a create proxy host with SSL example
**And** links to the examples/ folder for more scenarios

### Story 7.3: Runnable Example Projects

As a developer,
I want runnable console project examples,
So that I can copy and adapt real-world scenarios.

**Acceptance Criteria:**

**Given** the `examples/BasicUsage` project (net8.0)
**Then** it demonstrates: authenticate, list proxy hosts, create a proxy host, delete a proxy host

**Given** the `examples/CertificateManagement` project (net8.0)
**Then** it demonstrates: list certificates, check expiry, provision Let's Encrypt cert, renew cert

**Given** the `examples/LegacyFrameworkUsage` project (net48)
**Then** it demonstrates the same basic usage with `NginxApiClient.NewtonsoftJson`

**And** all examples compile and include inline comments explaining each step

### Story 7.4: CI/CD Pipeline & NuGet Publishing

As a developer,
I want automated CI/CD that builds, tests, and publishes to NuGet on release,
So that every release is consistently built and published.

**Acceptance Criteria:**

**Given** a pull request to the repository
**When** GitHub Actions CI runs
**Then** it builds all projects across all target frameworks
**And** runs all unit tests
**And** the build fails if any test fails or XML doc warning is present

**Given** a release tag is pushed (e.g., `v1.0.0`)
**When** the publish workflow runs
**Then** all three NuGet packages are published to NuGet.org
**And** `.snupkg` symbol packages are published alongside
**And** package version matches the release tag
