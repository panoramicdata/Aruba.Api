namespace Aruba.Api.Data;

/// <summary>
/// A switch as reported by the New Central network-monitoring service.
/// </summary>
public sealed class Switch
{
	/// <summary>The identifiers and labels of the device.</summary>
	public string? SerialNumber { get; init; }
	public string? MacAddress { get; init; }
	public string? DeviceName { get; init; }
	public string? Model { get; init; }
	public string? PartNumber { get; init; }

	/// <summary>The running firmware version.</summary>
	public string? FirmwareVersion { get; init; }

	/// <summary>The network addresses of the device.</summary>
	public string? Ipv4 { get; init; }
	public string? Ipv6 { get; init; }
	public string? PublicIp { get; init; }

	/// <summary>The connection status.</summary>
	public string? Status { get; init; }
	public string? SwitchRole { get; init; }
	public string? Deployment { get; init; }
	public string? DeviceFunction { get; init; }
	public string? SwitchType { get; init; }
	public long? UptimeInMillis { get; init; }
	public DateTimeOffset? LastSeenAt { get; init; }

	/// <summary>The site the device belongs to.</summary>
	public string? SiteId { get; init; }
	public string? SiteName { get; init; }

	/// <summary>The stack identifiers, when the switch is part of a stack.</summary>
	public string? StackId { get; init; }
	public int? StackMemberId { get; init; }

	/// <summary>Per-member resource and utilisation trends.</summary>
	public IReadOnlyList<SwitchTrend> SwitchTrends { get; init; } = [];

	/// <summary>The New Central identifiers of the resource.</summary>
	public string? Id { get; init; }
	public string? Type { get; init; }
}

/// <summary>
/// A point-in-time resource/utilisation snapshot for a switch (or stack member).
/// </summary>
public sealed class SwitchTrend
{
	/// <summary>The serial number of the switch.</summary>
	public string? Serial { get; init; }
	/// <summary>The role of the switch.</summary>
	public string? SwitchRole { get; init; }
	/// <summary>The uplink ports.</summary>
	public string? UpLinkPorts { get; init; }

	/// <summary>CPU utilization percentage.</summary>
	public double? CpuUtilization { get; init; }
	/// <summary>Memory utilization percentage.</summary>
	public double? MemoryUtilization { get; init; }
	/// <summary>PoE power available (watts).</summary>
	public double? PoeAvailable { get; init; }
	/// <summary>PoE power consumed (watts).</summary>
	public double? PoeConsumption { get; init; }
	/// <summary>Total power consumption (watts).</summary>
	public double? PowerConsumption { get; init; }
	/// <summary>System temperature (°C).</summary>
	public double? SystemTemperature { get; init; }
	/// <summary>Total power consumption including PoE (watts).</summary>
	public double? TotalPowerConsumption { get; init; }
	/// <summary>Generic usage metric.</summary>
	public double? Usage { get; init; }
}