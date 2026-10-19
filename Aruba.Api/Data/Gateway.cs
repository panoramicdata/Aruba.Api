namespace Aruba.Api.Data;

/// <summary>
/// A gateway as reported by the New Central network-monitoring service.
/// </summary>
public sealed class Gateway : DeviceBase
{
	/// <summary>Current CPU utilisation, as a percentage.</summary>
	public double? CpuUtilization { get; init; }

	/// <summary>Current memory utilisation, as a percentage.</summary>
	public double? MemoryUtilization { get; init; }

	/// <summary>The IP address.</summary>
	public string? IpAddress { get; init; }

	/// <summary>The device uptime, in milliseconds.</summary>
	public long? UptimeInMillis { get; init; }

	/// <summary>The MAC address range owned by the gateway.</summary>
	public string? MacRange { get; init; }

	/// <summary>The reason for the last reboot.</summary>
	public string? RebootReason { get; init; }

	/// <summary>The operating mode, for example <c>Cluster</c>.</summary>
	public string? Mode { get; init; }

	/// <summary>The device role, for example <c>Isoleader</c>.</summary>
	public string? Role { get; init; }

	/// <summary>The cluster name, when the gateway is part of a cluster.</summary>
	public string? ClusterName { get; init; }
}
