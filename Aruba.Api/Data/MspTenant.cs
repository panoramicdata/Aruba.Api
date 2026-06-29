namespace Aruba.Api.Data;

/// <summary>
/// A managed tenant as reported by the New Central MSP service.
/// </summary>
public sealed class MspTenant
{
	/// <summary>The tenant name.</summary>
	public string? TenantName { get; init; }

	/// <summary>The tenant identifier.</summary>
	public string? TenantId { get; init; }

	/// <summary>The total number of sites in the tenant.</summary>
	public int? TotalSites { get; init; }

	/// <summary>The number of degraded sites in the tenant.</summary>
	public int? DegradedSites { get; init; }

	/// <summary>The device ownership model, for example <c>MSP</c>.</summary>
	public string? DeviceOwnership { get; init; }

	/// <summary>The device health breakdown for the tenant.</summary>
	public HealthCounts? DeviceHealthStatus { get; init; }

	/// <summary>The alert breakdown for the tenant.</summary>
	public MspAlertCounts? Alerts { get; init; }

	/// <summary>The time the tenant was last updated (Unix epoch seconds).</summary>
	public long? LastUpdatedTime { get; init; }

	/// <summary>The time the tenant was created (Unix epoch seconds).</summary>
	public long? CreatedTime { get; init; }
}

/// <summary>
/// An alert-severity breakdown for an MSP tenant.
/// </summary>
public sealed class MspAlertCounts
{
	/// <summary>The total number of alerts.</summary>
	public int Total { get; init; }

	/// <summary>The number of critical alerts.</summary>
	public int Critical { get; init; }

	/// <summary>The number of major alerts.</summary>
	public int Major { get; init; }

	/// <summary>The number of minor alerts.</summary>
	public int Minor { get; init; }
}
