# Aruba.Api — Development Plan

This is the roadmap for taking `Aruba.Api` from its current v1 foundation to **full New Central API
coverage** with **high code coverage**. Read [DESIGN.md](DESIGN.md) first for architecture and
conventions. This plan is the handover document — pick up work from the "Next actions" of whichever
workstream you are advancing.

## Vision / definition of done

1. Every New Central REST endpoint we choose to support is reachable through a typed, documented
   Refit method on `ArubaCentralClient`.
2. Every response is a strongly-typed model (no `Dictionary<string,object>` placeholders remain on
   supported endpoints).
3. Line coverage ≥ 80% (stretch 90%) with meaningful assertions, not just construction.
4. `dotnet build` is warning-free, Codacy reports a clean grade, and the live read-only integration
   suite passes against a real tenant.

## Approach (decided)

- **Hand-craft incrementally**, matching the existing house style and the paragon `Highlight.Api`.
  No code generation. Use the official Postman collections + live tenant responses as the source of
  truth for shapes.
- **Code coverage first**, then expand API coverage — solidify what ships before growing it.
- Work in small, reviewable PRs scoped to one API area at a time. Keep the build green and Codacy
  clean at every step.

## Source of truth

- New Central MRT APIs — `MRT APIs.postman_collection.json` (Monitoring, Reporting, Troubleshooting;
  284 requests).
- New Central Configuration APIs — `Configuration APIs.postman_collection.json` (1,078 requests).
- Connectivity reference — `aruba.cs` (client_credentials → SSO → regional gateway).
- Official docs — https://developer.arubanetworks.com/new-central/docs
- Postman collection — https://www.postman.com/hpe-aruba-networking/new-hpe-aruba-networking-central/

> These collections are large (the Configuration one is ~59 MB). Extract endpoint paths, query
> params, request bodies, and **example responses** programmatically (e.g. a small Python pass over
> the JSON) rather than opening them whole. Build models from the example response payloads.

---

## Workstream A — Code coverage (do this first)

**Baseline (at handover): ~40% line coverage (265/663 lines).** Run:

```sh
dotnet test Aruba.Api.Test/Aruba.Api.Test.csproj -c Release \
  --filter "Category!=Integration" --collect:"XPlat Code Coverage"
```

### Gaps and target tests

| Area | Current | Tests to add |
|------|--------:|--------------|
| `AuthenticatedBackingOffHandler` | ~42% | 429 + `Retry-After` honoured; 5xx back-off; give-up after `MaxAttemptCount`; `CalculateBackoffDelay` edge cases (covered); User-Agent header added; Debug logging path with secret masking |
| `OAuthTokenProvider` | ~90% | non-success token response → `ArubaAuthenticationException`; malformed JSON; missing `access_token`; expiry-tolerance refresh; concurrent callers share one refresh |
| `TolerantDateTimeOffsetConverter` | ~38% | `""`→null, null, epoch-ms number, epoch-ms string, ISO-8601, invalid→`JsonException`, round-trip Write |
| `BetterJsonStringEnumConverter` | 0% | `[EnumMember]` value mapping both directions; unknown value throws; fallback to field name |
| `ServiceCollectionExtensions` | 0% | builder overload registers a usable singleton; options overload; validation throws on missing required values |
| `Data/*` models | mostly 0% | deserialize each model from a **real example response** fixture and assert key fields (doubles as a guard when typing new endpoints) |
| Exceptions | partial | `StatusCode` propagation; `ArubaReadOnlyViolationException.Method/RequestUri` |

### Method

- Add response fixtures under `Aruba.Api.Test/Unit/` mirroring `TestData.cs`, sourced from the
  Postman example responses, and assert deserialization. This is the natural place to lock in each
  model as it is typed in Workstream B.
- Keep using `MockHttpMessageHandler` for end-to-end-through-Refit tests.
- Grow the live `Integration` suite with one read-only test per typed area as models land.

**Next action:** raise handler + converter + DI + model-deserialization coverage to ≥ 80%, then keep
each new endpoint's model covered by a deserialization test as Workstream B proceeds.

---

## Workstream B — API coverage

### B0. Type the existing MRT placeholders (highest value, bounded)

Several already-wrapped Monitoring/Troubleshooting endpoints return `Dictionary<string,object>` or
`PagedResponse<Dictionary<string,object>>`. Replace these with typed models built from example
responses. Known placeholders include:

- **Access Points**: radios, BSSIDs, WLANs, swarms, AP ports/tunnels/radios/WLANs; all `*-trends`
  (model the `TrendResponse` variants).
