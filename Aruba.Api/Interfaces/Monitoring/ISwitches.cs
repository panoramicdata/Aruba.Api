using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Monitoring;

/// <summary>
/// Switch monitoring endpoints (<c>/network-monitoring/v1/switches</c> and related).
/// </summary>
public interface ISwitches
{
	/// <summary>Gets the list of switches associated with the customer.</summary>
	[Get("/network-monitoring/v1/switches")]
	Task<PagedResponse<Switch>> GetAllAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets a single switch by serial number.</summary>
	[Get("/network-monitoring/v1/switches/{serialNumber}")]
	Task<Switch> GetAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the members of a switch stack.</summary>
	[Get("/network-monitoring/v1/stack/{serialNumber}/members")]
	Task<PagedResponse<Dictionary<string, object>>> GetStackMembersAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the hardware categories for a switch.</summary>
	[Get("/network-monitoring/v1/switches/{serialNumber}/hardware-categories")]
	Task<PagedResponse<Dictionary<string, object>>> GetHardwareCategoriesAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the link aggregation groups (LAGs) for a switch.</summary>
	[Get("/network-monitoring/v1/switches/{serialNumber}/lag")]
	Task<PagedResponse<Dictionary<string, object>>> GetLagAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the interfaces for a switch.</summary>
	[Get("/network-monitoring/v1/switches/{serialNumber}/interfaces")]
	Task<PagedResponse<Dictionary<string, object>>> GetInterfacesAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the VLANs for a switch.</summary>
	[Get("/network-monitoring/v1/switches/{serialNumber}/vlans")]
	Task<PagedResponse<Dictionary<string, object>>> GetVlansAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the PoE status of a switch's interfaces.</summary>
	[Get("/network-monitoring/v1/switches/{serialNumber}/interface-poe")]
	Task<PagedResponse<Dictionary<string, object>>> GetInterfacePoeAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the VSX state for a switch.</summary>
	[Get("/network-monitoring/v1/switches/{serialNumber}/vsx")]
	Task<Dictionary<string, object>> GetVsxAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets interface trends for a switch.</summary>
	[Get("/network-monitoring/v1/switches/{serialNumber}/interface-trends")]
	Task<TrendResponse> GetInterfaceTrendsAsync(string serialNumber, TimeRangeQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets hardware trends for a switch.</summary>
	[Get("/network-monitoring/v1/switches/{serialNumber}/hardware-trends")]
	Task<TrendResponse> GetHardwareTrendsAsync(string serialNumber, TimeRangeQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets the top interfaces across switches, by trend.</summary>
	[Get("/network-monitoring/v1/switches/topn-interface-trends")]
	Task<TrendResponse> GetTopInterfaceTrendsAsync(TimeRangeQuery? query = null, CancellationToken cancellationToken = default);
}
