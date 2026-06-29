namespace Aruba.Api.Data;

/// <summary>
/// A site's health overview as reported by the New Central network-monitoring service.
/// </summary>
public sealed class SiteHealth
{
	/// <summary>The site identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }

	/// <summary>The site name.</summary>
	public string? SiteName { get; init; }

	/// <summary>The site postal address.</summary>
	public SiteAddress? Address { get; init; }

	/// <summary>The alert summary for the site.</summary>
	public SiteAlertSummary? Alerts { get; init; }

	/// <summary>The reasons contributing to the site's health.</summary>
	public IReadOnlyList<SiteHealthReason> Reasons { get; init; } = [];

	/// <summary>The overall health distribution for the site.</summary>
	public NamedValueGroups? Health { get; init; }

	/// <summary>The device count and device health distribution for the site.</summary>
	public SiteDeviceHealth? Devices { get; init; }
}

/// <summary>A postal address.</summary>
public sealed class SiteAddress
{
	/// <summary>The country.</summary>
	public string? Country { get; init; }

	/// <summary>The street address.</summary>
	public string? Address { get; init; }

	/// <summary>The city.</summary>
	public string? City { get; init; }

	/// <summary>The postal/ZIP code.</summary>
	public string? ZipCode { get; init; }

	/// <summary>The state or region.</summary>
	public string? State { get; init; }
}

/// <summary>An alert summary with a total and per-severity groups.</summary>
public sealed class SiteAlertSummary
{
	/// <summary>The total number of alerts.</summary>
	public int TotalCount { get; init; }

	/// <summary>The per-severity groups.</summary>
	public IReadOnlyList<NamedCount> Groups { get; init; } = [];
}

/// <summary>A reason contributing to a site's health.</summary>
public sealed class SiteHealthReason
{
	/// <summary>The health level, for example <c>Poor</c>.</summary>
	public string? Health { get; init; }

	/// <summary>The reason code, for example <c>CLIENT_HEALTH_POOR</c>.</summary>
	public string? Reason { get; init; }

	/// <summary>Supporting data for the reason.</summary>
	public NamedCount? Data { get; init; }
}

/// <summary>A device count together with a device health distribution.</summary>
public sealed class SiteDeviceHealth
{
	/// <summary>The number of devices.</summary>
	public int Count { get; init; }

	/// <summary>The device health distribution.</summary>
	public NamedValueGroups? Health { get; init; }
}

/// <summary>A set of named numeric value groups (for example a health distribution).</summary>
public sealed class NamedValueGroups
{
	/// <summary>The groups.</summary>
	public IReadOnlyList<NamedValue> Groups { get; init; } = [];
}

/// <summary>A name paired with a numeric value.</summary>
public sealed class NamedValue
{
	/// <summary>The group name.</summary>
	public string? Name { get; init; }

	/// <summary>The value.</summary>
	public double Value { get; init; }
}

/// <summary>A name paired with a count.</summary>
public sealed class NamedCount
{
	/// <summary>The group name.</summary>
	public string? Name { get; init; }

	/// <summary>The count.</summary>
	public int Count { get; init; }
}
