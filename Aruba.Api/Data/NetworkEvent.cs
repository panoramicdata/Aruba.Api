namespace Aruba.Api.Data;

/// <summary>
/// A network event as reported by the New Central troubleshooting service.
/// </summary>
public sealed class NetworkEvent
{
	/// <summary>The event identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }

	/// <summary>The event name.</summary>
	public string? Name { get; init; }

	/// <summary>The event category.</summary>
	public string? Category { get; init; }

	/// <summary>The event severity or level.</summary>
	public string? Level { get; init; }

	/// <summary>A human-readable description of the event.</summary>
	public string? Description { get; init; }

	/// <summary>The serial number of the device the event relates to, when applicable.</summary>
	public string? SerialNumber { get; init; }

	/// <summary>The identifier of the site the event relates to, when applicable.</summary>
	public string? SiteId { get; init; }

	/// <summary>The time the event occurred.</summary>
	public DateTimeOffset? OccurredAt { get; init; }
}
