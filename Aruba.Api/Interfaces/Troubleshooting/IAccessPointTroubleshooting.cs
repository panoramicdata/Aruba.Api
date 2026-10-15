using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Troubleshooting;

/// <summary>
/// Live diagnostics for an access point (<c>/network-troubleshooting/v1/aps</c>).
/// </summary>
/// <remarks>
/// Commands are asynchronous; poll <see cref="GetCommandResultAsync"/> with the originating
/// command name and task id to retrieve the result.
/// </remarks>
public interface IAccessPointTroubleshooting
{
	/// <summary>
	/// Runs a troubleshooting command on the access point. <paramref name="command"/> names the
	/// command to run — <c>ping</c>, <c>traceroute</c>, <c>speedtest</c>, <c>nslookup</c> or
	/// <c>disconnectUserByMacAddress</c> — and <paramref name="request"/> carries its parameters.
	/// </summary>
	[Post("/network-troubleshooting/v1/aps/{serialNumber}/{command}")]
	Task<AsyncOperationResponse> CommandAsync(
		string serialNumber,
		[Body] TroubleshootingCommandRequest request,
		string command,
		CancellationToken cancellationToken = default);

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

	/// <summary>Lists the recent troubleshooting tasks for the access point.</summary>
	[Get("/network-troubleshooting/v1/aps/{serialNumber}/list-tasks")]
	Task<PagedResponse<TroubleshootingTaskSummary>> ListTasksAsync(string serialNumber, CancellationToken cancellationToken = default);

	/// <summary>Gets the result of a previously started command (for example <c>ping</c>, <c>traceroute</c> or <c>speedtest</c>).</summary>
	[Get("/network-troubleshooting/v1/aps/{serialNumber}/{command}/async-operations/{taskId}")]
	Task<TroubleshootingTask> GetCommandResultAsync(string serialNumber, string command, string taskId, CancellationToken cancellationToken = default);
}
