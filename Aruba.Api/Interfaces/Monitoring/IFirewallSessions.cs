using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Monitoring;

/// <summary>
/// Firewall session monitoring endpoints (<c>/network-monitoring/v1/site-firewall-sessions</c> and related).
/// </summary>
public interface IFirewallSessions
{
	/// <summary>Gets firewall sessions for a site over a time range.</summary>
	[Get("/network-monitoring/v1/site-firewall-sessions")]
	Task<PagedResponse<Dictionary<string, object>>> GetSiteFirewallSessionsAsync(TimeRangeQuery query, CancellationToken cancellationToken = default);

	/// <summary>Gets firewall sessions for a specific client over a time range.</summary>
	[Get("/network-monitoring/v1/client-firewall-sessions")]
	Task<PagedResponse<Dictionary<string, object>>> GetClientFirewallSessionsAsync(
		TimeRangeQuery query,
		[AliasAs("client-mac")] string? clientMac = null,
		CancellationToken cancellationToken = default);

	/// <summary>Gets the clients with firewall sessions for a site over a time range.</summary>
	[Get("/network-monitoring/v1/firewall-clients")]
	Task<PagedResponse<Dictionary<string, object>>> GetFirewallClientsAsync(TimeRangeQuery query, CancellationToken cancellationToken = default);
}
