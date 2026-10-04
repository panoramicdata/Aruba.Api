# Aruba.Api — Design

This document describes how the repository is laid out and the design decisions behind the
`Aruba.Api` client. Read this first, then see [PLAN.md](PLAN.md) for the development roadmap.

## What this package is

A strongly-typed .NET 10 client for the **HPE Aruba Networking Central (New Central)** REST API —
the HPE GreenLake-hosted product, **not** the legacy/classic Aruba Central API. It wraps the API
behind [Refit](https://github.com/reactiveui/refit) interfaces and handles OAuth 2.0 authentication,
token refresh, transient-error retries, and read-only safety.

## Repository layout

```
Aruba.Api/                         # repo root
├─ Aruba.Api/                      # the shipping library (PackageId: PanoramicData.Aruba.Api)
│  ├─ ArubaCentralClient.cs        # the public entry point; exposes one property per API area
│  ├─ ArubaCentralClientOptions.cs # immutable options (required init members)
│  ├─ ArubaCentralClientOptionsBuilder.cs # mutable builder for the DI callback
│  ├─ ArubaCentralClusters.cs      # well-known regional gateway base URLs
│  ├─ AuthenticatedBackingOffHandler.cs   # DelegatingHandler: auth + retry + read-only + logging
│  ├─ ServiceCollectionExtensions.cs      # AddArubaCentralClient(...) DI helpers
│  ├─ GlobalUsings.cs
│  ├─ Authentication/
│  │  ├─ OAuthTokenProvider.cs     # client_credentials token acquisition + in-memory cache
│  │  └─ AccessToken.cs            # cached token + expiry
│  ├─ Converters/
│  │  ├─ BetterJsonStringEnumConverter.cs  # enum (de)serialisation honouring [EnumMember]
│  │  └─ TolerantDateTimeOffsetConverter.cs # handles ""/epoch-ms/ISO date forms
│  ├─ Exceptions/
│  │  ├─ ArubaApiException.cs                 # base
│  │  ├─ ArubaAuthenticationException.cs      # token request failed
│  │  └─ ArubaReadOnlyViolationException.cs   # blocked mutation in read-only mode
│  ├─ Interfaces/                  # Refit interfaces, grouped by New Central API area
│  │  ├─ Monitoring/   (IAccessPoints, IDevices, IClients, ISwitches, IGateways,
│  │  │                 ISiteHealth, ITopology, IApplicationVisibility,
│  │  │                 IFirewallSessions, IClientOnboarding)
│  │  ├─ Notifications/ (IAlerts, IInsights)
│  │  ├─ Reporting/     (IReports)
│  │  ├─ Services/      (IWebhooks, IFirmware)
│  │  ├─ Troubleshooting/ (IAccessPointTroubleshooting, ICxSwitchTroubleshooting,
│  │  │                    IAosSwitchTroubleshooting, IGatewayTroubleshooting, IEvents)
│  │  └─ Msp/           (IMspTenants)
│  └─ Data/                        # request/response models (System.Text.Json, camelCase)
├─ Aruba.Api.Test/                 # single test project, split by sub-namespace
│  ├─ Unit/                        # Aruba.Api.Test.Unit — fully mocked, no network
│  └─ Integration/                 # Aruba.Api.Test.Integration — live, READ-ONLY, fail-hard
├─ .codacy/ + .codacy.yaml         # Codacy CLI config + per-engine excludes
├─ .github/workflows/              # ci.yml (build/pack/publish), codeql.yml
├─ Directory.Build.props           # shared build settings (warnings-as-errors, nullable, docs)
├─ Directory.Packages.props        # Central Package Management — all versions here
├─ global.json / version.json      # SDK pin / Nerdbank.GitVersioning
└─ Publish.ps1                     # tag-and-push release script
```

## Core architecture

### Request pipeline

```
caller → RestService.For<IArea>(httpClient)        (Refit-generated implementation)
       → HttpClient (BaseAddress = regional gateway)
       → AuthenticatedBackingOffHandler (DelegatingHandler)
            1. ValidateReadOnlyMode  — block non-GET when IsReadOnly
            2. attach bearer token   — from OAuthTokenProvider (cached)
            3. log request           — Debug level, secrets masked
            4. send, then:
                 401  → refresh token once, retry
                 429  → honour Retry-After, exponential back-off, retry
                 5xx  → back-off, retry
                 else → return
       → HttpClientHandler (terminal)
```

`OAuthTokenProvider` performs the OAuth 2.0 `client_credentials` grant against HPE GreenLake SSO
(`https://sso.common.cloud.hpe.com/as/token.oauth2`) using its **own** `HttpClient` (to avoid
recursing through the auth handler). Tokens are cached in memory with a configurable expiry
tolerance and refreshed under a `SemaphoreSlim` so only one refresh is in flight at a time.

### Authentication flow

1. `client_id` + `client_secret` → POST to GreenLake SSO → `access_token` (Bearer).
2. Bearer token → regional API gateway (e.g. `https://us2.api.central.arubanetworks.com`).

The regional base URL depends on where the customer account is registered;
`ArubaCentralClusters` provides the well-known ones, but callers can supply any `Uri`.

### JSON conventions

- `System.Text.Json` only (never Newtonsoft).
- `JsonSerializerOptions` uses **camelCase** naming and case-insensitive matching, so most model
  properties need no `[JsonPropertyName]` (the API is camelCase; `Ipv4` ⇄ `ipv4`, etc.).
- `TolerantDateTimeOffsetConverter` is registered for `DateTimeOffset?` because New Central mixes
  empty strings, Unix epoch milliseconds, and ISO-8601 instants for timestamps.
- List endpoints return the shared envelope `PagedResponse<T>` (`Items`, `Count`, `Total`, `Next`).
- Time-series endpoints return `TrendResponse` (`Interval`, `Keys`, `Samples[]`).
- Asynchronous operations return `AsyncOperationResponse` (`Response`, `TaskId`, `Location`).

### Read-only by default (safety)

`ArubaCentralClientOptions.IsReadOnly` defaults to **`true`**. A client constructed without thinking
about it cannot mutate the tenant — any non-`GET` request throws `ArubaReadOnlyViolationException`
(derives from `InvalidOperationException`) before it is sent. Callers must explicitly set
`IsReadOnly = false` to perform writes. This is intentional for monitoring/reporting integrations.

## Conventions and governance

- **Target:** `net10.0`. `TreatWarningsAsErrors`, `Nullable`, `GenerateDocumentationFile` all on.
- **Docs:** public types/members carry XML docs (CS1591 is suppressed only to allow terse models).
- **Central Package Management:** every version lives in `Directory.Packages.props`; never inline a
  `Version=` on a `PackageReference`.
- **Versioning:** Nerdbank.GitVersioning. The package **major version tracks the New Central API
  version** — everything wrapped today is `/v1/`, so the package is `1.x`. A future `/v2/` surface
  bumps `version.json` to `2.0`.
- **Refit interfaces** are grouped to mirror the API areas (`network-monitoring`,
  `network-notifications`, `network-reporting`, `network-services`, `network-troubleshooting`,
  `network-msp`). Route parameters use Refit `{name}` placeholders; kebab-case query keys use
  `[AliasAs("...")]`.
- **Codacy:** `.codacy.yaml` excludes `bin/obj` and—deliberately—excludes `Aruba.Api/Interfaces/**`
  from SonarC#. Refit interfaces use optional parameters (`CancellationToken ct = default`,
  `ListQuery? query = null`), which SonarC# flags as S2360; excluding the interfaces is the
  sanctioned approach across Panoramic Data API packages.
