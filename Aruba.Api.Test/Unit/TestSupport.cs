using Aruba.Api.Converters;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Aruba.Api.Test.Unit;

/// <summary>
/// Shared helpers for the unit tests: a <see cref="JsonSerializerOptions"/> instance that mirrors the
/// one <see cref="ArubaCentralClient"/> configures for Refit, plus a capturing <see cref="ILogger"/>.
/// </summary>
internal static class TestSupport
{
	/// <summary>
	/// The same serializer configuration the client applies to every response (camelCase, tolerant
	/// dates), so model-deserialization tests exercise the real code path.
	/// </summary>
	public static JsonSerializerOptions JsonOptions { get; } = CreateJsonOptions();

	private static JsonSerializerOptions CreateJsonOptions()
	{
		var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			PropertyNameCaseInsensitive = true,
		};
		options.Converters.Add(new TolerantDateTimeOffsetConverter());
		return options;
	}

	/// <summary>Deserializes <paramref name="json"/> using <see cref="JsonOptions"/>.</summary>
	public static T Deserialize<T>(string json)
		=> JsonSerializer.Deserialize<T>(json, JsonOptions)
			?? throw new InvalidOperationException($"Deserialization of {typeof(T).Name} returned null.");
}

/// <summary>
/// An <see cref="ILogger"/> that captures every formatted message, for asserting on diagnostic output
/// (for example that secrets are masked). Enabled for all levels.
/// </summary>
internal sealed class CapturingLogger : ILogger
{
	public List<string> Messages { get; } = [];

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

	public bool IsEnabled(LogLevel logLevel) => true;

	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter)
		=> Messages.Add(formatter(state, exception));

	/// <summary>The concatenation of all captured messages, for simple substring assertions.</summary>
	public string AllText => string.Join("\n", Messages);
}
