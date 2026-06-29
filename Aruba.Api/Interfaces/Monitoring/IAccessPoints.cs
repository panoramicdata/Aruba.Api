using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Monitoring;

/// <summary>
/// Access point monitoring endpoints (<c>/network-monitoring/v1/aps</c> and related).
/// </summary>
public interface IAccessPoints
{
	/// <summary>Gets the list of access points associated with the customer.</summary>
	[Get("/network-monitoring/v1/aps")]
	Task<PagedResponse<AccessPoint>> GetAllAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets a single access point by serial number.</summary>
	[Get("/network-monitoring/v1/aps/{serialNumber}")]
	Task<AccessPoint> GetAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the list of radios across all access points.</summary>
	[Get("/network-monitoring/v1/radios")]
	Task<PagedResponse<Dictionary<string, object>>> GetRadiosAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets the list of BSSIDs across all access points.</summary>
	[Get("/network-monitoring/v1/bssids")]
	Task<PagedResponse<Dictionary<string, object>>> GetBssidsAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets the list of WLANs.</summary>
	[Get("/network-monitoring/v1/wlans")]
	Task<PagedResponse<Dictionary<string, object>>> GetWlansAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets a single WLAN by name.</summary>
	[Get("/network-monitoring/v1/wlans/{wlanName}")]
	Task<Dictionary<string, object>> GetWlanAsync(string wlanName, CancellationToken cancellationToken = default);

	/// <summary>Gets the list of swarms (clusters of Instant access points).</summary>
	[Get("/network-monitoring/v1/swarms")]
	Task<PagedResponse<Dictionary<string, object>>> GetSwarmsAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets a single swarm by cluster identifier.</summary>
	[Get("/network-monitoring/v1/swarms/{clusterId}")]
	Task<Dictionary<string, object>> GetSwarmAsync(string clusterId, CancellationToken cancellationToken = default);

	/// <summary>Gets the top access points ranked by total (wired and wireless) usage.</summary>
	[Get("/network-monitoring/v1/top-aps-by-usage")]
	Task<PagedResponse<AccessPoint>> GetTopByUsageAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets the top access points ranked by wireless usage.</summary>
	[Get("/network-monitoring/v1/top-aps-by-wireless-usage")]
	Task<PagedResponse<AccessPoint>> GetTopByWirelessUsageAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets the top access points ranked by wired usage.</summary>
	[Get("/network-monitoring/v1/top-aps-by-wired-usage")]
	Task<PagedResponse<AccessPoint>> GetTopByWiredUsageAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets CPU utilisation trends for an access point.</summary>
	[Get("/network-monitoring/v1/aps/{serialNumber}/cpu-utilization-trends")]
	Task<TrendResponse> GetCpuUtilizationTrendsAsync(string serialNumber, TimeRangeQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets memory utilisation trends for an access point.</summary>
	[Get("/network-monitoring/v1/aps/{serialNumber}/memory-utilization-trends")]
	Task<TrendResponse> GetMemoryUtilizationTrendsAsync(string serialNumber, TimeRangeQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets power consumption trends for an access point.</summary>
	[Get("/network-monitoring/v1/aps/{serialNumber}/power-consumption-trends")]
	Task<TrendResponse> GetPowerConsumptionTrendsAsync(string serialNumber, TimeRangeQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets throughput trends for an access point.</summary>
	[Get("/network-monitoring/v1/aps/{serialNumber}/throughput-trends")]
	Task<TrendResponse> GetThroughputTrendsAsync(string serialNumber, [AliasAs("interface-type")] string? interfaceType = null, TimeRangeQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets the radios for an access point.</summary>
	[Get("/network-monitoring/v1/aps/{serialNumber}/radios")]
	Task<PagedResponse<Dictionary<string, object>>> GetApRadiosAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the ports for an access point.</summary>
	[Get("/network-monitoring/v1/aps/{serialNumber}/ports")]
	Task<PagedResponse<Dictionary<string, object>>> GetApPortsAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the tunnels for an access point.</summary>
	[Get("/network-monitoring/v1/aps/{serialNumber}/tunnels")]
	Task<PagedResponse<Dictionary<string, object>>> GetApTunnelsAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the WLANs broadcast by an access point.</summary>
	[Get("/network-monitoring/v1/aps/{serialNumber}/wlans")]
	Task<PagedResponse<Dictionary<string, object>>> GetApWlansAsync(string serialNumber, CancellationToken cancellationToken = default);
}
