namespace Aruba.Api.Data;

/// <summary>
/// A gateway as reported by the New Central network-monitoring service.
/// </summary>
public sealed class Gateway
{
	/// <summary>The device serial number.</summary>
	public string? SerialNumber { get; init; }

	/// <summary>The device MAC address.</summary>
	public string? MacAddress { get; init; }

	/// <summary>The friendly device name.</summary>
	public string? DeviceName { get; init; }

	/// <summary>Current CPU utilisation, as a percentage.</summary>
	public double? CpuUtilization { get; init; }

	/// <summary>Current memory utilisation, as a percentage.</summary>
	public double? MemoryUtilization { get; init; }

	/// <summary>The identifier of the site the device belongs to.</summary>
	public string? SiteId { get; init; }

	/// <summary>The name of the site the device belongs to.</summary>
	public string? SiteName { get; init; }

	/// <summary>The running firmware version.</summary>
	public string? FirmwareVersion { get; init; }

	/// <summary>The IP address.</summary>
	public string? IpAddress { get; init; }

	/// <summary>The device uptime, in milliseconds.</summary>
	public long? UptimeInMillis { get; init; }

	/// <summary>The MAC address range owned by the gateway.</summary>
	public string? MacRange { get; init; }

	/// <summary>The reason for the last reboot.</summary>
	public string? RebootReason { get; init; }

	/// <summary>The device function, for example <c>VPNC</c>.</summary>
	public string? DeviceFunction { get; init; }

	/// <summary>The operating mode, for example <c>Cluster</c>.</summary>
	public string? Mode { get; init; }

	/// <summary>The connection status.</summary>
	public string? Status { get; init; }

	/// <summary>The device role, for example <c>Isoleader</c>.</summary>
	public string? Role { get; init; }

	/// <summary>The hardware model.</summary>
	public string? Model { get; init; }

	/// <summary>The cluster name, when the gateway is part of a cluster.</summary>
	public string? ClusterName { get; init; }

	/// <summary>The resource identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }
}
