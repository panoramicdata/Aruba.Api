# Migrating to 1.2

Version 1.2 replaces the four per-device troubleshooting interfaces with one shared interface, and moves the
properties that every device type has onto shared base classes. The first is a breaking change to the public API;
the second is not. This page lists every change and why it was made.

Package versions follow the vendor's API version, not our own breaking changes, so this ships as a minor release.
Read this page before upgrading from 1.1.x.

## What changed, at a glance

| Area | 1.1 | 1.2 | Breaking? |
|---|---|---|---|
| Troubleshooting | Four properties on `ArubaCentralClient`, one interface each | One property, `Troubleshooting`, and a `TroubleshootingTarget` argument | **Yes** |
| Troubleshooting commands | Magic strings, and a different method name per interface | `TroubleshootingCommands` names and one `CommandAsync` | **Yes** |
| Device models | `Device`, `AccessPoint`, `Switch` and `Gateway` each declared the same properties | Shared properties live on `DeviceBase` and `ManagedDevice` | No |

## Why

The four troubleshooting interfaces were the same eight operations written out four times, differing only in the
route segment (`aps`, `cx`, `aos-s`, `gateways`). Codacy graded each of them F for 98% duplication, and every real
change had to be made four times and checked four times. The device models had the same problem on a smaller scale:
about fifteen identical properties declared in three or four places.

Collapsing the copies means a change to, say, the reboot endpoint is made once; a new device kind is one enum member
rather than a new interface; and the tests that prove the routes are written once and run against every device kind.

## Troubleshooting

### Client properties

| 1.1 | 1.2 |
|---|---|
| `client.AccessPointTroubleshooting` | `client.Troubleshooting` with `TroubleshootingTarget.AccessPoint` |
| `client.CxSwitchTroubleshooting` | `client.Troubleshooting` with `TroubleshootingTarget.CxSwitch` |
| `client.AosSwitchTroubleshooting` | `client.Troubleshooting` with `TroubleshootingTarget.AosSwitch` |
| `client.GatewayTroubleshooting` | `client.Troubleshooting` with `TroubleshootingTarget.Gateway` |
| `client.Events` | unchanged |

### Interfaces

`IAccessPointTroubleshooting`, `ICxSwitchTroubleshooting`, `IAosSwitchTroubleshooting` and
`IGatewayTroubleshooting` are removed. Use `ITroubleshooting`. `IEvents` is unchanged.

### Methods

Every method now takes the target as its first argument, followed by the serial number as before.

| 1.1 | 1.2 |
|---|---|
| `RebootAsync(serial)` | `RebootAsync(target, serial)` |
| `LocateAsync(serial)` | `LocateAsync(target, serial)` (not available for gateways, as before) |
| `ListTasksAsync(serial)` | `ListTasksAsync(target, serial)` |
| `ShowCommandsAsync(serial, request)` | `ShowCommandsAsync(target, serial, request)` |
| `GetSupportedShowCommandsAsync(serial)` | `GetSupportedShowCommandsAsync(target, serial)` |
| `CommandAsync(serial, request, command)` (access point, CX switch, gateway) | `CommandAsync(target, serial, command, request)` |
| `ExecuteCommandAsync(serial, commandName, command)` (AOS-S switch) | `CommandAsync(target, serial, command, request)` |
| `GetCommandResultAsync(serial, command, taskId)` | `GetCommandResultAsync(target, serial, command, taskId)` |

Two things to watch for beyond the extra argument:

- The request body moved to after the command name. The compiler will catch this.
- The AOS-S switch method was `ExecuteCommandAsync` with a parameter called `commandName`. It is now
  `CommandAsync`, the same as the other three.

### Command names

The commands are still plain strings on the wire. `TroubleshootingCommands` gives them names so they are not typed
by hand:

| Name | Value | Applies to |
|---|---|---|
| `Ping` | `ping` | all |
| `Traceroute` | `traceroute` | all |
| `Speedtest`, `NsLookup`, `DisconnectUserByMacAddress` | `speedtest`, `nslookup`, `disconnectUserByMacAddress` | access points |
| `PoeBounce`, `PortBounce`, `CableTest` | `poeBounce`, `portBounce`, `cableTest` | switches |
| `PingSweep`, `Iperf`, `DisconnectClientByMacAddress` | `pingSweep`, `iperf`, `disconnectClientByMacAddress` | gateways |

The service still answers 404 for a command a device kind does not support; the type system does not stop you
asking.

### Before and after

```csharp
// 1.1
var accepted = await client.AccessPointTroubleshooting.CommandAsync(
    "AP00000001",
    new TroubleshootingCommandRequest { Host = "8.8.8.8", Count = 5 },
    "ping");

var result = await client.AccessPointTroubleshooting.GetCommandResultAsync(
    "AP00000001", "ping", accepted.TaskId!);
```

```csharp
// 1.2
var accepted = await client.Troubleshooting.CommandAsync(
    TroubleshootingTarget.AccessPoint,
    "AP00000001",
    TroubleshootingCommands.Ping,
    new TroubleshootingCommandRequest { Host = "8.8.8.8", Count = 5 });

var result = await client.Troubleshooting.GetCommandResultAsync(
    TroubleshootingTarget.AccessPoint, "AP00000001", TroubleshootingCommands.Ping, accepted.TaskId!);
```

If you call the same device kind throughout, a local constant keeps call sites short:

```csharp
const TroubleshootingTarget Aps = TroubleshootingTarget.AccessPoint;
await client.Troubleshooting.RebootAsync(Aps, "AP00000001");
```

### Dependency injection

If you registered the old interfaces yourself, register `ITroubleshooting` instead. Resolving the client through
`AddArubaCentralClient` needs no change.

## Device models

`DeviceBase` holds what every device reports: `SerialNumber`, `MacAddress`, `DeviceName`, `Model`,
`FirmwareVersion`, `Status`, `DeviceFunction`, `SiteId`, `SiteName`, `Id` and `Type`. `ManagedDevice` adds
`PartNumber`, `Deployment`, `Ipv4`, `Ipv6` and `LastSeenAt` for `Device`, `AccessPoint` and `Switch`.

| Type | Base class in 1.2 |
|---|---|
| `Device` | `ManagedDevice` |
| `AccessPoint` | `ManagedDevice` |
| `Switch` | `ManagedDevice` |
| `Gateway` | `DeviceBase` |

Every property you could read before you can still read, under the same name and type, and JSON is read exactly
as before. Existing code compiles unchanged. The only visible differences are that these types can now be passed
where a `DeviceBase` or `ManagedDevice` is wanted, and that the properties are declared on the base type, which
matters only to code that uses reflection with `BindingFlags.DeclaredOnly`.

## Not changed

Everything else is as it was: authentication, options, read-only mode, the other API areas, exceptions and the
`Events` interface.

## Getting help

If something in 1.1 has no clear equivalent here, open an issue at
<https://github.com/panoramicdata/Aruba.Api/issues> with the call you were making.
