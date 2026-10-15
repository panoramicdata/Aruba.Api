using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Troubleshooting;

/// <summary>
/// Live troubleshooting commands for AOS-CX switches (<c>/network-troubleshooting/v1/cx</c>).
/// </summary>
/// <remarks>
/// Commands are asynchronous; poll <see cref="GetCommandResultAsync"/> with the originating
/// command name and task id to retrieve the result.
/// </remarks>
public interface ICxSwitchTroubleshooting
{
	/// <summary>
	/// Runs a troubleshooting command on the switch. <paramref name="command"/> names the command
	/// to run — <c>ping</c>, <c>traceroute</c>, <c>poeBounce</c>, <c>portBounce</c> or
	/// <c>cableTest</c> — and <paramref name="request"/> carries that command's parameters.
	/// </summary>
	[Post("/network-troubleshooting/v1/cx/{serialNumber}/{command}")]
	Task<AsyncOperationResponse> CommandAsync(
		string serialNumber,
		[Body] TroubleshootingCommandRequest request,
		string command,
		CancellationToken cancellationToken = default);

	/// <summary>Runs one or more show commands on the switch.</summary>
	[Post("/network-troubleshooting/v1/cx/{serialNumber}/showCommands")]
	Task<AsyncOperationResponse> ShowCommandsAsync(string serialNumber, [Body] ShowCommandsRequest request, CancellationToken cancellationToken = default);

	/// <summary>Gets the list of show commands supported by the switch.</summary>
	[Get("/network-troubleshooting/v1/cx/{serialNumber}/show-commands")]
	Task<PagedResponse<string>> GetSupportedShowCommandsAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Reboots the switch.</summary>
	[Post("/network-troubleshooting/v1/cx/{serialNumber}/reboot")]
	Task<AsyncOperationResponse> RebootAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Flashes the switch's locator LED.</summary>
	[Post("/network-troubleshooting/v1/cx/{serialNumber}/locate")]
	Task<AsyncOperationResponse> LocateAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Lists the recent troubleshooting tasks for the switch.</summary>
	[Get("/network-troubleshooting/v1/cx/{serialNumber}/list-tasks")]
	Task<PagedResponse<TroubleshootingTaskSummary>> ListTasksAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the result of a previously started command (for example <c>ping</c> or <c>cableTest</c>).</summary>
	[Get("/network-troubleshooting/v1/cx/{serialNumber}/{command}/async-operations/{taskId}")]
	Task<TroubleshootingTask> GetCommandResultAsync(string serialNumber, string command, string taskId, CancellationToken cancellationToken = default);
}
