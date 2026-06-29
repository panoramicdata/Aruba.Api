namespace Aruba.Api.Data;

/// <summary>
/// The standard New Central collection envelope returned by list endpoints.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
public sealed class PagedResponse<T>
{
	/// <summary>The items in this page.</summary>
	public IReadOnlyList<T> Items { get; init; } = [];

	/// <summary>The number of items in this page.</summary>
	public int Count { get; init; }

	/// <summary>The total number of items available across all pages, when provided by the API.</summary>
	public int? Total { get; init; }

	/// <summary>
	/// An opaque cursor for the next page, or <see langword="null"/> when there are no further pages.
	/// Pass this value back as the <c>next</c> query parameter to retrieve the following page.
	/// </summary>
	public string? Next { get; init; }
}
