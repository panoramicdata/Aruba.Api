using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aruba.Api.Converters;

/// <summary>
/// A <see cref="JsonConverterFactory"/> that serialises enums using their
/// <see cref="EnumMemberAttribute"/> value where present, falling back to the field name.
/// </summary>
internal sealed class BetterJsonStringEnumConverter : JsonConverterFactory
{
	public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		var converterType = typeof(EnumMemberConverter<>).MakeGenericType(typeToConvert);
		return Activator.CreateInstance(converterType) as JsonConverter
			?? throw new InvalidOperationException($"Could not create {nameof(BetterJsonStringEnumConverter)} as a {nameof(JsonConverter)}");
	}

	private sealed class EnumMemberConverter<T> : JsonConverter<T> where T : struct, Enum
	{
		private readonly Dictionary<string, T> _fromValue = [];
		private readonly Dictionary<T, string> _toValue = [];

		public EnumMemberConverter()
		{
			foreach (var field in typeof(T).GetFields().Where(f => f.IsStatic))
			{
				var enumValue = field.GetValue(null) is T value
					? value
					: throw new InvalidOperationException($"Field {field.Name} is not a valid enum value.");
				var enumMemberAttr = field.GetCustomAttributes(typeof(EnumMemberAttribute), false)
					.Cast<EnumMemberAttribute>()
					.FirstOrDefault();

				var valueString = enumMemberAttr?.Value ?? field.Name;
				_fromValue[valueString] = enumValue;
				_toValue[enumValue] = valueString;
			}
		}

		public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			_ = typeToConvert; // Required by the JsonConverter<T> signature.
			_ = options; // Required by the JsonConverter<T> signature.
			var value = reader.GetString() ?? throw new JsonException($"Could not read a string for {typeof(T).Name}.");
			if (_fromValue.TryGetValue(value, out var result))
			{
				return result;
			}

			throw new JsonException($"Unknown {typeof(T).Name} enum value: {value}");
		}

		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
		{
			_ = options; // Required by the JsonConverter<T> signature.
			if (_toValue.TryGetValue(value, out var stringValue))
			{
				writer.WriteStringValue(stringValue);
			}
			else
			{
				writer.WriteStringValue(value.ToString());
			}
		}
	}
}
