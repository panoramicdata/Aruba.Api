using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Monitoring;

/// <summary>
/// Gateway monitoring endpoints (<c>/network-monitoring/v1/gateways</c> and related clusters).
/// </summary>
public interface IGateways
{
	/// <summary>Gets the list of gateways associated with the customer.</summary>
	[Get("/network-monitoring/v1/gateways")]
	Task<PagedResponse<Gateway>> GetAllAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets a single gateway by serial number.</summary>
	[Get("/network-monitoring/v1/gateways/{serialNumber}")]
	Task<Gateway> GetAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the ports for a gateway.</summary>
	[Get("/network-monitoring/v1/gateways/{serialNumber}/ports")]
	Task<PagedResponse<Dictionary<string, object>>> GetPortsAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets a single port for a gateway.</summary>
	[Get("/network-monitoring/v1/gateways/{serialNumber}/ports/{portNumber}")]
	Task<Dictionary<string, object>> GetPortAsync(string serialNumber, string portNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the VLANs for a gateway.</summary>
	[Get("/network-monitoring/v1/gateways/{serialNumber}/vlans")]
	Task<PagedResponse<Dictionary<string, object>>> GetVlansAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets a single VLAN for a gateway.</summary>
	[Get("/network-monitoring/v1/gateways/{serialNumber}/vlans/{vlanId}")]
	Task<Dictionary<string, object>> GetVlanAsync(string serialNumber, string vlanId, CancellationToken cancellationToken = default);

	/// <summary>Gets the tunnels for a gateway.</summary>
	[Get("/network-monitoring/v1/gateways/{serialNumber}/tunnels")]
	Task<PagedResponse<Dictionary<string, object>>> GetTunnelsAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the uplinks for a gateway.</summary>
	[Get("/network-monitoring/v1/gateways/{serialNumber}/uplinks")]
	Task<PagedResponse<Dictionary<string, object>>> GetUplinksAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets CPU utilisation trends for a gateway.</summary>
	[Get("/network-monitoring/v1/gateways/{serialNumber}/cpu-utilization-trends")]
	Task<TrendResponse> GetCpuUtilizationTrendsAsync(string serialNumber, TimeRangeQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets memory utilisation trends for a gateway.</summary>
	[Get("/network-monitoring/v1/gateways/{serialNumber}/memory-utilization-trends")]
	Task<TrendResponse> GetMemoryUtilizationTrendsAsync(string serialNumber, TimeRangeQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets the DHCP pools configured on a gateway.</summary>
	[Get("/network-monitoring/v1/gateways/{serialNumber}/dhcp-pools")]
	Task<PagedResponse<Dictionary<string, object>>> GetDhcpPoolsAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the DHCP clients served by a gateway.</summary>
	[Get("/network-monitoring/v1/gateways/{serialNumber}/dhcp-clients")]
	Task<PagedResponse<Dictionary<string, object>>> GetDhcpClientsAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the members of a gateway cluster.</summary>
	[Get("/network-monitoring/v1/clusters/{clusterName}/members")]
	Task<PagedResponse<Dictionary<string, object>>> GetClusterMembersAsync(string clusterName, CancellationToken cancellationToken = default);

	/// <summary>Gets the tunnels for a gateway cluster.</summary>
	[Get("/network-monitoring/v1/clusters/{clusterName}/tunnels")]
	Task<PagedResponse<Dictionary<string, object>>> GetClusterTunnelsAsync(string clusterName, CancellationToken cancellationToken = default);

	/// <summary>Gets the tunnel-health summary for a gateway cluster.</summary>
	[Get("/network-monitoring/v1/clusters/{clusterName}/tunnels-health-summary")]
	Task<List<Dictionary<string, object>>> GetClusterTunnelHealthSummaryAsync(string clusterName, CancellationToken cancellationToken = default);
}
