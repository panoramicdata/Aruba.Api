namespace Aruba.Api.Data;

/// <summary>
/// A time-series ("trend") response: a set of named series sampled at a fixed interval.
/// </summary>
public sealed class TrendResponse
{
	/// <summary>The sampling interval, for example <c>"5 mins"</c>.</summary>
	public string? Interval { get; init; }

	/// <summary>The names of the series, ordered to match each <see cref="TrendSample.Data"/> array.</summary>
	public IReadOnlyList<string> Keys { get; init; } = [];

	/// <summary>The samples, ordered by time.</summary>
	public IReadOnlyList<TrendSample> Samples { get; init; } = [];

	/// <summary>The identifier of the resource the trend relates to.</summary>
	public string? Id { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }
}

/// <summary>
/// A single sample within a <see cref="TrendResponse"/>.
/// </summary>
public sealed class TrendSample
{
	/// <summary>The values, one per entry in <see cref="TrendResponse.Keys"/>.</summary>
	public IReadOnlyList<double> Data { get; init; } = [];

	/// <summary>The timestamp of this sample.</summary>
	public DateTimeOffset Ts { get; init; }
}
