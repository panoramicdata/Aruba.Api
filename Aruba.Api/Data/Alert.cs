namespace Aruba.Api.Data;

/// <summary>
/// A notification alert as reported by the New Central network-notifications service.
/// </summary>
public sealed class Alert
{
	/// <summary>The resource identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }

	/// <summary>The alert name.</summary>
	public string? Name { get; init; }

	/// <summary>A human-readable summary of the alert.</summary>
	public string? Summary { get; init; }

	/// <summary>The status, for example <c>Active</c> or <c>Cleared</c>.</summary>
	public string? Status { get; init; }

	/// <summary>The category, for example <c>System</c>.</summary>
	public string? Category { get; init; }

	/// <summary>The alert definition key.</summary>
	public string? Key { get; init; }

	/// <summary>The severity, for example <c>Critical</c>, <c>Major</c> or <c>Minor</c>.</summary>
	public string? Severity { get; init; }

	/// <summary>The priority, for example <c>Very High</c>, <c>High</c> or <c>Medium</c>.</summary>
	public string? Priority { get; init; }

	/// <summary>The device type the alert relates to.</summary>
	public string? DeviceType { get; init; }

	/// <summary>The reason the alert was cleared, when applicable.</summary>
	public string? ClearedReason { get; init; }

	/// <summary>The user that last updated the alert, when applicable.</summary>
	public string? UpdatedBy { get; init; }

	/// <summary>The time the alert was created.</summary>
	public DateTimeOffset? CreatedAt { get; init; }

	/// <summary>The time the alert was last updated, when applicable.</summary>
	public DateTimeOffset? UpdatedAt { get; init; }
}
