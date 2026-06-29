using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Aruba.Api;

/// <summary>
/// Configuration for an <see cref="ArubaCentralClient"/>.
/// </summary>
public class ArubaCentralClientOptions
{
	/// <summary>
	/// The regional API gateway base address for your HPE Aruba Networking Central account,
	/// for example <c>https://us2.api.central.arubanetworks.com</c>.
	/// Use a value from <see cref="ArubaCentralClusters"/> or supply your own — see
	/// <see href="https://developer.arubanetworks.com/new-central/docs/getting-started-with-rest-apis#finding-your-base-url">Finding your Base URL</see>.
	/// </summary>
	public required Uri BaseAddress { get; init; }

	/// <summary>
	/// The OAuth 2.0 client identifier (API client credentials) for New Central.
	/// </summary>
	public required string ClientId { get; init; }

	/// <summary>
	/// The OAuth 2.0 client secret (API client credentials) for New Central.
	/// This is a secret and must never be committed to source control or logged.
	/// </summary>
	public required string ClientSecret { get; init; }

	/// <summary>
	/// The HPE GreenLake SSO token endpoint used for the <c>client_credentials</c> grant.
	/// Defaults to <c>https://sso.common.cloud.hpe.com/as/token.oauth2</c>.
	/// </summary>
	public Uri TokenEndpoint { get; init; } = new("https://sso.common.cloud.hpe.com/as/token.oauth2");

	/// <summary>
	/// The <c>User-Agent</c> header sent with every request.
	/// </summary>
	public string UserAgent { get; init; } = "Aruba.Api/1.0";

	/// <summary>
	/// When <see langword="true"/>, only HTTP <c>GET</c> requests are permitted; any attempt to
	/// mutate state (POST/PUT/PATCH/DELETE) throws an <see cref="InvalidOperationException"/>.
	/// Useful as a safety guard for read-only monitoring integrations.
	/// </summary>
	public bool IsReadOnly { get; init; }

	/// <summary>
	/// The maximum number of attempts (including the first) for a single request before giving up.
	/// </summary>
	public int MaxAttemptCount { get; init; } = 5;

	/// <summary>
	/// The exponential back-off factor applied between retry attempts.
	/// </summary>
	public double BackOffDelayFactor { get; init; } = 1.5;

	/// <summary>
	/// The maximum back-off delay, in seconds, between retry attempts.
	/// </summary>
	public int MaxBackOffDelaySeconds { get; init; } = 60;

	/// <summary>
	/// A number of seconds subtracted from an access token's reported lifetime so that tokens
	/// are refreshed slightly before they actually expire.
	/// </summary>
	public int TokenExpiryToleranceSeconds { get; init; } = 30;

	/// <summary>
	/// The logger used for diagnostic output. Defaults to a no-op logger.
	/// </summary>
	public ILogger Logger { get; init; } = new NullLogger<ArubaCentralClient>();

	/// <summary>
	/// Validates that the options are internally consistent.
	/// </summary>
	/// <exception cref="ArgumentException">Thrown when a required value is missing or invalid.</exception>
	public void Validate()
	{
		if (BaseAddress is null)
		{
			throw new ArgumentException($"{nameof(BaseAddress)} is required.", nameof(BaseAddress));
		}

		if (string.IsNullOrWhiteSpace(ClientId))
		{
			throw new ArgumentException($"{nameof(ClientId)} is required.", nameof(ClientId));
		}

		if (string.IsNullOrWhiteSpace(ClientSecret))
		{
			throw new ArgumentException($"{nameof(ClientSecret)} is required.", nameof(ClientSecret));
		}

		if (MaxAttemptCount < 1)
		{
			throw new ArgumentException($"{nameof(MaxAttemptCount)} must be at least 1.", nameof(MaxAttemptCount));
		}
	}
}
