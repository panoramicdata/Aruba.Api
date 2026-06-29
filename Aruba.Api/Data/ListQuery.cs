namespace Aruba.Api.Data;

/// <summary>
/// Common query parameters accepted by New Central list endpoints. All values are optional.
/// </summary>
/// <remarks>
/// Pass an instance to a list method to page and filter results. Property names are mapped to the
/// kebab-case query keys the API expects via <see cref="AliasAsAttribute"/>.
/// </remarks>
public class ListQuery
{
	/// <summary>The maximum number of items to return.</summary>
	[AliasAs("limit")]
	public int? Limit { get; init; }

	/// <summary>The zero-based offset of the first item to return.</summary>
	[AliasAs("offset")]
	public int? Offset { get; init; }

	/// <summary>An opaque cursor (the <c>next</c> value from a previous page) for cursor-based paging.</summary>
	[AliasAs("next")]
	public string? Next { get; init; }

	/// <summary>The sort expression, when supported by the endpoint.</summary>
	[AliasAs("sort")]
	public string? Sort { get; init; }

	/// <summary>A filter expression, when supported by the endpoint.</summary>
	[AliasAs("filter")]
	public string? Filter { get; init; }

	/// <summary>Restricts results to a single site.</summary>
	[AliasAs("site-id")]
	public string? SiteId { get; init; }
}

/// <summary>
/// A <see cref="ListQuery"/> extended with a time range. The <c>start-at</c>/<c>end-at</c> values
/// are passed verbatim; depending on the endpoint these are ISO-8601 instants
/// (for example <c>2024-11-20T14:14:33Z</c>) or Unix epoch milliseconds (for example
/// <c>1770072927960</c>) — consult the endpoint documentation.
/// </summary>
public sealed class TimeRangeQuery : ListQuery
{
	/// <summary>The start of the time range (ISO-8601 instant or epoch milliseconds).</summary>
	[AliasAs("start-at")]
	public string? StartAt { get; init; }

	/// <summary>The end of the time range (ISO-8601 instant or epoch milliseconds).</summary>
	[AliasAs("end-at")]
	public string? EndAt { get; init; }
}
