using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Troubleshooting;

/// <summary>
/// Network event endpoints (<c>/network-troubleshooting/v1/events</c>).
/// </summary>
public interface IEvents
{
	/// <summary>
	/// Gets network events for a context (for example a site) over a time range.
	/// </summary>
	[Get("/network-troubleshooting/v1/events")]
	Task<PagedResponse<NetworkEvent>> GetAllAsync(
		[AliasAs("context-type")] string contextType,
		[AliasAs("context-identifier")] string contextIdentifier,
		[AliasAs("start-at")] string startAt,
		[AliasAs("end-at")] string endAt,
		[AliasAs("site-id")] string? siteId = null,
		[AliasAs("limit")] int? limit = null,
		CancellationToken cancellationToken = default);

	/// <summary>Gets the additional attributes for a single event.</summary>
	[Get("/network-troubleshooting/v1/event-extra-attributes")]
	Task<Dictionary<string, object>> GetExtraAttributesAsync(
		[AliasAs("event-identifier")] string eventIdentifier,
		[AliasAs("site-id")] string siteId,
		[AliasAs("time-at")] string timeAt,
		CancellationToken cancellationToken = default);

	/// <summary>Gets the available event filters for a context over a time range.</summary>
	[Get("/network-troubleshooting/v1/event-filters")]
	Task<Dictionary<string, object>> GetFiltersAsync(
		[AliasAs("context-type")] string contextType,
		[AliasAs("context-identifier")] string contextIdentifier,
		[AliasAs("start-at")] string startAt,
		[AliasAs("end-at")] string endAt,
		[AliasAs("site-id")] string? siteId = null,
		CancellationToken cancellationToken = default);
}
