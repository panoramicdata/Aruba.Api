using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Services;

/// <summary>
/// Firmware information endpoints (<c>/network-services/v1/firmware-details</c>).
/// </summary>
public interface IFirmware
{
	/// <summary>Gets firmware details for devices.</summary>
	[Get("/network-services/v1/firmware-details")]
	Task<PagedResponse<FirmwareDetails>> GetDetailsAsync(ListQuery? query = null, CancellationToken cancellationToken = default);
}
