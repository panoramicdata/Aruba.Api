namespace Aruba.Api.Data;

/// <summary>
/// What every kind of device (access point, switch, gateway, or the generic <see cref="Device"/>) reports about
/// itself under New Central network monitoring.
/// </summary>
public abstract class DeviceBase
{
	/// <summary>The device serial number.</summary>
	public string? SerialNumber { get; init; }

	/// <summary>The device MAC address.</summary>
	public string? MacAddress { get; init; }

	/// <summary>The friendly device name.</summary>
	public string? DeviceName { get; init; }

	/// <summary>The hardware model.</summary>
	public string? Model { get; init; }

	/// <summary>The running firmware version.</summary>
	public string? FirmwareVersion { get; init; }

	/// <summary>The connection status.</summary>
	public string? Status { get; init; }

	/// <summary>The device function, for example <c>VPNC</c> for a gateway.</summary>
	public string? DeviceFunction { get; init; }

	/// <summary>The identifier of the site the device belongs to.</summary>
	public string? SiteId { get; init; }

	/// <summary>The name of the site the device belongs to.</summary>
	public string? SiteName { get; init; }

	/// <summary>The New Central resource identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }
}
