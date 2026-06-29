using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Notifications;

/// <summary>
/// Alert endpoints (<c>/network-notifications/v1/alerts</c>).
/// </summary>
public interface IAlerts
{
	/// <summary>Gets the list of alerts.</summary>
	[Get("/network-notifications/v1/alerts")]
	Task<PagedResponse<Alert>> GetAllAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Clears the specified alerts. Returns an acknowledgement to poll for completion.</summary>
	[Post("/network-notifications/v1/alerts/clear")]
	Task<AsyncOperationResponse> ClearAsync([Body] AlertActionRequest request, CancellationToken cancellationToken = default);

	/// <summary>Defers the specified alerts. Returns an acknowledgement to poll for completion.</summary>
	[Post("/network-notifications/v1/alerts/defer")]
	Task<AsyncOperationResponse> DeferAsync([Body] AlertActionRequest request, CancellationToken cancellationToken = default);

	/// <summary>Sets the specified alerts back to active. Returns an acknowledgement to poll for completion.</summary>
	[Post("/network-notifications/v1/alerts/active")]
	Task<AsyncOperationResponse> SetActiveAsync([Body] AlertActionRequest request, CancellationToken cancellationToken = default);

	/// <summary>Changes the priority of the specified alerts. Returns an acknowledgement to poll for completion.</summary>
	[Post("/network-notifications/v1/alerts/priority")]
	Task<AsyncOperationResponse> SetPriorityAsync([Body] AlertActionRequest request, CancellationToken cancellationToken = default);

	/// <summary>Gets the alert classification breakdown (by category or type).</summary>
	[Get("/network-notifications/v1/alerts/classification")]
	Task<AlertClassification> GetClassificationAsync([AliasAs("type")] string type = "category", CancellationToken cancellationToken = default);

	/// <summary>Gets the status of an asynchronous alert operation by task identifier.</summary>
	[Get("/network-notifications/v1/alerts/async-operations/{taskId}")]
	Task<TroubleshootingTask> GetAsyncOperationAsync(string taskId, CancellationToken cancellationToken = default);
}
