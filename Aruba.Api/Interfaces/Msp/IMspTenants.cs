using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Msp;

/// <summary>
/// Managed Service Provider (MSP) endpoints (<c>/network-msp/v1</c>).
/// </summary>
public interface IMspTenants
{
	/// <summary>
	/// Gets information about all tenants managed by the MSP.
	/// </summary>
	[Get("/network-msp/v1/list-tenants")]
	Task<PagedResponse<MspTenant>> ListAsync(ListQuery? query = null, CancellationToken cancellationToken = default);
}
