namespace Aruba.Api.Data;

/// <summary>
/// A wired or wireless client as reported by the New Central network-monitoring service.
/// </summary>
public sealed class Client
{
	/// <summary>The client MAC address.</summary>
	public string? MacAddress { get; init; }

	/// <summary>The resource identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }

	/// <summary>The friendly client name.</summary>
	public string? ClientName { get; init; }

	/// <summary>The connection status, for example <c>Connected</c>.</summary>
	public string? Status { get; init; }

	/// <summary>The type of device the client is connected to, for example <c>AP</c>.</summary>
	public string? ConnectedDeviceType { get; init; }

	/// <summary>The connection type, for example <c>Wireless</c> or <c>Wired</c>.</summary>
	public string? ClientConnectionType { get; init; }

	/// <summary>The IPv4 address.</summary>
	public string? Ipv4 { get; init; }

	/// <summary>The IPv6 address.</summary>
	public string? Ipv6 { get; init; }

	/// <summary>The serial number of the device the client is connected to.</summary>
	public string? ConnectedDeviceSerial { get; init; }

	/// <summary>The MAC address of the device/radio the client is connected to.</summary>
	public string? ConnectedTo { get; init; }

	/// <summary>The time the client was last seen, when available.</summary>
	public DateTimeOffset? LastSeenAt { get; init; }

	/// <summary>The WLAN (SSID) name, for wireless clients.</summary>
	public string? WlanName { get; init; }

	/// <summary>The switch port, for wired clients.</summary>
	public string? Port { get; init; }

	/// <summary>The client role.</summary>
	public string? Role { get; init; }

	/// <summary>The VLAN identifier.</summary>
	public string? VlanId { get; init; }

	/// <summary>The VLAN name.</summary>
	public string? VlanName { get; init; }

	/// <summary>The time the client connected, when available.</summary>
	public DateTimeOffset? ConnectedAt { get; init; }

	/// <summary>The authenticated user name, when available.</summary>
	public string? UserName { get; init; }

	/// <summary>The client host name, when available.</summary>
	public string? HostName { get; init; }

	/// <summary>The wireless security mode, for example <c>WPA2-Enterprise</c>.</summary>
	public string? WirelessSecurity { get; init; }

	/// <summary>The client device manufacturer.</summary>
	public string? ClientManufacturer { get; init; }

	/// <summary>The client device function/category, for example <c>Mobile</c>.</summary>
	public string? ClientFunction { get; init; }

	/// <summary>The client operating system.</summary>
	public string? ClientOperatingSystem { get; init; }

	/// <summary>The identifier of the site the client is connected within.</summary>
	public string? SiteId { get; init; }

	/// <summary>The name of the site the client is connected within.</summary>
	public string? SiteName { get; init; }

	/// <summary>The wireless band, for example <c>5GHZ</c>.</summary>
	public string? WirelessBand { get; init; }

	/// <summary>The wireless channel.</summary>
	public string? WirelessChannel { get; init; }

	/// <summary>The BSSID the client is associated with.</summary>
	public string? Bssid { get; init; }

	/// <summary>The radio MAC address.</summary>
	public string? RadioMacAddress { get; init; }

	/// <summary>The authentication type, for example <c>Captive Portal</c>.</summary>
	public string? AuthenticationType { get; init; }
}
