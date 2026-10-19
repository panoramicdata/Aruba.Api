namespace Aruba.Api.Data;

/// <summary>
/// A managed network device (access point, switch or gateway) as reported by the
/// New Central network-monitoring service.
/// </summary>
public sealed class Device : ManagedDevice
{
	/// <summary>The device classification.</summary>
	public string? DeviceType { get; init; }

	/// <summary>The device role.</summary>
	public string? Role { get; init; }

	/// <summary>How long the device has been up, in milliseconds.</summary>
	public long? UptimeInMillis { get; init; }

	/// <summary>The building the device is in.</summary>
	public string? BuildingId { get; init; }

	/// <summary>The floor the device is on.</summary>
	public string? FloorId { get; init; }

	/// <summary>The cluster the device is part of, when applicable.</summary>
	public string? ClusterName { get; init; }

	/// <summary>The configuration state of the device.</summary>
	public string? ConfigStatus { get; init; }

	/// <summary>When the configuration was last modified.</summary>
	public DateTimeOffset? ConfigLastModifiedAt { get; init; }

	/// <summary>Free-form operator notes associated with the device.</summary>
	public string? Notes { get; init; }
}
