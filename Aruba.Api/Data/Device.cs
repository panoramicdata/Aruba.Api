namespace Aruba.Api.Data;

/// <summary>
/// A managed network device (access point, switch or gateway) as reported by the
/// New Central network-monitoring service.
/// </summary>
public sealed class Device
{
	/// <summary>The identifiers and labels of the device.</summary>
	public string? SerialNumber { get; init; }
	public string? MacAddress { get; init; }
	public string? DeviceName { get; init; }
	public string? Model { get; init; }
	public string? PartNumber { get; init; }

	/// <summary>The device classification and state.</summary>
	public string? DeviceType { get; init; }
	public string? Deployment { get; init; }
	public string? Role { get; init; }
	public string? DeviceFunction { get; init; }
	public string? FirmwareVersion { get; init; }
	public string? Status { get; init; }

	/// <summary>The network addresses of the device.</summary>
	public string? Ipv4 { get; init; }
	public string? Ipv6 { get; init; }

	/// <summary>When the device was last seen and how long it has been up.</summary>
	public long? UptimeInMillis { get; init; }
	public DateTimeOffset? LastSeenAt { get; init; }

	/// <summary>The site and physical location of the device.</summary>
	public string? SiteId { get; init; }
	public string? SiteName { get; init; }
	public string? BuildingId { get; init; }
	public string? FloorId { get; init; }
	public string? ClusterName { get; init; }

	/// <summary>The configuration state of the device.</summary>
	public string? ConfigStatus { get; init; }
	public DateTimeOffset? ConfigLastModifiedAt { get; init; }

	/// <summary>Free-form operator notes associated with the device.</summary>
	public string? Notes { get; init; }

	/// <summary>The New Central identifiers of the resource.</summary>
	public string? Id { get; init; }
	public string? Type { get; init; }
}
