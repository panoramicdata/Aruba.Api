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
	/// <summary>Starts a ping from the gateway.</summary>
	[Post("/network-troubleshooting/v1/gateways/{serialNumber}/ping")]
	Task<AsyncOperationResponse> PingAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Starts a ping sweep from the gateway.</summary>
	[Post("/network-troubleshooting/v1/gateways/{serialNumber}/pingSweep")]
	Task<AsyncOperationResponse> PingSweepAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Starts a traceroute from the gateway.</summary>
	[Post("/network-troubleshooting/v1/gateways/{serialNumber}/traceroute")]
	Task<AsyncOperationResponse> TracerouteAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Runs an iperf throughput test from the gateway.</summary>
	[Post("/network-troubleshooting/v1/gateways/{serialNumber}/iperf")]
	Task<AsyncOperationResponse> IperfAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Runs one or more show commands on the gateway.</summary>
	[Post("/network-troubleshooting/v1/gateways/{serialNumber}/showCommands")]
	Task<AsyncOperationResponse> ShowCommandsAsync(string serialNumber, [Body] ShowCommandsRequest request, CancellationToken cancellationToken = default);

	/// <summary>Gets the list of show commands supported by the gateway.</summary>
	[Get("/network-troubleshooting/v1/gateways/{serialNumber}/show-commands")]
	Task<PagedResponse<string>> GetSupportedShowCommandsAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Reboots the gateway.</summary>
	[Post("/network-troubleshooting/v1/gateways/{serialNumber}/reboot")]
	Task<AsyncOperationResponse> RebootAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Disconnects a specific client from the gateway by MAC address.</summary>
	[Post("/network-troubleshooting/v1/gateways/{serialNumber}/disconnectClientByMacAddress")]
	Task<AsyncOperationResponse> DisconnectClientAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Lists the recent troubleshooting tasks for the gateway.</summary>
	[Get("/network-troubleshooting/v1/gateways/{serialNumber}/list-tasks")]
	Task<PagedResponse<TroubleshootingTaskSummary>> ListTasksAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the result of a previously started command (for example <c>ping</c> or <c>iperf</c>).</summary>
	[Get("/network-troubleshooting/v1/gateways/{serialNumber}/{command}/async-operations/{taskId}")]
	Task<TroubleshootingTask> GetCommandResultAsync(string serialNumber, string command, string taskId, CancellationToken cancellationToken = default);
}
