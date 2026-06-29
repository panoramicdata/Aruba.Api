using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Monitoring;

/// <summary>
/// Network topology endpoints (<c>/network-monitoring/v1/topology</c> and related).
/// </summary>
public interface ITopology
{
	/// <summary>Gets the network topology for a site.</summary>
	[Get("/network-monitoring/v1/topology/{siteId}")]
	Task<Dictionary<string, object>> GetSiteTopologyAsync(string siteId, CancellationToken cancellationToken = default);

	/// <summary>Gets details of an unmanaged device by MAC address within a site.</summary>
	[Get("/network-monitoring/v1/unmanaged-device/{macAddress}")]
	Task<Dictionary<string, object>> GetUnmanagedDeviceAsync(string macAddress, [AliasAs("site-id")] string siteId, CancellationToken cancellationToken = default);

	/// <summary>Gets the isolated (unconnected) devices within a site.</summary>
	[Get("/network-monitoring/v1/isolated-devices/{siteId}")]
	Task<PagedResponse<Dictionary<string, object>>> GetIsolatedDevicesAsync(string siteId, CancellationToken cancellationToken = default);

	/// <summary>Gets the neighbours of a device by serial number.</summary>
	[Get("/network-monitoring/v1/neighbours/{serialNumber}")]
	Task<PagedResponse<Dictionary<string, object>>> GetNeighboursAsync(string serialNumber, CancellationToken cancellationToken = default);
}
