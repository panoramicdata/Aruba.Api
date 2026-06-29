namespace Aruba.Api.Data;

/// <summary>
/// The body for a device troubleshooting command (ping, traceroute, speed test, cable test, etc.).
/// Only the fields relevant to the chosen command need be supplied.
/// </summary>
public sealed class TroubleshootingCommandRequest
{
	/// <summary>The target host or IP address (ping, traceroute, nslookup, http/https, tcp).</summary>
	public string? Host { get; init; }

	/// <summary>The number of packets/probes to send, when applicable.</summary>
	public int? Count { get; init; }

	/// <summary>The packet size in bytes, when applicable.</summary>
	public int? PacketSize { get; init; }

	/// <summary>The TCP/UDP port, when applicable.</summary>
	public int? Port { get; init; }

	/// <summary>The interface or port to operate on (cable test, port bounce, PoE bounce).</summary>
	public string? Interface { get; init; }

	/// <summary>The VLAN identifier, when applicable.</summary>
	public string? VlanId { get; init; }

	/// <summary>Free-form additional command arguments, when the command supports them.</summary>
	public IReadOnlyDictionary<string, string>? Arguments { get; init; }
}

/// <summary>
/// The body for one or more device "show" commands.
/// </summary>
public sealed class ShowCommandsRequest
{
	/// <summary>The show commands to execute.</summary>
	public required IReadOnlyList<string> Commands { get; init; }
}

/// <summary>
/// The result of an asynchronous troubleshooting task, retrieved from an
/// <c>async-operations/{taskId}</c> endpoint.
/// </summary>
public sealed class TroubleshootingTask
{
	/// <summary>The task identifier.</summary>
	public string? TaskId { get; init; }

	/// <summary>The task status, for example <c>QUEUED</c>, <c>RUNNING</c>, <c>COMPLETED</c> or <c>FAILED</c>.</summary>
	public string? Status { get; init; }

	/// <summary>The command output, when the task has completed.</summary>
	public string? Output { get; init; }

	/// <summary>The time the task was created.</summary>
	public DateTimeOffset? CreatedAt { get; init; }

	/// <summary>The time the task completed, when applicable.</summary>
	public DateTimeOffset? CompletedAt { get; init; }
}

/// <summary>
/// A summary of a troubleshooting task as returned by the <c>list-tasks</c> endpoints.
/// </summary>
public sealed class TroubleshootingTaskSummary
{
	/// <summary>The task identifier.</summary>
	public string? TaskId { get; init; }

	/// <summary>The command that was run.</summary>
	public string? Command { get; init; }

	/// <summary>The task status.</summary>
	public string? Status { get; init; }

	/// <summary>The time the task was created.</summary>
	public DateTimeOffset? CreatedAt { get; init; }
}
