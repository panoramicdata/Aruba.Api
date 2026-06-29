using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Monitoring;

/// <summary>
/// Site- and tenant-level health endpoints (<c>/network-monitoring/v1/sites-health</c> and related).
/// </summary>
public interface ISiteHealth
{
	/// <summary>Gets the health overview for all sites.</summary>
	[Get("/network-monitoring/v1/sites-health")]
	Task<PagedResponse<SiteHealth>> GetAllSitesHealthAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets the health overview for a single site.</summary>
	[Get("/network-monitoring/v1/site-health/{siteId}")]
	Task<SiteHealth> GetSiteHealthAsync(string siteId, CancellationToken cancellationToken = default);

	/// <summary>Gets per-site device health.</summary>
	[Get("/network-monitoring/v1/sites-device-health")]
	Task<PagedResponse<Dictionary<string, object>>> GetSitesDeviceHealthAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets tenant-wide device health.</summary>
	[Get("/network-monitoring/v1/tenant-device-health")]
	Task<Dictionary<string, object>> GetTenantDeviceHealthAsync(CancellationToken cancellationToken = default);

	/// <summary>Gets per-site client health.</summary>
	[Get("/network-monitoring/v1/sites-client-health")]
	Task<PagedResponse<Dictionary<string, object>>> GetSitesClientHealthAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets tenant-wide client health.</summary>
	[Get("/network-monitoring/v1/tenant-client-health")]
	Task<Dictionary<string, object>> GetTenantClientHealthAsync(CancellationToken cancellationToken = default);
}
