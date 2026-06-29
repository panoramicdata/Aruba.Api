using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Notifications;

/// <summary>
/// Insight endpoints (<c>/network-notifications/v1/insights</c>).
/// </summary>
public interface IInsights
{
	/// <summary>Gets the list of insights.</summary>
	[Get("/network-notifications/v1/insights")]
	Task<PagedResponse<Insight>> GetAllAsync(ListQuery? query = null, CancellationToken cancellationToken = default);
}
