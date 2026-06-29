namespace Aruba.Api.Data;

/// <summary>
/// A managed network device (access point, switch or gateway) as reported by the
/// New Central network-monitoring service.
/// </summary>
public sealed class Device
{
	/// <summary>The device serial number.</summary>
	public string? SerialNumber { get; init; }

	/// <summary>The device MAC address.</summary>
	public string? MacAddress { get; init; }

	/// <summary>The friendly device name.</summary>
	public string? DeviceName { get; init; }

	/// <summary>The device type, for example <c>ACCESS_POINT</c>, <c>SWITCH</c> or <c>GATEWAY</c>.</summary>
	public string? DeviceType { get; init; }

	/// <summary>The hardware model.</summary>
	public string? Model { get; init; }

	/// <summary>The manufacturer part number.</summary>
	public string? PartNumber { get; init; }

	/// <summary>The deployment type.</summary>
	public string? Deployment { get; init; }

	/// <summary>The device role.</summary>
	public string? Role { get; init; }

	/// <summary>The device function.</summary>
	public string? DeviceFunction { get; init; }

	/// <summary>The running firmware version.</summary>
	public string? FirmwareVersion { get; init; }

	/// <summary>The IPv4 address.</summary>
	public string? Ipv4 { get; init; }

	/// <summary>The IPv6 address.</summary>
	public string? Ipv6 { get; init; }

	/// <summary>The connection status.</summary>
	public string? Status { get; init; }

	/// <summary>The device uptime, in milliseconds.</summary>
	public long? UptimeInMillis { get; init; }

	/// <summary>The time the device was last seen, when available.</summary>
	public DateTimeOffset? LastSeenAt { get; init; }

	/// <summary>The identifier of the site the device belongs to.</summary>
	public string? SiteId { get; init; }

	/// <summary>The name of the site the device belongs to.</summary>
	public string? SiteName { get; init; }

	/// <summary>The identifier of the building.</summary>
	public string? BuildingId { get; init; }

	/// <summary>The identifier of the floor.</summary>
	public string? FloorId { get; init; }

	/// <summary>The cluster name, when applicable.</summary>
	public string? ClusterName { get; init; }

	/// <summary>The configuration status, when available.</summary>
	public string? ConfigStatus { get; init; }

	/// <summary>The time the configuration was last modified, when available.</summary>
	public DateTimeOffset? ConfigLastModifiedAt { get; init; }

	/// <summary>Free-form operator notes associated with the device.</summary>
	public string? Notes { get; init; }

	/// <summary>The resource identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }
}
