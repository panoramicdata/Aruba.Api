namespace Aruba.Api.Data;

/// <summary>
/// The acknowledgement returned by asynchronous operations (for example clearing alerts or running
/// a device troubleshooting command). Poll the corresponding <c>async-operations/{taskId}</c>
/// endpoint to obtain the result.
/// </summary>
public sealed class AsyncOperationResponse
{
	/// <summary>A human-readable acknowledgement, for example <c>"Request accepted"</c>.</summary>
	public string? Response { get; init; }

	/// <summary>The identifier of the asynchronous task, used to poll for the result.</summary>
	public string? TaskId { get; init; }

	/// <summary>The relative URL at which the task status can be polled.</summary>
	public string? Location { get; init; }
}
