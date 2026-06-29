using Aruba.Api.Authentication;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Aruba.Api;

/// <summary>
/// A <see cref="DelegatingHandler"/> that injects a bearer token from
/// <see cref="OAuthTokenProvider"/>, enforces read-only mode, logs requests/responses, and
/// retries transient failures (HTTP 401/429/5xx) with exponential back-off.
/// </summary>
internal sealed class AuthenticatedBackingOffHandler : DelegatingHandler
{
	private readonly ArubaCentralClientOptions _options;
	private readonly OAuthTokenProvider _tokenProvider;
	private readonly ILogger _logger;
	private const LogLevel TraceLevel = LogLevel.Debug;

	public AuthenticatedBackingOffHandler(ArubaCentralClientOptions options, OAuthTokenProvider tokenProvider)
	{
		_options = options;
		_tokenProvider = tokenProvider;
		_logger = options.Logger;
	}

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		ValidateReadOnlyMode(request);
		AddRequestHeaders(request);

		var logPrefix = $"Request {Guid.NewGuid()}: ";
		var attemptCount = 0;
		var tokenRefreshed = false;

		while (true)
		{
			attemptCount++;
			cancellationToken.ThrowIfCancellationRequested();

			// Acquire (or reuse a cached) token and set the Authorization header for this attempt.
			var forceRefresh = tokenRefreshed;
			var token = await _tokenProvider.GetAccessTokenAsync(forceRefresh, cancellationToken).ConfigureAwait(false);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

			await LogRequestAsync(logPrefix, request, cancellationToken).ConfigureAwait(false);

			var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

			await LogResponseAsync(logPrefix, response, cancellationToken).ConfigureAwait(false);

			// On a 401, refresh the token once and retry immediately.
			if (response.StatusCode == HttpStatusCode.Unauthorized && !tokenRefreshed && attemptCount < _options.MaxAttemptCount)
			{
				tokenRefreshed = true;
				_logger.LogDebug("{LogPrefix}Received 401; refreshing access token and retrying.", logPrefix);
				response.Dispose();
				continue;
			}

			var delay = GetRetryDelay((int)response.StatusCode, response, attemptCount, logPrefix);
			if (delay == TimeSpan.Zero || attemptCount >= _options.MaxAttemptCount)
			{
				if (delay != TimeSpan.Zero)
				{
					LogGivingUp(logPrefix, (int)response.StatusCode, attemptCount, request);
				}

				return response;
			}

			LogRetrying(logPrefix, (int)response.StatusCode, attemptCount, delay, request);
			response.Dispose();
			await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
		}
	}

	private void ValidateReadOnlyMode(HttpRequestMessage request)
	{
		if (_options.IsReadOnly && request.Method != HttpMethod.Get)
		{
			throw new Exceptions.ArubaReadOnlyViolationException(request.Method, request.RequestUri);
		}
	}

	private void AddRequestHeaders(HttpRequestMessage request)
	{
		if (!request.Headers.Contains("User-Agent"))
		{
			request.Headers.Add("User-Agent", _options.UserAgent);
		}
	}

	private TimeSpan GetRetryDelay(int statusCode, HttpResponseMessage response, int attemptCount, string logPrefix)
		=> statusCode switch
		{
			429 => Handle429(response, attemptCount, logPrefix),
			502 or 503 or 504 => HandleServerError(statusCode, attemptCount, logPrefix),
			_ => TimeSpan.Zero,
		};

	private TimeSpan Handle429(HttpResponseMessage response, int attemptCount, string logPrefix)
	{
		var retryAfterSeconds = GetRetryAfterSeconds(response);
		_logger.LogDebug(
			"{LogPrefix}Received 429 on attempt {AttemptCount}/{MaxAttemptCount}.",
			logPrefix, attemptCount, _options.MaxAttemptCount);
		return CalculateBackoffDelay(attemptCount, retryAfterSeconds, _options.BackOffDelayFactor, _options.MaxBackOffDelaySeconds);
	}

	private TimeSpan HandleServerError(int statusCode, int attemptCount, string logPrefix)
	{
		_logger.LogInformation(
			"{LogPrefix}Received {StatusCode} on attempt {AttemptCount}/{MaxAttemptCount}.",
			logPrefix, statusCode, attemptCount, _options.MaxAttemptCount);
		return CalculateBackoffDelay(attemptCount, 1, _options.BackOffDelayFactor, _options.MaxBackOffDelaySeconds);
	}

	private static int GetRetryAfterSeconds(HttpResponseMessage response)
	{
		if (response.Headers.RetryAfter?.Delta is { } delta)
		{
			return (int)Math.Ceiling(delta.TotalSeconds);
		}

		var found = response.Headers.TryGetValues("Retry-After", out var values);
		var raw = found ? values?.FirstOrDefault() ?? "1" : "1";
		return int.TryParse(raw, out var seconds) ? seconds : 1;
	}

	private async Task LogRequestAsync(string logPrefix, HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (!_logger.IsEnabled(TraceLevel))
		{
			return;
		}

		_logger.Log(TraceLevel, "{LogPrefix}{Method} {Uri}", logPrefix, request.Method, request.RequestUri);
		_logger.Log(TraceLevel, "{LogPrefix}Request Headers:\r\n{Headers}", logPrefix, FormatHeaders(request.Headers));
		if (request.Content is not null)
		{
			var content = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			_logger.Log(TraceLevel, "{LogPrefix}Request Body:\r\n{RequestContent}", logPrefix, content);
		}
	}

	private async Task LogResponseAsync(string logPrefix, HttpResponseMessage response, CancellationToken cancellationToken)
	{
		if (!_logger.IsEnabled(TraceLevel))
		{
			return;
		}

		_logger.Log(TraceLevel, "{LogPrefix}Response {StatusCode}", logPrefix, (int)response.StatusCode);
		_logger.Log(TraceLevel, "{LogPrefix}Response Headers:\r\n{Headers}", logPrefix, FormatHeaders(response.Headers));
		if (response.Content is not null)
		{
			var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			_logger.Log(TraceLevel, "{LogPrefix}Response Body:\r\n{ResponseContent}", logPrefix, content);
		}
	}

	private void LogGivingUp(string logPrefix, int statusCode, int attemptCount, HttpRequestMessage request)
		=> _logger.LogInformation(
			"{LogPrefix}Giving up. Returning {StatusCode} after {AttemptCount}/{MaxAttemptCount} attempts. ({Method} {Url})",
			logPrefix, statusCode, attemptCount, _options.MaxAttemptCount, request.Method, request.RequestUri);

	private void LogRetrying(string logPrefix, int statusCode, int attemptCount, TimeSpan delay, HttpRequestMessage request)
		=> _logger.LogInformation(
			"{LogPrefix}Received {StatusCode} on attempt {AttemptCount}/{MaxAttemptCount} - waiting {Seconds:N2}s. ({Method} {Url})",
			logPrefix, statusCode, attemptCount, _options.MaxAttemptCount, delay.TotalSeconds, request.Method, request.RequestUri);

	private static string FormatHeaders(HttpHeaders headers)
	{
		var sb = new StringBuilder();
		foreach (var header in headers)
		{
			var value = header.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase)
				? "***MASKED***"
				: string.Join(", ", header.Value);
			sb.AppendLine($"  {header.Key}: {value}");
		}

		return sb.ToString().TrimEnd();
	}

	/// <summary>
	/// Calculates the back-off delay, honouring any <c>Retry-After</c> hint but never exceeding the
	/// configured maximum: wait at least <paramref name="retryAfterSeconds"/>, then back off by
	/// <paramref name="backOffDelayFactor"/> raised to the power of the attempt count.
	/// </summary>
	internal static TimeSpan CalculateBackoffDelay(
		int attemptCount,
		int retryAfterSeconds,
		double backOffDelayFactor,
		int maxBackOffDelaySeconds)
		=> TimeSpan.FromSeconds(
			Math.Min(
				Math.Max(
					Math.Pow(backOffDelayFactor, attemptCount - 1),
					retryAfterSeconds),
				maxBackOffDelaySeconds));
}
