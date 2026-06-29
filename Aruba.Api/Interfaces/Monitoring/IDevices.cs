using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Monitoring;

/// <summary>
/// Device monitoring endpoints (<c>/network-monitoring/v1/devices</c> and related).
/// </summary>
public interface IDevices
{
	/// <summary>
	/// Gets the list of devices (access points, switches and gateways) associated with the customer.
	/// </summary>
	[Get("/network-monitoring/v1/devices")]
	Task<PagedResponse<Device>> GetAllAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Applies a partial update to a device, identified by its serial number.
	/// </summary>
	[Patch("/network-monitoring/v1/devices/{serialNumber}")]
	Task<Device> UpdateAsync(string serialNumber, [Body] DeviceUpdateRequest request, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes (deregisters) a device, identified by its serial number.
	/// </summary>
	[Delete("/network-monitoring/v1/devices/{serialNumber}")]
	Task DeleteAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets the device inventory associated with the customer.
	/// </summary>
	[Get("/network-monitoring/v1/device-inventory")]
	Task<PagedResponse<Device>> GetInventoryAsync(ListQuery? query = null, CancellationToken cancellationToken = default);
}
