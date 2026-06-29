namespace Aruba.Api.Data;

/// <summary>
/// An insight as reported by the New Central network-notifications service.
/// </summary>
public sealed class Insight
{
	/// <summary>The insight identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }

	/// <summary>The insight name.</summary>
	public string? Name { get; init; }

	/// <summary>The insight category.</summary>
	public string? Category { get; init; }

	/// <summary>The insight severity.</summary>
	public string? Severity { get; init; }

	/// <summary>A human-readable description of the insight.</summary>
	public string? Description { get; init; }

	/// <summary>The time the insight was generated.</summary>
	public DateTimeOffset? CreatedAt { get; init; }
}
