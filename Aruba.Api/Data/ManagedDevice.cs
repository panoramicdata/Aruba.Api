namespace Aruba.Api.Data;

/// <summary>
/// What the generic <see cref="Device"/>, an <see cref="AccessPoint"/> and a <see cref="Switch"/> all report
/// beyond <see cref="DeviceBase"/>: part number, deployment, addresses and when the device was last seen.
/// </summary>
public abstract class ManagedDevice : DeviceBase
{
	/// <summary>The hardware part number.</summary>
	public string? PartNumber { get; init; }

	/// <summary>The deployment mode.</summary>
	public string? Deployment { get; init; }

	/// <summary>The IPv4 address.</summary>
	public string? Ipv4 { get; init; }

	/// <summary>The IPv6 address.</summary>
	public string? Ipv6 { get; init; }

	/// <summary>When the device was last seen.</summary>
	public DateTimeOffset? LastSeenAt { get; init; }
}
