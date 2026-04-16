---
title: "Product Brief: NginxApiClient"
status: "complete"
created: "2026-04-16"
updated: "2026-04-16"
inputs: [web-research, user-interviews, npm-api-analysis, existing-client-survey]
---

# Product Brief: NginxApiClient

## Executive Summary

NGINX Proxy Manager (NPM) is one of the most widely deployed reverse proxy management tools in the Docker ecosystem, with over 100 million Docker pulls and 32,500+ GitHub stars. Its web-based UI makes proxy management accessible, but its REST API — the key to programmatic automation — remains undocumented, untyped, and underserved by client libraries.

NginxApiClient is a comprehensive, idiomatic C# client library for the NGINX Proxy Manager REST API. It covers all 11 API resource groups, supports .NET Framework 4.8 through .NET 10, and ships as a multi-package NuGet solution with full documentation. Despite clients existing in Python, Go, Bash, and PHP, there is currently **zero** .NET coverage — NginxApiClient is the first and only .NET client for NPM on NuGet.

Built by a developer who depends on it for their own infrastructure, the library follows .NET best practices: async/await throughout, proper `HttpClient` lifecycle management, strongly-typed models, and a generic interface architecture that decouples the core API from serialization concerns. Combined with comprehensive documentation that effectively becomes the missing API reference for NPM itself, NginxApiClient transforms NPM from a click-through GUI into a first-class programmable infrastructure primitive for .NET developers.

## The Problem

.NET developers who run NGINX Proxy Manager face a frustrating reality:

