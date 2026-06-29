using Microsoft.Extensions.Logging;

namespace Aruba.Api;

/// <summary>
/// A mutable builder for <see cref="ArubaCentralClientOptions"/>, used by the dependency-injection
/// registration helpers where an <see cref="Action{T}"/> configuration callback is more ergonomic
/// than constructing the init-only options directly.
/// </summary>
public sealed class ArubaCentralClientOptionsBuilder
{
	/// <inheritdoc cref="ArubaCentralClientOptions.BaseAddress"/>
	public Uri? BaseAddress { get; set; }

	/// <inheritdoc cref="ArubaCentralClientOptions.ClientId"/>
	public string? ClientId { get; set; }

	/// <inheritdoc cref="ArubaCentralClientOptions.ClientSecret"/>
	public string? ClientSecret { get; set; }

	/// <inheritdoc cref="ArubaCentralClientOptions.TokenEndpoint"/>
	public Uri TokenEndpoint { get; set; } = new("https://sso.common.cloud.hpe.com/as/token.oauth2");

	/// <inheritdoc cref="ArubaCentralClientOptions.UserAgent"/>
	public string UserAgent { get; set; } = "Aruba.Api/1.0";

	/// <inheritdoc cref="ArubaCentralClientOptions.IsReadOnly"/>
	public bool IsReadOnly { get; set; }

	/// <inheritdoc cref="ArubaCentralClientOptions.MaxAttemptCount"/>
	public int MaxAttemptCount { get; set; } = 5;

	/// <inheritdoc cref="ArubaCentralClientOptions.BackOffDelayFactor"/>
	public double BackOffDelayFactor { get; set; } = 1.5;

	/// <inheritdoc cref="ArubaCentralClientOptions.MaxBackOffDelaySeconds"/>
	public int MaxBackOffDelaySeconds { get; set; } = 60;

	/// <inheritdoc cref="ArubaCentralClientOptions.TokenExpiryToleranceSeconds"/>
	public int TokenExpiryToleranceSeconds { get; set; } = 30;

	/// <inheritdoc cref="ArubaCentralClientOptions.Logger"/>
	public ILogger? Logger { get; set; }

	/// <summary>
	/// Builds an immutable <see cref="ArubaCentralClientOptions"/> from the current values.
	/// </summary>
	/// <exception cref="ArgumentException">Thrown when a required value is missing.</exception>
	public ArubaCentralClientOptions Build()
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

		var options = new ArubaCentralClientOptions
		{
			BaseAddress = BaseAddress,
			ClientId = ClientId,
			ClientSecret = ClientSecret,
			TokenEndpoint = TokenEndpoint,
			UserAgent = UserAgent,
			IsReadOnly = IsReadOnly,
			MaxAttemptCount = MaxAttemptCount,
			BackOffDelayFactor = BackOffDelayFactor,
			MaxBackOffDelaySeconds = MaxBackOffDelaySeconds,
			TokenExpiryToleranceSeconds = TokenExpiryToleranceSeconds,
			Logger = Logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<ArubaCentralClient>.Instance,
		};

		options.Validate();
		return options;
	}
}
