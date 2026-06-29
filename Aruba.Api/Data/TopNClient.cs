namespace Aruba.Api.Data;

/// <summary>
/// A client ranked by usage, as returned by the top-N clients endpoint.
/// </summary>
public sealed class TopNClient
{
	/// <summary>The friendly client name.</summary>
	public string? ClientName { get; init; }

	/// <summary>The client MAC address.</summary>
	public string? MacAddress { get; init; }

	/// <summary>Throughput usage, in bytes.</summary>
	public long? Usage { get; init; }

	/// <summary>The connection type, for example <c>Wireless</c> or <c>Wired</c>.</summary>
	public string? ClientConnectionType { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }

	/// <summary>The resource identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The name of the site the client is connected within.</summary>
	public string? SiteName { get; init; }

	/// <summary>The identifier of the site the client is connected within.</summary>
	public string? SiteId { get; init; }
}
