using Aruba.Api.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Aruba.Api.Authentication;

/// <summary>
/// Obtains and caches HPE GreenLake SSO access tokens using the OAuth 2.0
/// <c>client_credentials</c> grant, refreshing them automatically before they expire.
/// </summary>
/// <remarks>
/// The provider serialises concurrent refreshes so that only one token request is in flight at a
/// time. It uses its own <see cref="HttpClient"/> (without the authenticating handler) to avoid
/// recursion when acquiring a token.
/// </remarks>
internal sealed class OAuthTokenProvider : IDisposable
{
	private readonly ArubaCentralClientOptions _options;
	private readonly ILogger _logger;
	private readonly HttpClient _tokenHttpClient;
	private readonly bool _ownsHttpClient;
	private readonly SemaphoreSlim _gate = new(1, 1);

	private AccessToken? _current;

	public OAuthTokenProvider(ArubaCentralClientOptions options, HttpClient? tokenHttpClient = null)
	{
		_options = options;
		_logger = options.Logger;
		_ownsHttpClient = tokenHttpClient is null;
		_tokenHttpClient = tokenHttpClient ?? new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
	}

	/// <summary>
	/// Returns a valid access token, acquiring or refreshing one if necessary.
	/// </summary>
	/// <param name="forceRefresh">When <see langword="true"/>, any cached token is discarded first.</param>
	/// <param name="cancellationToken">A token to cancel the operation.</param>
	public async Task<string> GetAccessTokenAsync(bool forceRefresh, CancellationToken cancellationToken)
	{
		var now = DateTimeOffset.UtcNow;
		if (!forceRefresh && _current is { } cached && cached.IsValidAt(now))
		{
			return cached.Value;
		}

		await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			now = DateTimeOffset.UtcNow;
			if (!forceRefresh && _current is { } cached2 && cached2.IsValidAt(now))
			{
				return cached2.Value;
			}

			var token = await RequestTokenAsync(cancellationToken).ConfigureAwait(false);
			_current = token;
			return token.Value;
		}
		finally
		{
			_gate.Release();
		}
	}

	private async Task<AccessToken> RequestTokenAsync(CancellationToken cancellationToken)
	{
		_logger.LogDebug("Requesting access token from {TokenEndpoint} (grant_type=client_credentials).", _options.TokenEndpoint);

		using var form = new FormUrlEncodedContent(new Dictionary<string, string>
		{
			["grant_type"] = "client_credentials",
			["client_id"] = _options.ClientId,
			["client_secret"] = _options.ClientSecret,
		});

		using var request = new HttpRequestMessage(HttpMethod.Post, _options.TokenEndpoint) { Content = form };
		request.Headers.Add("Accept", "application/json");

		using var response = await _tokenHttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
		var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

		if (!response.IsSuccessStatusCode)
		{
			_logger.LogError("Token request failed with HTTP {StatusCode}.", (int)response.StatusCode);
			throw new ArubaAuthenticationException(
				$"Failed to obtain an access token (HTTP {(int)response.StatusCode}). Verify the ClientId, ClientSecret and TokenEndpoint.",
				response.StatusCode);
		}

		TokenResponse? parsed;
		try
		{
			parsed = JsonSerializer.Deserialize<TokenResponse>(body);
		}
		catch (JsonException)
		{
			throw new ArubaAuthenticationException("The token endpoint returned a response that could not be parsed as JSON.");
		}

		if (parsed is null || string.IsNullOrEmpty(parsed.AccessToken))
		{
			throw new ArubaAuthenticationException("The token endpoint response did not contain an access_token.");
		}

		var lifetimeSeconds = Math.Max(0, parsed.ExpiresIn - _options.TokenExpiryToleranceSeconds);
		var expiresAt = DateTimeOffset.UtcNow.AddSeconds(lifetimeSeconds);

		_logger.LogDebug("Obtained access token (expires_in={ExpiresIn}s).", parsed.ExpiresIn);
		return new AccessToken(parsed.AccessToken, expiresAt);
	}

	public void Dispose()
	{
		_gate.Dispose();
		if (_ownsHttpClient)
		{
			_tokenHttpClient.Dispose();
		}
	}

	private sealed class TokenResponse
	{
		[JsonPropertyName("access_token")]
		public string? AccessToken { get; init; }

		[JsonPropertyName("token_type")]
		public string? TokenType { get; init; }

		[JsonPropertyName("expires_in")]
		public int ExpiresIn { get; init; }
	}
}
