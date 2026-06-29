namespace Aruba.Api.Data;

/// <summary>
/// The body for a bulk alert action (clear, defer, set-active or change-priority).
/// </summary>
public sealed class AlertActionRequest
{
	/// <summary>The identifiers of the alerts the action applies to.</summary>
	public IReadOnlyList<string> AlertIds { get; init; } = [];

	/// <summary>
	/// For a defer action, the number of minutes to defer the alerts for.
	/// </summary>
	public int? DeferMinutes { get; init; }

	/// <summary>
	/// For a priority action, the priority to set, for example <c>Very High</c>.
	/// </summary>
	public string? Priority { get; init; }

	/// <summary>An optional reason or note for the action.</summary>
	public string? Reason { get; init; }
}
