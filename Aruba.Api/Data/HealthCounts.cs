namespace Aruba.Api.Data;

/// <summary>
/// A breakdown of a population into good/fair/poor health buckets.
/// </summary>
public sealed class HealthCounts
{
	/// <summary>The total number of items, when provided.</summary>
	public int? Total { get; init; }

	/// <summary>The number of items in good health.</summary>
	public int Good { get; init; }

	/// <summary>The number of items in fair health.</summary>
	public int Fair { get; init; }

	/// <summary>The number of items in poor health.</summary>
	public int Poor { get; init; }
}
