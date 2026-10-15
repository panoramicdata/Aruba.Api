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
	/// Starts a ping from the switch. The payload must list the destinations to probe;
	/// follow up with <see cref="GetCommandResultAsync"/> to collect the replies.
	/// </summary>
	[Post("/network-troubleshooting/v1/aos-s/{serialNumber}/ping")]
	Task<AsyncOperationResponse> PingAsync(
		string serialNumber,
		[Body] TroubleshootingCommandRequest command,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Traces the network path to a target, reporting each hop along the way.
	/// </summary>
	[Post("/network-troubleshooting/v1/aos-s/{serialNumber}/traceroute")]
	Task<AsyncOperationResponse> TracerouteAsync(
		string serialNumber,
		[Body] TroubleshootingCommandRequest command,
		CancellationToken cancellationToken = default);

	/// <summary>Bounces PoE on a port.</summary>
	[Post("/network-troubleshooting/v1/aos-s/{serialNumber}/poeBounce")]
	Task<AsyncOperationResponse> PoeBounceAsync(
		string serialNumber,
		[Body] TroubleshootingCommandRequest command,
		CancellationToken cancellationToken = default);

	/// <summary>Bounces a port (administratively down then up).</summary>
	[Post("/network-troubleshooting/v1/aos-s/{serialNumber}/portBounce")]
	Task<AsyncOperationResponse> PortBounceAsync(
		string serialNumber,
		[Body] TroubleshootingCommandRequest command,
		CancellationToken cancellationToken = default);

	/// <summary>Verifies the physical integrity of the cable attached to a port.</summary>
	[Post("/network-troubleshooting/v1/aos-s/{serialNumber}/cableTest")]
	Task<AsyncOperationResponse> CableTestAsync(
		string serialNumber,
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