- **Switches**: stack members, hardware-categories, LAG, interfaces, VLANs, interface-PoE, VSX.
- **Gateways**: ports, VLANs, tunnels, uplinks, DHCP pools/clients, cluster members/tunnels and the
  tunnel-health summaries.
- **Site Health**: per-site / tenant device-health and client-health payloads.
- **Topology**: site topology graph, unmanaged/isolated devices, neighbours.
- **Application Visibility, Firewall Sessions, Client Onboarding**: response shapes.

### B1. Complete the remaining MRT endpoints (284 total; core wrapped, tail remaining)

Not yet wrapped at all:

- **FloorPlan / site-maps** (~34 endpoints): floors, buildings, walls, zones, wall-types, imports,
  device placement (deployed/planned/assigned). Mutating — exercise via `IsReadOnly = false`.
- **Services → Location** (~15): asset-tags + metadata, AP ranging scans, device-locations,
  Wi-Fi client locations; **Location Analytics** (trends, site insights).
- **Full AP/Gateway trend matrix**: the per-radio, per-port, per-tunnel, per-uplink trend endpoints
  not yet individually surfaced.
- **Troubleshooting**: remaining per-device commands (e.g. AP `aaa`/`disconnectUser*`, gateway
  `halt`/`disconnectClient*`, `getArpTable` async pollers) — extend the existing per-device
  interfaces following the established command + `GetCommandResultAsync` pattern.

### B2. Configuration API (1,078 endpoints) — the large effort

Wrap area-by-area (Postman top-level folders). Suggested order (smaller/foundational first), with
request counts to scope each PR:

| # | Area | Reqs | | # | Area | Reqs |
|--:|------|----:|-|--:|------|----:|
| 1 | Scope Management | 49 | | 9 | Roles & Policy | 29 |
| 2 | Assignments | 4 | | 10 | Named Object | 10 |
| 3 | Config Management | 8 | | 11 | High Availability | 6 |
| 4 | Configuration Health | 2 | | 12 | Application Experience | 5 |
| 5 | Firmware Policy | 3 | | 13 | IoT | 5 |
| 6 | Interfaces | 108 | | 14 | Tunnels | 10 |
| 7 | Network Services | 105 | | 15 | Miscellaneous / Extensions | 7 |
| 8 | Security | 155 | | 16 | VLANs & Networks | 51 |
|   | Routing & Overlays | 95 | |  | Wireless | 35 |
|   | System | 200 | |  | Telemetry | 80 |
|   | Central NAC (+ Service) | 87 | |  | Services | 24 |

Most Configuration resources follow a regular CRUD shape (list / get / create / update / delete) per
sub-resource, so each area is repetitive once the first resource in it is modelled. Many are
mutating — they must be reachable only with `IsReadOnly = false`, and unit-tested with the read-only
gate in mind.

**Next action:** start at B0 (type MRT placeholders) — it is bounded, immediately useful, and builds
the model-fixture test habit before the Configuration marathon.

---

## How to add an endpoint (checklist)

1. Find the endpoint + an example response in the relevant Postman collection.
2. Add/extend the Refit interface under `Aruba.Api/Interfaces/<Area>/` (`[Get]`/`[Post]`/… with the
   path relative to the regional gateway). Use `[AliasAs]` for kebab-case query keys.
3. Add a typed model under `Aruba.Api/Data/` from the example payload (camelCase; `DateTimeOffset?`
   for timestamps; list endpoints → `PagedResponse<T>`).
4. Expose the interface as a property on `ArubaCentralClient` (if new).
5. Add a unit test deserializing a real example response; add a read-only integration test if it is
   a GET.
6. `dotnet build` clean, `dotnet test` green, then run the Codacy skill to confirm no new issues.

## Quality gates per PR

- `dotnet build -c Release` — zero warnings.
- `dotnet test --filter "Category!=Integration"` — all green, no skips.
- Live integration smoke (with credentials) for any new GET area.
- Codacy: 0 new issues (`dotnet .github/skills/codacy/Codacy.cs list-issues --repo panoramicdata/Aruba.Api`).
- Coverage does not regress.

## Housekeeping / open items at handover

- **NuGet 1.0.7**: CI pushed it and NuGet accepted it, but it had not appeared on the public feed —
  verify validation status under the NuGet account (Manage Packages) and re-publish if needed.
- **Refit 11 vs 12**: decide whether to keep Refit 12 (adds `ReactiveUI.Primitives` transitively,
  needs the `CS0436` suppression) or revert to the leaner 11.0.1.
- **Codacy skill**: the `codacy` skill in `PanoramicData.Skills` was extended with `add-repo` and
  `list-issues` actions and an `api.codacy.com` lookup URL — ensure those changes are committed there.
