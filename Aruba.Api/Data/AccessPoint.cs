namespace Aruba.Api.Data;

/// <summary>
/// An access point as reported by the New Central network-monitoring service.
/// </summary>
public sealed class AccessPoint
{
	/// <summary>The device serial number.</summary>
	public string? SerialNumber { get; init; }

	/// <summary>The device MAC address.</summary>
	public string? MacAddress { get; init; }

	/// <summary>The friendly device name.</summary>
	public string? DeviceName { get; init; }

	/// <summary>The hardware model, for example <c>AP-115</c>.</summary>
	public string? Model { get; init; }

	/// <summary>The manufacturer part number.</summary>
	public string? PartNumber { get; init; }

	/// <summary>The deployment type, for example <c>Standalone</c>.</summary>
	public string? Deployment { get; init; }

	/// <summary>The device role, for example <c>Conductor</c>.</summary>
	public string? Role { get; init; }

	/// <summary>The device function, for example <c>Instant</c>.</summary>
	public string? DeviceFunction { get; init; }

	/// <summary>The running firmware version.</summary>
	public string? FirmwareVersion { get; init; }

	/// <summary>The IPv4 address.</summary>
	public string? Ipv4 { get; init; }

	/// <summary>The IPv6 address.</summary>
	public string? Ipv6 { get; init; }

	/// <summary>The public IPv4 address.</summary>
	public string? PublicIpv4 { get; init; }

	/// <summary>The connection status, for example <c>ONLINE</c> or <c>OFFLINE</c>.</summary>
	public string? Status { get; init; }

	/// <summary>The time the device was last seen, when available.</summary>
	public DateTimeOffset? LastSeenAt { get; init; }

	/// <summary>The identifier of the site the device belongs to.</summary>
	public string? SiteId { get; init; }

	/// <summary>The name of the site the device belongs to.</summary>
	public string? SiteName { get; init; }

	/// <summary>The cluster name, when the device is part of a cluster.</summary>
	public string? ClusterName { get; init; }

	/// <summary>The identifier of the cluster.</summary>
	public string? ClusterId { get; init; }

	/// <summary>The reason for the last reboot.</summary>
	public string? LastRebootReason { get; init; }

	/// <summary>The identifier of the device group.</summary>
	public string? DeviceGroupId { get; init; }

	/// <summary>The name of the device group.</summary>
	public string? DeviceGroupName { get; init; }

	/// <summary>Current CPU utilisation, as a percentage.</summary>
	public double? CpuUtilization { get; init; }

	/// <summary>Current memory utilisation, as a percentage.</summary>
	public double? MemoryUtilization { get; init; }

	/// <summary>Current power consumption, in watts.</summary>
	public double? PowerConsumption { get; init; }

	/// <summary>The number of connected clients.</summary>
	public int? ClientCount { get; init; }

	/// <summary>The identifier of the building the device is placed in.</summary>
	public string? BuildingId { get; init; }

	/// <summary>The identifier of the floor the device is placed on.</summary>
	public string? FloorId { get; init; }

	/// <summary>The resource identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }
}