- **No programmatic access from C#.** Managing proxy hosts, certificates, redirections, and access lists requires clicking through the web UI manually — or shelling out to curl/bash scripts.
- **The API exists but is undocumented.** NPM exposes a full REST API on port 81, but the official swagger spec is incomplete (missing certificates endpoints, missing DELETE operations). Developers must reverse-engineer endpoints from source code or community scripts (GitHub issues #341, #3749, discussion #3527).
- **Existing clients in other languages are thin.** The Go Terraform provider (Sander0542, 43 releases) is the most mature but is locked to the Terraform ecosystem. The most complete general-purpose tool is a Bash script (92 stars). Python (N4v41, 8 commits) and PHP (eighteen73) libraries exist but offer minimal coverage and no type safety.
- **No type safety anywhere.** Even in languages with existing clients, none offer compile-time safety, IntelliSense support, or structured error handling. Proxy automation errors surface at runtime — potentially at 2am when a certificate expires or a proxy host disappears.

The result: .NET developers building deployment automation, internal dashboards, or infrastructure tooling either avoid the NPM API entirely or write fragile, hand-rolled HTTP calls.

## The Solution

NginxApiClient provides a multi-package NuGet solution that gives .NET developers complete, typed access to every NGINX Proxy Manager API endpoint:

### Package Architecture

| Package | Purpose | Targets |
|---------|---------|---------|
| **NginxApiClient** | Core library: interfaces, models, API abstractions | .NET Standard 2.0 (all platforms) |
| **NginxApiClient.SystemTextJson** | Serialization implementation using System.Text.Json | .NET 8, .NET 10 |
| **NginxApiClient.NewtonsoftJson** | Serialization implementation using Newtonsoft.Json | .NET Framework 4.8, .NET Standard 2.0 |

This generic interface pattern keeps the core library free of serializer dependencies while allowing consumers to choose the implementation that fits their target framework.

### Capabilities

- **Full API coverage** across all 11 resource groups: Tokens (auth), Users, Proxy Hosts, Redirection Hosts, Dead Hosts, Streams, Access Lists, Certificates (including Let's Encrypt provisioning), Settings, Audit Log, and Reports. API surface based on NPM v2.11+ with endpoint discovery from source code analysis, community scripts, and the Terraform provider implementation.
- **Transparent authentication** — handles the JWT bearer token lifecycle (obtain, proactive refresh before expiry, re-authentication on failure) so consumers never deal with 401 errors from expired tokens.
- **Testable by design** — exposes an `INginxProxyManagerClient` interface so consumers can mock the client in their own unit tests without needing a live NPM instance.
- **Async-first design** with proper cancellation token support throughout.
- **Structured error model** — custom exception hierarchy (`NginxApiException`, `NginxAuthenticationException`, `NginxNotFoundException`) that maps NPM API errors to typed, catchable exceptions with meaningful messages.
- **DI integration** — `IServiceCollection` extension methods (`services.AddNginxApiClient(...)`) for seamless ASP.NET Core integration.
- **Comprehensive documentation** — every endpoint documented with request/response examples, a quick-start guide from zero to first API call in under 5 minutes, and real-world usage scenarios.

## What Makes This Different

- **First-mover monopoly on NuGet** — NginxApiClient is the only result for "nginx proxy manager" on NuGet.org. In a niche backed by a 100M+ Docker pull upstream project, this first-mover position compounds over time as the library accumulates downloads, stars, and search ranking.
- **Type safety as risk reduction** — strongly-typed models with compile-time guarantees mean proxy automation errors surface at build time, not in production. This is the structural advantage C# has over every existing Bash/Python/PHP client, and it matters most for infrastructure code.
- **The documentation IS the product** — because NPM's official API docs are incomplete, NginxApiClient's documentation fills a void that attracts developers across all languages. The docs become a community resource and organic discovery channel, not just a library companion.
- **Broadest framework coverage** — the sub-package architecture enables .NET Framework 4.8 support for legacy/enterprise environments alongside .NET 8/10 for modern projects, without forcing serializer choices on anyone.
- **Dogfooded by the author** — built because the author depends on it for their own infrastructure. Libraries maintained by authors who use them personally have lower abandonment risk and higher quality attention.

## Who This Serves

**Primary: .NET developers who self-host with NGINX Proxy Manager.**
Developers running Docker-based infrastructure — homelabs, small business deployments, internal tooling stacks — who want to automate proxy management from C# applications. They build deployment pipelines, admin dashboards, or infrastructure-as-code solutions. They feel underserved in a world of Python scripts and Bash one-liners.

**Secondary: Enterprise .NET teams managing internal infrastructure.**
Organizations running NPM behind the firewall for internal service routing, who need to integrate proxy management into existing .NET-based DevOps tooling. The .NET Framework 4.8 support via `NginxApiClient.NewtonsoftJson` is particularly relevant here.

**Tertiary: Non-.NET developers seeking NPM API reference.**
Because NPM's API is undocumented, NginxApiClient's comprehensive documentation attracts developers from other ecosystems who simply need to understand what the API can do.

## Success Criteria

| Metric | Target |
|--------|--------|
| API coverage | All 11 NPM resource groups with full CRUD operations |
| Framework support | .NET Framework 4.8, .NET 8, .NET 10 — all building and tested |
| Documentation | Every endpoint documented with request/response examples, quick-start guide, usage scenarios |
| NuGet discoverability | Published with optimized tags (`nginx-proxy-manager`, `npm-api`, `reverse-proxy`, `nginx`, `docker`) |
| Personal utility | Used in the author's own projects as the primary NPM integration method |
| Community adoption | Organic NuGet downloads and GitHub engagement within first 6 months |
| NPM version compatibility | Tested against NPM v2.11+ with CI integration tests against a Dockerized NPM instance |

## Scope

**In scope for v1:**
- Core library with generic interface pattern (`NginxApiClient`)
- System.Text.Json serialization package (`NginxApiClient.SystemTextJson`)
- Newtonsoft.Json serialization package (`NginxApiClient.NewtonsoftJson`)
- Complete API client covering all 11 resource groups (scale down if specific endpoints prove non-functional server-side)
- `INginxProxyManagerClient` interface for consumer testability
- `IServiceCollection` extension methods for DI integration
- Automatic JWT token management (login, proactive refresh, re-auth on failure)
- Strongly-typed models for all request/response payloads
- Custom exception hierarchy for structured error handling
- Async API with cancellation token support
- Comprehensive XML doc comments for IntelliSense
- Documentation: API reference with examples, quick-start guide, usage scenarios
- GitHub repository with CI/CD (integration tests against Dockerized NPM, automated NuGet publishing)
- MIT License

**Explicitly out of scope:**
- CLI tool or UI components
- Blazor/ASP.NET UI integration packages
- Support for NPM forks (e.g., NPMplus) beyond what's API-compatible
- Reactive/observable patterns (may be added later)

## Risks & Mitigations

| Risk | Severity | Mitigation |
|------|----------|------------|
| NPM API changes without notice (no versioning guarantees) | High | Pin CI tests to specific NPM Docker tags; maintain a compatibility matrix; version the NuGet package aligned with tested NPM releases |
| API endpoints are broken or partially implemented server-side | Medium | Scope allows scaling down resource groups that don't work; document known limitations per NPM version |
| Multi-target complexity (.NET Fx 4.8 vs modern .NET) | Medium | Sub-package architecture isolates serializer differences; per-target testing in CI |
| NPM project abandoned or superseded | Low | Library's value is tied to NPM's 100M+ install base; even maintenance-mode NPM deployments need automation |

## Vision

NginxApiClient remains a focused, well-maintained client library. Over the next 2-3 years, the goal is to:

- **Keep pace with NPM releases** — update models and endpoints as the NPM API evolves, with a documented compatibility matrix per release
- **Build a rich examples library** — real-world scenarios (bulk host migration, certificate lifecycle automation, backup/restore, environment config sync) that demonstrate the full power of programmatic NPM management
- **Become the reference** — the go-to resource not just for .NET developers, but for anyone wanting to understand what the NPM API can do. An aspirational milestone: referenced from the NPM project's own community resources
