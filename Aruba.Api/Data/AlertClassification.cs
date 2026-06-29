namespace Aruba.Api.Data;

/// <summary>
/// The classification breakdown of alerts (by category or type).
/// </summary>
public sealed class AlertClassification
{
	/// <summary>The classification dimension, for example <c>category</c>.</summary>
	public string? Type { get; init; }

	/// <summary>The breakdown entries.</summary>
	public IReadOnlyList<AlertClassificationEntry> ClassificationData { get; init; } = [];
}

/// <summary>
/// A single entry within an <see cref="AlertClassification"/>.
/// </summary>
public sealed class AlertClassificationEntry
{
	/// <summary>The classification name.</summary>
	public string? Name { get; init; }

	/// <summary>The number of alerts in this classification.</summary>
	public int Count { get; init; }

	/// <summary>The classification type identifier.</summary>
	public string? TypeId { get; init; }
}
