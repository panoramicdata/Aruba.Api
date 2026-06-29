using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aruba.Api.Converters;

/// <summary>
/// A <see cref="DateTimeOffset"/>? converter that tolerates the empty strings and Unix-epoch
/// millisecond timestamps that New Central uses interchangeably with ISO-8601 dates.
/// An empty or whitespace string is read as <see langword="null"/>.
/// </summary>
internal sealed class TolerantDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
	public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		switch (reader.TokenType)
		{
			case JsonTokenType.Null:
				return null;

			case JsonTokenType.Number:
				// Epoch milliseconds.
				return DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64());

			case JsonTokenType.String:
				var text = reader.GetString();
				if (string.IsNullOrWhiteSpace(text))
				{
					return null;
				}

				if (DateTimeOffset.TryParse(text, out var parsed))
				{
					return parsed;
				}

				// Some endpoints return epoch milliseconds as a string.
				if (long.TryParse(text, out var epochMs))
				{
					return DateTimeOffset.FromUnixTimeMilliseconds(epochMs);
				}

				throw new JsonException($"Could not parse '{text}' as a DateTimeOffset.");

			default:
				throw new JsonException($"Unexpected token {reader.TokenType} when parsing a DateTimeOffset.");
		}
	}

	public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
	{
		if (value is null)
		{
			writer.WriteNullValue();
		}
		else
		{
			writer.WriteStringValue(value.Value);
		}
	}
}
