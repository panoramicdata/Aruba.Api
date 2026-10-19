namespace Aruba.Api.Data;

/// <summary>
/// A switch as reported by the New Central network-monitoring service.
/// </summary>
public sealed class Switch : ManagedDevice
{
	/// <summary>The public IP address.</summary>
	public string? PublicIp { get; init; }

	/// <summary>The switch role.</summary>
	public string? SwitchRole { get; init; }

	/// <summary>The switch type.</summary>
	public string? SwitchType { get; init; }

	/// <summary>How long the switch has been up, in milliseconds.</summary>
	public long? UptimeInMillis { get; init; }

	/// <summary>The stack identifier, when the switch is part of a stack.</summary>
	public string? StackId { get; init; }

	/// <summary>The member number within the stack.</summary>
	public int? StackMemberId { get; init; }

	/// <summary>Per-member resource and utilisation trends.</summary>
	public IReadOnlyList<SwitchTrend> SwitchTrends { get; init; } = [];
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