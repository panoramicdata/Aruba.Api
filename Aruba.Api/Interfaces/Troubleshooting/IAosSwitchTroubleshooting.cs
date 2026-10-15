using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Troubleshooting;

/// <summary>
/// Live troubleshooting commands for AOS-S switches (<c>/network-troubleshooting/v1/aos-s</c>).
/// </summary>
/// <remarks>
/// Commands are asynchronous; poll <see cref="GetCommandResultAsync"/> with the originating
/// command name and task id to retrieve the result.
/// </remarks>
public interface IAosSwitchTroubleshooting
{
	/// <summary>
	/// Starts a troubleshooting command on the switch. The <paramref name="commandName"/>
	/// selects the endpoint — one of <c>ping</c>, <c>traceroute</c>, <c>poeBounce</c>,
	/// <c>portBounce</c> or <c>cableTest</c>. The payload must describe what to probe or bounce;
	/// follow up with <see cref="GetCommandResultAsync"/> using the same name to collect the result.
	/// </summary>
	[Post("/network-troubleshooting/v1/aos-s/{serialNumber}/{commandName}")]
	Task<AsyncOperationResponse> ExecuteCommandAsync(
		string serialNumber,
		string commandName,
		[Body] TroubleshootingCommandRequest command,
		CancellationToken cancellationToken = default);

	/// <summary>Runs one or more show commands on the switch.</summary>
	[Post("/network-troubleshooting/v1/aos-s/{serialNumber}/showCommands")]
	Task<AsyncOperationResponse> ShowCommandsAsync(
		string serialNumber,
		[Body] ShowCommandsRequest command,
		CancellationToken cancellationToken = default);

	/// <summary>Gets the list of show commands supported by the switch.</summary>
	[Get("/network-troubleshooting/v1/aos-s/{serialNumber}/show-commands")]
	Task<PagedResponse<string>> GetSupportedShowCommandsAsync(
		string serialNumber,
		CancellationToken cancellationToken = default);

	/// <summary>Reboots the switch.</summary>
	[Post("/network-troubleshooting/v1/aos-s/{serialNumber}/reboot")]
	Task<AsyncOperationResponse> RebootAsync(
		string serialNumber,
		CancellationToken cancellationToken = default);

	/// <summary>Flashes the switch's locator LED.</summary>
	[Post("/network-troubleshooting/v1/aos-s/{serialNumber}/locate")]
	Task<AsyncOperationResponse> LocateAsync(
		string serialNumber,
		CancellationToken cancellationToken = default);

	/// <summary>Lists the recent troubleshooting tasks for the switch.</summary>
	[Get("/network-troubleshooting/v1/aos-s/{serialNumber}/list-tasks")]
	Task<PagedResponse<TroubleshootingTaskSummary>> ListTasksAsync(
		string serialNumber,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves the outcome of a command that was previously started (for instance
	/// <c>ping</c>, <c>traceroute</c> or <c>cableTest</c>).
	/// </summary>
	[Get("/network-troubleshooting/v1/aos-s/{serialNumber}/{command}/async-operations/{taskId}")]
	Task<TroubleshootingTask> GetCommandResultAsync(
		string serialNumber,
		string command,
		string taskId,
		CancellationToken cancellationToken = default);
}
