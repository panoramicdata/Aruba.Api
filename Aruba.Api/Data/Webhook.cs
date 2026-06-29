namespace Aruba.Api.Data;

/// <summary>
/// A webhook registration as reported by the New Central services API.
/// </summary>
public sealed class Webhook
{
	/// <summary>The webhook identifier.</summary>
	public string? Id { get; init; }

	/// <summary>The webhook name.</summary>
	public string? Name { get; init; }

	/// <summary>The New Central resource type discriminator.</summary>
	public string? Type { get; init; }

	/// <summary>The destination endpoint URL.</summary>
	public string? Endpoint { get; init; }

	/// <summary>The authentication mechanism, for example <c>API_KEY</c>.</summary>
	public string? AuthMechanism { get; init; }

	/// <summary>The configuration generation number.</summary>
	public int? Generation { get; init; }

	/// <summary>The HMAC signing key, returned when the webhook is created or its key is rotated.</summary>
	public string? HmacKey { get; init; }

	/// <summary>The time the webhook was created.</summary>
	public DateTimeOffset? CreatedAt { get; init; }

	/// <summary>The time the webhook was last updated.</summary>
	public DateTimeOffset? UpdatedAt { get; init; }
}

/// <summary>
/// The body for creating or updating a <see cref="Webhook"/>. New Central wraps the payload in an
/// <c>input</c> object.
/// </summary>
public sealed class WebhookRequest
{
	/// <summary>The wrapped webhook input.</summary>
	public required WebhookInput Input { get; init; }
}

/// <summary>
/// The webhook properties carried inside a <see cref="WebhookRequest"/>.
/// </summary>
public sealed class WebhookInput
{
	/// <summary>The webhook name.</summary>
	public required string Name { get; init; }

	/// <summary>The destination endpoint URL.</summary>
	public required string Endpoint { get; init; }

	/// <summary>The authentication mechanism, for example <c>API_KEY</c>.</summary>
	public string? AuthMechanism { get; init; }

	/// <summary>The API key, when <see cref="AuthMechanism"/> is <c>API_KEY</c>.</summary>
	public string? ApiKey { get; init; }
}
