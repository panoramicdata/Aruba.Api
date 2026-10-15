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

	/// <summary>The switch role within a stack, for example <c>Conductor</c>.</summary>
	public string? SwitchRole { get; init; }

	/// <summary>The deployment type, for example <c>Stack</c>.</summary>
	public string? Deployment { get; init; }

	/// <summary>The device function.</summary>
	public string? DeviceFunction { get; init; }

	/// <summary>The switch type, for example <c>pvos</c> or <c>cx</c>.</summary>
	public string? SwitchType { get; init; }

	/// <summary>The device uptime, in milliseconds.</summary>
	public long? UptimeInMillis { get; init; }

	/// <summary>The time the device was last seen, when available.</summary>
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
	/// <summary>CPU utilisation, as a percentage.</summary>
	public double? CpuUtilization { get; init; }

	/// <summary>Memory utilisation, as a percentage.</summary>
	public double? MemoryUtilization { get; init; }

	/// <summary>Available PoE budget, in watts.</summary>
	public double? PoeAvailable { get; init; }

	/// <summary>PoE consumption, in watts.</summary>
	public double? PoeConsumption { get; init; }

	/// <summary>Power consumption, in watts.</summary>
	public double? PowerConsumption { get; init; }

	/// <summary>The serial number of the member this snapshot relates to.</summary>
	public string? Serial { get; init; }

	/// <summary>The switch role, when applicable.</summary>
	public string? SwitchRole { get; init; }

	/// <summary>System temperature, in degrees Celsius.</summary>
	public double? SystemTemperature { get; init; }

	/// <summary>Total power consumption, in watts.</summary>
	public double? TotalPowerConsumption { get; init; }

	/// <summary>The uplink ports, for example <c>1/8</c>.</summary>
	public string? UpLinkPorts { get; init; }

	/// <summary>Throughput usage, in bytes.</summary>
	public double? Usage { get; init; }
}