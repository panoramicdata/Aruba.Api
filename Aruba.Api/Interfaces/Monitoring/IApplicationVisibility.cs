using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Monitoring;

/// <summary>
/// Application visibility endpoints (<c>/network-monitoring/v1/applications</c>).
/// </summary>
public interface IApplicationVisibility
{
	/// <summary>
	/// Gets application usage for a site over a time range.
	/// </summary>
	[Get("/network-monitoring/v1/applications")]
	Task<PagedResponse<Dictionary<string, object>>> GetApplicationsAsync(TimeRangeQuery query, CancellationToken cancellationToken = default);
}
