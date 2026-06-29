namespace Aruba.Api.Data;

/// <summary>
/// A report definition as reported by the New Central reporting service.
/// </summary>
public sealed class Report
{
	/// <summary>The report identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }

	/// <summary>The report name.</summary>
	public string? Name { get; init; }

	/// <summary>The scope the report applies to.</summary>
	public ReportScope? ReportScope { get; init; }

	/// <summary>The report type.</summary>
	public string? ReportType { get; init; }

	/// <summary>The user that created the report.</summary>
	public string? CreatedBy { get; init; }

	/// <summary>The time the report was created.</summary>
	public DateTimeOffset? CreatedAt { get; init; }

	/// <summary>The user that last modified the report.</summary>
	public string? LastModifiedBy { get; init; }

	/// <summary>The time the report was last modified.</summary>
	public DateTimeOffset? LastModifiedAt { get; init; }

	/// <summary>The report schedule.</summary>
	public ReportSchedule? ReportSchedule { get; init; }
}

/// <summary>
/// The scope a <see cref="Report"/> applies to.
/// </summary>
public sealed class ReportScope
{
	/// <summary>The scope type.</summary>
	public string? ScopeType { get; init; }
}

/// <summary>
/// The schedule of a <see cref="Report"/>.
/// </summary>
public sealed class ReportSchedule
{
	/// <summary>The schedule type, for example <c>EVERY_YEAR</c> or <c>ONE_TIME</c>.</summary>
	public string? ScheduleType { get; init; }

	/// <summary>The time the report last ran.</summary>
	public DateTimeOffset? LastRunTs { get; init; }

	/// <summary>The status of the last run, for example <c>ACTIVE</c> or <c>PAUSED</c>.</summary>
	public string? LastRunStatus { get; init; }

	/// <summary>The time the report will next run.</summary>
	public DateTimeOffset? NextRunTs { get; init; }

	/// <summary>The number of completed runs.</summary>
	public int? CompletedRuns { get; init; }

	/// <summary>The total number of scheduled runs.</summary>
	public int? TotalRuns { get; init; }

	/// <summary>The time the schedule ends.</summary>
	public DateTimeOffset? ScheduleEnd { get; init; }
}

/// <summary>
/// A single execution of a <see cref="Report"/>.
/// </summary>
public sealed class ReportRun
{
	/// <summary>The run identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The run status.</summary>
	public string? Status { get; init; }

	/// <summary>The time the run started.</summary>
	public DateTimeOffset? StartedAt { get; init; }

	/// <summary>The time the run completed.</summary>
	public DateTimeOffset? CompletedAt { get; init; }
}
