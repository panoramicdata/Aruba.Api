using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Monitoring;

/// <summary>
/// Client onboarding-experience endpoints (<c>/network-monitoring/v1/client-onboarding-*</c>).
/// </summary>
public interface IClientOnboarding
{
	/// <summary>Gets the overall client onboarding score over a time range.</summary>
	[Get("/network-monitoring/v1/client-onboarding-score")]
	Task<Dictionary<string, object>> GetScoreAsync(TimeRangeQuery query, CancellationToken cancellationToken = default);

	/// <summary>Exports the client onboarding stage data over a time range.</summary>
	[Get("/network-monitoring/v1/client-onboarding-stage/export")]
	Task<Dictionary<string, object>> ExportStagesAsync(TimeRangeQuery query, CancellationToken cancellationToken = default);

	/// <summary>Gets the client onboarding failure reasons over a time range.</summary>
	[Get("/network-monitoring/v1/client-onboarding-stage/reasons")]
	Task<Dictionary<string, object>> GetStageReasonsAsync(TimeRangeQuery query, CancellationToken cancellationToken = default);

	/// <summary>Gets the count of clients per onboarding stage over a time range.</summary>
	[Get("/network-monitoring/v1/client-onboarding-stage/count")]
	Task<Dictionary<string, object>> GetStageCountAsync(TimeRangeQuery query, CancellationToken cancellationToken = default);
}
