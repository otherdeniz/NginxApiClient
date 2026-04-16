---
title: "Product Brief Distillate: NginxApiClient"
type: llm-distillate
source: "product-brief-NginxApiClient.md"
created: "2026-04-16"
purpose: "Token-efficient context for downstream PRD creation"
---

# Product Brief Distillate: NginxApiClient

## Package Architecture

- Multi-package NuGet solution with generic interface pattern to decouple core API from serialization
- **NginxApiClient** (core): interfaces, models, API abstractions — targets .NET Standard 2.0
- **NginxApiClient.SystemTextJson**: serialization impl for .NET 8 / .NET 10
- **NginxApiClient.NewtonsoftJson**: serialization impl for .NET Framework 4.8 / .NET Standard 2.0
- DI integration via `IServiceCollection` extension methods (`services.AddNginxApiClient(...)`)
- `INginxProxyManagerClient` interface exposed for consumer testability (mocking)
- MIT License (changed from GPL v3 during discovery — GPL is too restrictive for a NuGet library)

## NPM API Surface (from source code, community scripts, Terraform provider analysis)

- **Base path:** `/api` on NPM instance (typically port 81 in Docker)
- **Auth:** POST `/api/tokens` with `identity` (email) + `secret` (password) → JWT bearer token
- **Token lifecycle:** JWT has expiry; client must handle proactive refresh before expiry and reactive re-auth on 401

### Endpoint Map

| Resource | Prefix | Operations | Notes |
|----------|--------|------------|-------|
| Tokens | `/api/tokens` | Create (login), refresh | Auth entry point |
| Users | `/api/users` | CRUD, permissions | Admin operations |
| Proxy Hosts | `/api/nginx/proxy-hosts` | Create, list, get, update, delete, enable, disable | Core resource — most used |
| Redirection Hosts | `/api/nginx/redirection-hosts` | Create, list, get, update, delete, enable, disable | |
| Dead Hosts (404) | `/api/nginx/dead-hosts` | Create, list, get, update, delete, enable, disable | |
| Streams | `/api/nginx/streams` | Create, list, get, update, delete, enable, disable | TCP/UDP forwarding |
| Access Lists | `/api/nginx/access-lists` | CRUD | Auth/IP-based restrictions |
| Certificates | `/api/nginx/certificates` | CRUD + upload/download + Let's Encrypt provision | **Missing from swagger spec** |
| Settings | `/api/settings` | Get, update | Default site settings |
| Audit Log | `/api/audit-log` | List | Read-only |
| Reports | `/api/reports/hosts` | Host statistics | Read-only |
| Schema | `/api/schema` | OpenAPI spec | Incomplete — missing certs, DELETEs |

### Key Proxy Host Properties (from Terraform provider)

- `domain_names` (string array), `forward_scheme` (http/https), `forward_host`, `forward_port`
- `caching_enabled`, `allow_websocket_upgrade`, `block_exploits`
- `access_list_id`, `certificate_id`
- `ssl_forced`, `hsts_enabled`, `hsts_subdomains`, `http2_support`
- `locations[]` (path-based routing)
- `advanced_config` (custom nginx directives — raw string)

## Competitive Landscape

### Existing NPM API clients (no C# client exists anywhere)

| Language | Project | Maturity | Coverage |
|----------|---------|----------|----------|
| Go (Terraform) | Sander0542/terraform-provider-nginxproxymanager | High (43 releases, v1.2.2) | Proxy hosts, certs, access lists — but Terraform-locked |
| Go (Terraform) | home-devops/nginxproxymanager | Low | Alternative provider |
| Bash | Erreur32/nginx-proxy-manager-Bash-API (92 stars) | Medium | Most comprehensive general-purpose tool — covers proxy hosts, redirections, users, ACLs, certs, backups |
| JavaScript | aalasolutions/nginx-proxy-manager-sdk | Low | JS SDK |
| Python | N4v41/NginxPM_api (8 commits) | Low | Minimal, built for automated deploys |
| Python | kmanwar89/nginx_proxy_manager_automation | Low | Automation scripts |
| PHP | eighteen73/nginx-proxy-manager-api | Low | Packagist library |
| PHP (Drupal) | nginx_proxy_manager_connector | Low | Drupal module |
| Ansible | DenAV/nginx-proxy-manager-ansible | Low | Ansible role |

### NuGet ecosystem — confirmed empty

- `Azure.ResourceManager.Nginx` — Azure-managed NGINX, unrelated
- `NginxConfigParser` — parses nginx config files, unrelated
- No NPM API client exists on NuGet.org

## NPM Project Stats

- GitHub stars: ~32,500
- Docker pulls: 100M+ (hub.docker.com/r/jc21/nginx-proxy-manager)
- Forks: ~3,700
- Active fork: ZoeyVid/NPMplus (extends NPM further)
- API based on NPM v2.11+ (target version for client)

## API Documentation Gaps (key GitHub references)

- Discussion #3527: Users complain API is undocumented
- Issue #341: Original API documentation request
- Issue #3749: Request to document all endpoints
- Discussion #3265: Users want programmatic automation for CI/CD
- Discussion #5199: Request for Swagger UI at `/api`
- Swagger spec is incomplete: missing certificates endpoints, missing DELETE operations
- **Best reverse-engineering reference:** the Bash API script (Erreur32, 92 stars) — more complete than official swagger

## Error Model Design

- Custom exception hierarchy: `NginxApiException` (base), `NginxAuthenticationException`, `NginxNotFoundException`
- Map HTTP 4xx/5xx to typed exceptions with meaningful messages
- NPM API returns non-standard errors in some cases — handle gracefully

## Technical Constraints & Decisions

- .NET Framework 4.8 support is required (author needs it) — drives the sub-package architecture
- System.Text.Json on modern targets, Newtonsoft.Json on .NET Framework 4.8
- Async/await throughout with CancellationToken support
- Proper `HttpClient` lifecycle management (avoid socket exhaustion)
- Nullable reference types on modern targets
- JWT token management: proactive refresh before expiry, re-auth on 401 failure, thread-safe for concurrent requests

## Rejected Ideas (do not re-propose)

- **CLI tool on top of the client** — author wants purely the API client library
- **Blazor/ASP.NET UI components** — out of scope
- **Mock server or testing utilities** — out of scope (interface `INginxProxyManagerClient` covers consumer testing needs)
- **Reactive/observable patterns** — may be added later, not v1
- **Support for NPM forks (NPMplus)** — only if API-compatible, not actively targeted
- **GPL v3 license** — replaced with MIT for NuGet ecosystem compatibility
- **Dropping .NET Framework 4.8** — considered but rejected, author needs it

## User & Market Context

- .NET ecosystem: 7M+ developers, C# was TIOBE 2025 Language of the Year
- Primary audience: .NET devs self-hosting with NPM (homelab, small business, internal tools)
- Secondary: enterprise .NET teams on legacy stacks needing proxy automation
- Strong homelab/.NET crossover — many enterprise .NET devs also run personal infrastructure
- No explicit C# demand signal in forums (absence of tooling = absence of focal point for demand)
- Latent demand validated by: Python/Go/Bash clients existing, Terraform providers thriving, multiple GitHub issues requesting API automation

## Open Questions for PRD

- Exact JWT token expiry duration from NPM — affects refresh strategy design
- Whether NPM rate-limits API calls — affects retry/backoff policy needs
- Let's Encrypt provisioning via API — is it synchronous or async/polling? Affects client design for certificate operations
- Which resource groups have fully functional endpoints vs. partially implemented — requires hands-on testing against live NPM instance
- Specific NPM versions to include in the compatibility matrix for v1
