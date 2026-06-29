using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Monitoring;

/// <summary>
/// Client monitoring endpoints (<c>/network-monitoring/v1/clients</c> and related).
/// </summary>
public interface IClients
{
	/// <summary>Gets the list of wired and wireless clients.</summary>
	[Get("/network-monitoring/v1/clients")]
	Task<PagedResponse<Client>> GetAllAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets a single client by MAC address.</summary>
	[Get("/network-monitoring/v1/clients/{macAddress}")]
	Task<Client> GetAsync(string macAddress, CancellationToken cancellationToken = default);

	/// <summary>Gets the wired/wireless client-count trend.</summary>
	[Get("/network-monitoring/v1/clients-trend")]
	Task<TrendResponse> GetTrendAsync(TimeRangeQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets the top clients ranked by usage.</summary>
	[Get("/network-monitoring/v1/clients-topn-usage")]
	Task<PagedResponse<TopNClient>> GetTopByUsageAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets the mobility trail (roaming history) for a client.</summary>
	[Get("/network-monitoring/v1/clients/{macAddress}/mobility-trail")]
	Task<PagedResponse<Dictionary<string, object>>> GetMobilityTrailAsync(string macAddress, TimeRangeQuery? query = null, CancellationToken cancellationToken = default);
}
