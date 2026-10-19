namespace Aruba.Api.Data;

/// <summary>
/// An access point as reported by the New Central network-monitoring service. The cluster, reboot,
/// utilisation and location fields are access-point specific.
/// </summary>
public sealed class AccessPoint : ManagedDevice
{
	/// <summary>The public IPv4 address.</summary>
	public string? PublicIpv4 { get; init; }

	/// <summary>The device role.</summary>
	public string? Role { get; init; }

	/// <summary>The Instant cluster the device is part of, when applicable.</summary>
	public string? ClusterName { get; init; }

	/// <summary>The identifier of the Instant cluster.</summary>
	public string? ClusterId { get; init; }

	/// <summary>The reason for the most recent reboot.</summary>
	public string? LastRebootReason { get; init; }

	/// <summary>The identifier of the device group.</summary>
	public string? DeviceGroupId { get; init; }

	/// <summary>The name of the device group.</summary>
	public string? DeviceGroupName { get; init; }

	/// <summary>Current CPU utilisation, as a percentage.</summary>
	public double? CpuUtilization { get; init; }

	/// <summary>Current memory utilisation, as a percentage.</summary>
	public double? MemoryUtilization { get; init; }

	/// <summary>Current power consumption.</summary>
	public double? PowerConsumption { get; init; }

	/// <summary>The number of connected clients.</summary>
	public int? ClientCount { get; init; }

	/// <summary>The building the device is in.</summary>
	public string? BuildingId { get; init; }

	/// <summary>The floor the device is on.</summary>
	public string? FloorId { get; init; }
}
