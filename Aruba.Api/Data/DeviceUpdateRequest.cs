namespace Aruba.Api.Data;

/// <summary>
/// The body for a partial update (<c>PATCH</c>) of a monitored device.
/// </summary>
public sealed class DeviceUpdateRequest
{
	/// <summary>The friendly device name to set.</summary>
	public string? DeviceName { get; init; }

	/// <summary>Free-form operator notes to set.</summary>
	public string? Notes { get; init; }
}
