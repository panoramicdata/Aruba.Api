using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Troubleshooting;

/// <summary>
/// Live troubleshooting commands for gateways (<c>/network-troubleshooting/v1/gateways</c>).
/// </summary>
/// <remarks>
/// Commands are asynchronous; poll <see cref="GetCommandResultAsync"/> with the originating
/// command name and task id to retrieve the result.
/// </remarks>
public interface IGatewayTroubleshooting
{
	/// <summary>
	/// Starts a troubleshooting command on the gateway. <paramref name="command"/> names the
	/// command to run — <c>ping</c>, <c>pingSweep</c>, <c>traceroute</c>, <c>iperf</c> or
	/// <c>disconnectClientByMacAddress</c> — and <paramref name="request"/> carries its parameters.
	/// </summary>
	[Post("/network-troubleshooting/v1/gateways/{serialNumber}/{command}")]
	Task<AsyncOperationResponse> CommandAsync(
		string serialNumber,
		[Body] TroubleshootingCommandRequest request,
		string command,
		CancellationToken cancellationToken = default);

	/// <summary>Runs one or more show commands on the gateway.</summary>
	[Post("/network-troubleshooting/v1/gateways/{serialNumber}/showCommands")]
	Task<AsyncOperationResponse> ShowCommandsAsync(
		string serialNumber,
		[Body] ShowCommandsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>Gets the list of show commands supported by the gateway.</summary>
	[Get("/network-troubleshooting/v1/gateways/{serialNumber}/show-commands")]
	Task<PagedResponse<string>> GetSupportedShowCommandsAsync(
		string serialNumber,
		CancellationToken cancellationToken = default);

	/// <summary>Reboots the gateway.</summary>
	[Post("/network-troubleshooting/v1/gateways/{serialNumber}/reboot")]
	Task<AsyncOperationResponse> RebootAsync(
		string serialNumber,
		CancellationToken cancellationToken = default);

	/// <summary>Lists the recent troubleshooting tasks for the gateway.</summary>
	[Get("/network-troubleshooting/v1/gateways/{serialNumber}/list-tasks")]
	Task<PagedResponse<TroubleshootingTaskSummary>> ListTasksAsync(
		string serialNumber,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets the result of a previously started command (for example <c>ping</c>,
	/// <c>pingSweep</c>, <c>traceroute</c> or <c>iperf</c>).
	/// </summary>
	[Get("/network-troubleshooting/v1/gateways/{serialNumber}/{command}/async-operations/{taskId}")]
	Task<TroubleshootingTask> GetCommandResultAsync(
		string serialNumber,
		string command,
		string taskId,
		CancellationToken cancellationToken = default);
}
