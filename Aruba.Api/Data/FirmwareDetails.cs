namespace Aruba.Api.Data;

/// <summary>
/// Firmware details for a device, as reported by the New Central services API.
/// </summary>
public sealed class FirmwareDetails
{
	/// <summary>The device serial number.</summary>
	public string? SerialNumber { get; init; }

	/// <summary>The device type.</summary>
	public string? DeviceType { get; init; }

	/// <summary>The currently running firmware version.</summary>
	public string? CurrentVersion { get; init; }

	/// <summary>The recommended firmware version, when available.</summary>
	public string? RecommendedVersion { get; init; }

	/// <summary>Whether an upgrade is available.</summary>
	public bool? UpgradeAvailable { get; init; }
}
