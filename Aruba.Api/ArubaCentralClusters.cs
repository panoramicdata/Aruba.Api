namespace Aruba.Api;

/// <summary>
/// Well-known HPE Aruba Networking Central (New Central) regional API gateway base addresses.
/// </summary>
/// <remarks>
/// The correct base address for your account depends on the geographical cluster where it is
/// registered. Always confirm yours using the official
/// <see href="https://developer.arubanetworks.com/new-central/docs/getting-started-with-rest-apis#finding-your-base-url">
/// Finding your Base URL</see> guide — if your cluster is not listed here, pass an explicit
/// <see cref="Uri"/> to <see cref="ArubaCentralClientOptions.BaseAddress"/>.
/// </remarks>
public static class ArubaCentralClusters
{
	/// <summary>US-WEST (us2) regional API gateway.</summary>
	public static Uri UsWest { get; } = new("https://us2.api.central.arubanetworks.com");

	/// <summary>US-EAST (us1) regional API gateway.</summary>
	public static Uri UsEast { get; } = new("https://us1.api.central.arubanetworks.com");

	/// <summary>Europe (eu1) regional API gateway.</summary>
	public static Uri Europe { get; } = new("https://eu1.api.central.arubanetworks.com");

	/// <summary>Asia-Pacific (apac1) regional API gateway.</summary>
	public static Uri AsiaPacific { get; } = new("https://apac1.api.central.arubanetworks.com");

	/// <summary>Canada (ca1) regional API gateway.</summary>
	public static Uri Canada { get; } = new("https://ca1.api.central.arubanetworks.com");
}
