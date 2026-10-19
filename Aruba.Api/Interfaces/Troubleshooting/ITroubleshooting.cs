using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Troubleshooting;

/// <summary>
/// Live troubleshooting for any kind of device (<c>/network-troubleshooting/v1/{target}</c>).
/// </summary>
/// <remarks>
/// Access points, AOS-CX switches, AOS-S switches and gateways expose the same operations under a different route
/// segment, so there is one interface and the <see cref="TroubleshootingTarget"/> chooses the segment.
/// Commands are asynchronous: start one with <see cref="CommandAsync"/>, then poll <see cref="GetCommandResultAsync"/>
/// with the same command name and the returned task id. <see cref="TroubleshootingCommands"/> names the commands.
/// </remarks>
public interface ITroubleshooting
{
	/// <summary>
	/// Starts a troubleshooting command. <paramref name="command"/> names it (see <see cref="TroubleshootingCommands"/>)
	/// and <paramref name="request"/> carries its parameters.
	/// </summary>
	[Post("/network-troubleshooting/v1/{target}/{serialNumber}/{command}")]
	Task<AsyncOperationResponse> CommandAsync(
		TroubleshootingTarget target,
		string serialNumber,
		string command,
		[Body] TroubleshootingCommandRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>Runs one or more show commands on the device.</summary>
	[Post("/network-troubleshooting/v1/{target}/{serialNumber}/showCommands")]
	Task<AsyncOperationResponse> ShowCommandsAsync(
		TroubleshootingTarget target,
		string serialNumber,
		[Body] ShowCommandsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>Gets the list of show commands the device supports.</summary>
	[Get("/network-troubleshooting/v1/{target}/{serialNumber}/show-commands")]
	Task<PagedResponse<string>> GetSupportedShowCommandsAsync(
		TroubleshootingTarget target,
		string serialNumber,
		CancellationToken cancellationToken = default);

	/// <summary>Reboots the device.</summary>
	[Post("/network-troubleshooting/v1/{target}/{serialNumber}/reboot")]
	Task<AsyncOperationResponse> RebootAsync(
		TroubleshootingTarget target,
		string serialNumber,
		CancellationToken cancellationToken = default);

	/// <summary>Flashes the device's locator LED. Not available for gateways.</summary>
	[Post("/network-troubleshooting/v1/{target}/{serialNumber}/locate")]
	Task<AsyncOperationResponse> LocateAsync(
		TroubleshootingTarget target,
		string serialNumber,
		CancellationToken cancellationToken = default);

	/// <summary>Lists the recent troubleshooting tasks for the device.</summary>
	[Get("/network-troubleshooting/v1/{target}/{serialNumber}/list-tasks")]
	Task<PagedResponse<TroubleshootingTaskSummary>> ListTasksAsync(
		TroubleshootingTarget target,
		string serialNumber,
		CancellationToken cancellationToken = default);

	/// <summary>Gets the result of a command started earlier, using the same command name and the task id it returned.</summary>
	[Get("/network-troubleshooting/v1/{target}/{serialNumber}/{command}/async-operations/{taskId}")]
	Task<TroubleshootingTask> GetCommandResultAsync(
		TroubleshootingTarget target,
		string serialNumber,
		string command,
		string taskId,
		CancellationToken cancellationToken = default);
}
