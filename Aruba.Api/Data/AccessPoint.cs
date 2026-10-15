namespace Aruba.Api.Data;

/// <summary>
/// An access point as reported by the New Central network-monitoring service.
/// </summary>
public sealed class AccessPoint
{
	/// <summary>The identifiers and labels of the device.</summary>
	public string? SerialNumber { get; init; }
	public string? MacAddress { get; init; }
	public string? DeviceName { get; init; }
	public string? Model { get; init; }
	public string? PartNumber { get; init; }

	/// <summary>The device classification and state.</summary>
	public string? Deployment { get; init; }
	public string? Role { get; init; }
	public string? DeviceFunction { get; init; }
	public string? FirmwareVersion { get; init; }
	public string? Status { get; init; }

	/// <summary>The network addresses of the device.</summary>
	public string? Ipv4 { get; init; }
	public string? Ipv6 { get; init; }
	public string? PublicIpv4 { get; init; }

	/// <summary>When the device was last seen.</summary>
	public DateTimeOffset? LastSeenAt { get; init; }

	/// <summary>The site the device belongs to.</summary>
	public string? SiteId { get; init; }
	public string? SiteName { get; init; }

	/// <summary>The Instant cluster the device is part of, when applicable.</summary>
	public string? ClusterName { get; init; }
	public string? ClusterId { get; init; }

	/// <summary>The most recent reboot and its device group.</summary>
	public string? LastRebootReason { get; init; }
	public string? DeviceGroupId { get; init; }
	public string? DeviceGroupName { get; init; }

	/// <summary>The resource utilisation of the device.</summary>
	public double? CpuUtilization { get; init; }
	public double? MemoryUtilization { get; init; }
	public double? PowerConsumption { get; init; }
	public int? ClientCount { get; init; }

	/// <summary>The physical location of the device.</summary>
	public string? BuildingId { get; init; }
	public string? FloorId { get; init; }

	/// <summary>The New Central identifiers of the resource.</summary>
	public string? Id { get; init; }
	public string? Type { get; init; }
}