- **CI/CD:** `ci.yml` restores, builds, packs and uploads artifacts on every push/PR, and publishes
  to NuGet via Trusted Publishing on a version tag. **Tests are currently not run in CI** (the suite
  includes live read-only integration tests that fail hard without credentials) — run them locally.
- **Publishing:** `Publish.ps1` enforces a clean tree on `main` synced with origin, derives the NBGV
  version, checks for a duplicate tag, then tags and pushes to trigger the publish job.

## Testing model

One test project, `Aruba.Api.Test`, split by sub-namespace:

- `Aruba.Api.Test.Unit` — fast, fully mocked via `MockHttpMessageHandler` (which answers both the
  SSO token request and API requests). No network.
- `Aruba.Api.Test.Integration` — live, **read-only** calls against a real tenant. Credentials come
  from user-secrets (local) or `Aruba__*` environment variables (CI). These tests **fail** (not
  skip) when credentials are absent — a silently-skipped integration test gives false confidence.
  Tagged `[Trait("Category","Integration")]` for selective runs.

`failSkips: true` is enforced for the whole project — no test ever skips.

## Key decisions (and why)

- **`Dictionary<string,object>` placeholders.** Many Monitoring/Troubleshooting endpoints currently
  return `Dictionary<string,object>` (or `PagedResponse<Dictionary<string,object>>`) rather than a
  typed model. This was a deliberate v1 trade-off: ship correct, complete *interface coverage* of the
  MRT surface without blocking on hand-modelling every response shape. No data is lost. Replacing
  these with typed models is the bulk of the API-coverage work — see [PLAN.md](PLAN.md).
- **Hand-crafted, not generated.** Interfaces and models are written by hand (matching the Panoramic
  Data house style and the paragon `Highlight.Api`), using real example responses from the official
  Postman collections as the source of truth.
- **Refit 12 note.** Refit 12 pulls `ReactiveUI.Primitives` into the dependency graph and its source
  generator forces a `CS0436` suppression in the test project. If a leaner dependency graph is
  preferred, Refit 11.0.1 is a drop-in alternative with neither issue.
