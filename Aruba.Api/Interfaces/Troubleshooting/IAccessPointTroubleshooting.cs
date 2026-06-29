using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Troubleshooting;

/// <summary>
/// Live troubleshooting commands for access points (<c>/network-troubleshooting/v1/aps</c>).
/// </summary>
/// <remarks>
/// Commands are asynchronous: each returns an <see cref="AsyncOperationResponse"/> containing a
/// <c>taskId</c>. Poll <see cref="GetCommandResultAsync"/> with the originating command name and
/// task id to retrieve the result.
/// </remarks>
public interface IAccessPointTroubleshooting
{
	/// <summary>Starts a ping from the access point.</summary>
	[Post("/network-troubleshooting/v1/aps/{serialNumber}/ping")]
	Task<AsyncOperationResponse> PingAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Starts a traceroute from the access point.</summary>
	[Post("/network-troubleshooting/v1/aps/{serialNumber}/traceroute")]
	Task<AsyncOperationResponse> TracerouteAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Starts a speed test from the access point.</summary>
	[Post("/network-troubleshooting/v1/aps/{serialNumber}/speedtest")]
	Task<AsyncOperationResponse> SpeedTestAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Starts an nslookup from the access point.</summary>
	[Post("/network-troubleshooting/v1/aps/{serialNumber}/nslookup")]
	Task<AsyncOperationResponse> NsLookupAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Runs one or more show commands on the access point.</summary>
	[Post("/network-troubleshooting/v1/aps/{serialNumber}/showCommands")]
	Task<AsyncOperationResponse> ShowCommandsAsync(string serialNumber, [Body] ShowCommandsRequest request, CancellationToken cancellationToken = default);

	/// <summary>Gets the list of show commands supported by the access point.</summary>
	[Get("/network-troubleshooting/v1/aps/{serialNumber}/show-commands")]
	Task<PagedResponse<string>> GetSupportedShowCommandsAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Reboots the access point.</summary>
	[Post("/network-troubleshooting/v1/aps/{serialNumber}/reboot")]
	Task<AsyncOperationResponse> RebootAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Flashes the access point's locator LED.</summary>
	[Post("/network-troubleshooting/v1/aps/{serialNumber}/locate")]
	Task<AsyncOperationResponse> LocateAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Disconnects a specific client from the access point by MAC address.</summary>
	[Post("/network-troubleshooting/v1/aps/{serialNumber}/disconnectUserByMacAddress")]
	Task<AsyncOperationResponse> DisconnectClientAsync(string serialNumber, [Body] TroubleshootingCommandRequest request, CancellationToken cancellationToken = default);

	/// <summary>Lists the recent troubleshooting tasks for the access point.</summary>
	[Get("/network-troubleshooting/v1/aps/{serialNumber}/list-tasks")]
	Task<PagedResponse<TroubleshootingTaskSummary>> ListTasksAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets the result of a previously started command. <paramref name="command"/> is the command
	/// segment (for example <c>ping</c>, <c>traceroute</c> or <c>speedtest</c>).
	/// </summary>
	[Get("/network-troubleshooting/v1/aps/{serialNumber}/{command}/async-operations/{taskId}")]
	Task<TroubleshootingTask> GetCommandResultAsync(string serialNumber, string command, string taskId, CancellationToken cancellationToken = default);
}
