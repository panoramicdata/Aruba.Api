# Aruba.Api

[![NuGet](https://img.shields.io/nuget/v/PanoramicData.Aruba.Api.svg)](https://www.nuget.org/packages/PanoramicData.Aruba.Api)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/bb86df3b2bde46a1b30ecf539f9b5744)](https://app.codacy.com/gh/panoramicdata/Aruba.Api/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4.svg)](https://dotnet.microsoft.com/)

A modern, strongly-typed .NET 10 client for the **HPE Aruba Networking Central (New Central)** REST API.

It handles OAuth 2.0 authentication, token refresh, transient-error retries and read-only safety for you, and exposes each API area through clean, [Refit](https://github.com/reactiveui/refit)-backed interfaces.

> This client targets **New Central** (the HPE GreenLake-hosted Aruba Central). It is not for the legacy/classic Aruba Central API.

## Installation

```sh
dotnet add package PanoramicData.Aruba.Api
```

## Getting your credentials

1. In New Central, create **API client credentials** — see
   [Create client credentials](https://developer.arubanetworks.com/new-central/docs/generating-and-managing-access-tokens#create-client-credentials).
   You will receive a **client_id** and **client_secret**.
2. Find your account's regional **base URL** — see
   [Finding your Base URL](https://developer.arubanetworks.com/new-central/docs/getting-started-with-rest-apis#finding-your-base-url).

The client exchanges your `client_id` / `client_secret` for a bearer token using the OAuth 2.0
`client_credentials` grant against HPE GreenLake SSO (`https://sso.common.cloud.hpe.com`), then
calls the regional API gateway. Tokens are cached in memory and refreshed automatically.

## Quick start

```csharp
using Aruba.Api;

using var client = new ArubaCentralClient(new ArubaCentralClientOptions
{
    BaseAddress = ArubaCentralClusters.UsWest, // or new Uri("https://<your-cluster>.api.central.arubanetworks.com")
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret",
});

// List access points (paged)
var aps = await client.AccessPoints.GetAllAsync(new ListQuery { Limit = 100 });
foreach (var ap in aps.Items)
{
    Console.WriteLine($"{ap.DeviceName} ({ap.SerialNumber}) - {ap.Status} - {ap.ClientCount} clients");
}

// Get site health
var sites = await client.SiteHealth.GetAllSitesHealthAsync();

// List alerts
var alerts = await client.Alerts.GetAllAsync(new ListQuery { Limit = 50 });
```

> **Safe by default:** the client is **read-only unless you opt in**. `IsReadOnly` defaults to
> `true`, so every `GET` works out of the box, but any mutating call (POST/PUT/PATCH/DELETE) throws
> an `InvalidOperationException` *before the request is sent*. See
> [Performing writes](#performing-writes-opt-in) below.

### Cursor-based paging

List endpoints return a `PagedResponse<T>` with `Items`, `Count`, `Total` and `Next`. Pass `Next`
back via `ListQuery` to fetch the following page:

```csharp
string? cursor = null;
do
{
    var page = await client.Clients.GetAllAsync(new ListQuery { Limit = 500, Next = cursor });
    Process(page.Items);
    cursor = page.Next;
}
while (cursor is not null);
```

### Read-only safety (default)

`IsReadOnly` defaults to **`true`** — a client you construct without thinking about it can never
mutate your tenant. Any non-`GET` request throws an `InvalidOperationException` before it leaves the
process. This makes monitoring/reporting integrations safe by construction.

### Performing writes (opt in)

To call mutating endpoints (clear alerts, update a device, manage webhooks, run troubleshooting
commands, …) you must explicitly set `IsReadOnly = false`:

```csharp
using var writeClient = new ArubaCentralClient(new ArubaCentralClientOptions
{
    BaseAddress = ArubaCentralClusters.Europe,
    ClientId = id,
    ClientSecret = secret,
    IsReadOnly = false, // opt in to writes
});

var alerts = await writeClient.Alerts.GetAllAsync(new ListQuery { Limit = 50 });
await writeClient.Alerts.ClearAsync(new AlertActionRequest
{
    AlertIds = alerts.Items.Select(a => a.Id!).ToList(),
});
```

### Live troubleshooting (asynchronous commands)

Troubleshooting commands are asynchronous: they return a task id you poll for the result. They are
mutating (`POST`) operations, so they require a client created with `IsReadOnly = false`
(see [Performing writes](#performing-writes-opt-in)).

```csharp
var accepted = await client.AccessPointTroubleshooting.PingAsync(
    "AP00000001",
    new TroubleshootingCommandRequest { Host = "8.8.8.8", Count = 5 });

var result = await client.AccessPointTroubleshooting.GetCommandResultAsync(
    "AP00000001", command: "ping", taskId: accepted.TaskId!);

Console.WriteLine(result.Output);
```

### Dependency injection

```csharp
services.AddArubaCentralClient(builder =>
{
    builder.BaseAddress = ArubaCentralClusters.UsWest;
    builder.ClientId = configuration["Aruba:ClientId"];
    builder.ClientSecret = configuration["Aruba:ClientSecret"];
    builder.IsReadOnly = true;
});
```

### Logging

Pass any `Microsoft.Extensions.Logging.ILogger`. At `Debug` level the client logs full request and
response detail; the `Authorization` header and `client_secret` are always masked.

## API coverage

The client covers the New Central **MRT** API set (Monitoring, Reporting, Troubleshooting) and
related notification and MSP services. Every area is reached through a property on
`ArubaCentralClient`:

| Area | Property | Notes |
|------|----------|-------|
| Monitoring — Access Points | `AccessPoints` | List/detail, top-N, radios, ports, tunnels, WLANs, key trends |
| Monitoring — Devices | `Devices` | List, inventory, update, delete |
| Monitoring — Clients | `Clients` | List/detail, trend, top-N usage, mobility trail |
| Monitoring — Switches | `Switches` | List/detail, stack, interfaces, VLANs, PoE, VSX, trends |
| Monitoring — Gateways | `Gateways` | List/detail, ports, VLANs, tunnels, uplinks, clusters, trends |
| Monitoring — Site Health | `SiteHealth` | Per-site and tenant device/client health |
| Monitoring — Topology | `Topology` | Site topology, neighbours, isolated/unmanaged devices |
| Monitoring — Application Visibility | `ApplicationVisibility` | Application usage |
| Monitoring — Firewall Sessions | `FirewallSessions` | Site/client firewall sessions |
| Monitoring — Client Onboarding | `ClientOnboarding` | Onboarding score/stages |
| Notifications — Alerts | `Alerts` | List, clear/defer/activate/prioritise, classification |
| Notifications — Insights | `Insights` | List |
| Reporting | `Reports` | List, update, run history |
| Services — Webhooks | `Webhooks` | Full CRUD + HMAC key rotation |
| Services — Firmware | `Firmware` | Firmware details |
| Troubleshooting — Access Points | `AccessPointTroubleshooting` | Ping, traceroute, speed test, show commands, reboot, … |
| Troubleshooting — AOS-CX Switches | `CxSwitchTroubleshooting` | Ping, cable test, port/PoE bounce, show commands, … |
| Troubleshooting — AOS-S Switches | `AosSwitchTroubleshooting` | Ping, cable test, port/PoE bounce, show commands, … |
| Troubleshooting — Gateways | `GatewayTroubleshooting` | Ping (+sweep), iperf, show commands, reboot, … |
| Troubleshooting — Events | `Events` | Events, extra attributes, filters |
| MSP | `MspTenants` | List managed tenants |

Strongly-typed models are provided for the core resources (access points, devices, switches,
gateways, clients, alerts, site health, reports, MSP tenants and more). Less common or
highly-variable payloads are surfaced as `Dictionary<string, object>` so no data is lost while the
typed surface grows; contributions adding typed models are welcome — see
[CONTRIBUTING.md](CONTRIBUTING.md).

> **Not yet wrapped in 1.0:** the New Central **Configuration** API (a separate, very large surface)
> and the floor-plan/site-map editing endpoints. These are planned for future releases.

## Quality

- Targets `net10.0`, builds with `TreatWarningsAsErrors` and nullable reference types enabled.
- Deterministic versioning via [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning).
- Symbols (`snupkg`) published for source debugging.
- Unit tests run on xUnit v3 with `failSkips` enforced.

## Contributing & roadmap

- [DESIGN.md](DESIGN.md) — repository layout, architecture, and conventions.
- [PLAN.md](PLAN.md) — roadmap to full API coverage and high code coverage.
- [CONTRIBUTING.md](CONTRIBUTING.md) — how to add an endpoint and the quality gates.

## Links

- [NuGet package](https://www.nuget.org/packages/PanoramicData.Aruba.Api)
- [GitHub repository](https://github.com/panoramicdata/Aruba.Api)
- [Issue tracker](https://github.com/panoramicdata/Aruba.Api/issues)
- [New Central API documentation](https://developer.arubanetworks.com/new-central/docs)

## License

Licensed under the [MIT License](LICENSE).
