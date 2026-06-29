using Aruba.Api.Data;

namespace Aruba.Api.Interfaces.Services;

/// <summary>
/// Webhook management endpoints (<c>/network-services/v1/webhooks</c>).
/// </summary>
public interface IWebhooks
{
	/// <summary>Gets the list of registered webhooks.</summary>
	[Get("/network-services/v1/webhooks")]
	Task<PagedResponse<Webhook>> GetAllAsync(ListQuery? query = null, CancellationToken cancellationToken = default);

	/// <summary>Gets a single webhook by identifier.</summary>
	[Get("/network-services/v1/webhooks/{id}")]
	Task<Webhook> GetAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>Registers a new webhook.</summary>
	[Post("/network-services/v1/webhooks")]
	Task<Webhook> CreateAsync([Body] WebhookRequest request, CancellationToken cancellationToken = default);

	/// <summary>Replaces an existing webhook.</summary>
	[Put("/network-services/v1/webhooks/{id}")]
	Task<Webhook> UpdateAsync(string id, [Body] WebhookRequest request, CancellationToken cancellationToken = default);

	/// <summary>Applies a partial update to an existing webhook.</summary>
	[Patch("/network-services/v1/webhooks/{id}")]
	Task<Webhook> PatchAsync(string id, [Body] WebhookRequest request, CancellationToken cancellationToken = default);

	/// <summary>Deletes a webhook.</summary>
	[Delete("/network-services/v1/webhooks/{id}")]
	Task DeleteAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>Rotates the HMAC signing key for a webhook.</summary>
	[Post("/network-services/v1/webhooks/{id}/rotate-hmac-key")]
	Task<Webhook> RotateHmacKeyAsync(string id, CancellationToken cancellationToken = default);
}
