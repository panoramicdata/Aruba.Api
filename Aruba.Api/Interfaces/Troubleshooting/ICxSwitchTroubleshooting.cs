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
	/// <summary>Starts a ping from the switch.</summary>
	[Post("/network-troubleshooting/v1/cx/{serialNumber}/ping")]
	Task<AsyncOperationResponse> PingAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Starts a traceroute from the switch.</summary>
	[Post("/network-troubleshooting/v1/cx/{serialNumber}/traceroute")]
	Task<AsyncOperationResponse> TracerouteAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Bounces PoE on a port.</summary>
	[Post("/network-troubleshooting/v1/cx/{serialNumber}/poeBounce")]
	Task<AsyncOperationResponse> PoeBounceAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Bounces a port (administratively down then up).</summary>
	[Post("/network-troubleshooting/v1/cx/{serialNumber}/portBounce")]
	Task<AsyncOperationResponse> PortBounceAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Runs a cable test on a port.</summary>
	[Post("/network-troubleshooting/v1/cx/{serialNumber}/cableTest")]
	Task<AsyncOperationResponse> CableTestAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

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
