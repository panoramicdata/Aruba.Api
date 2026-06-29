using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Reporting;

/// <summary>
/// Reporting endpoints (<c>/network-reporting/v1/reports</c>).
/// </summary>
public interface IReports
{
	/// <summary>Gets the list of report definitions.</summary>
	[Get("/network-reporting/v1/reports")]
	Task<PagedResponse<Report>> GetAllAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Updates a report definition.</summary>
	[Put("/network-reporting/v1/reports/{reportId}")]
	Task<Report> UpdateAsync(string reportId, [Body] Report report, CancellationToken cancellationToken = default);

	/// <summary>Gets the run history for a report.</summary>
	[Get("/network-reporting/v1/reports/{reportId}/report-runs")]
	Task<PagedResponse<ReportRun>> GetRunsAsync(string reportId, ListQuery? query = null, CancellationToken cancellationToken = default);
}
