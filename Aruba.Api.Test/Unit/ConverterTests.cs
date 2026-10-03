using Aruba.Api.Converters;
using AwesomeAssertions;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Aruba.Api.Test.Unit;

public sealed class TolerantDateTimeOffsetConverterTests
{
	private static readonly JsonSerializerOptions Options = CreateOptions();

	private static JsonSerializerOptions CreateOptions()
	{
		var options = new JsonSerializerOptions();
		options.Converters.Add(new TolerantDateTimeOffsetConverter());
		return options;
	}

	private static readonly DateTimeOffset Epoch2021 = new(2021, 1, 1, 0, 0, 0, TimeSpan.Zero);
	private const long Epoch2021Ms = 1609459200000;

	[Fact]
	public void Read_EmptyString_IsNull()
		=> JsonSerializer.Deserialize<DateTimeOffset?>("\"\"", Options).Should().BeNull();

	[Fact]
	public void Read_Whitespace_IsNull()
		=> JsonSerializer.Deserialize<DateTimeOffset?>("\"   \"", Options).Should().BeNull();

	[Fact]
	public void Read_Null_IsNull()
		=> JsonSerializer.Deserialize<DateTimeOffset?>("null", Options).Should().BeNull();

	[Fact]
	public void Read_EpochMillisecondsNumber_IsParsed()
		=> JsonSerializer.Deserialize<DateTimeOffset?>(Epoch2021Ms.ToString(), Options)
			.Should().Be(Epoch2021);

	[Fact]
	public void Read_EpochMillisecondsString_IsParsed()
		=> JsonSerializer.Deserialize<DateTimeOffset?>($"\"{Epoch2021Ms}\"", Options)
			.Should().Be(Epoch2021);

	[Fact]
	public void Read_Iso8601String_IsParsed()
		=> JsonSerializer.Deserialize<DateTimeOffset?>("\"2021-01-01T00:00:00Z\"", Options)
			.Should().Be(Epoch2021);

	[Fact]
	public void Read_UnparseableString_Throws()
	{
		var act = () => JsonSerializer.Deserialize<DateTimeOffset?>("\"not-a-date\"", Options);
		act.Should().Throw<JsonException>();
	}

	[Fact]
	public void Read_UnexpectedToken_Throws()
	{
		var act = () => JsonSerializer.Deserialize<DateTimeOffset?>("true", Options);
		act.Should().Throw<JsonException>();
	}

	[Fact]
	public void Write_Null_EmitsNull()
		=> JsonSerializer.Serialize<DateTimeOffset?>(null, Options).Should().Be("null");

	[Fact]
	public void Write_Value_RoundTrips()
	{
		var json = JsonSerializer.Serialize<DateTimeOffset?>(Epoch2021, Options);
		JsonSerializer.Deserialize<DateTimeOffset?>(json, Options).Should().Be(Epoch2021);
	}
}

public sealed class BetterJsonStringEnumConverterTests
{
	private enum Colour
	{
		[EnumMember(Value = "bright-red")]
		Red,

		// No EnumMember — must fall back to the field name.
		Green,
	}

	private static readonly JsonSerializerOptions Options = CreateOptions();

	private static JsonSerializerOptions CreateOptions()
	{
		var options = new JsonSerializerOptions();
		options.Converters.Add(new BetterJsonStringEnumConverter());
		return options;
	}

	[Fact]
	public void CanConvert_EnumTrue_NonEnumFalse()
	{
		var factory = new BetterJsonStringEnumConverter();
		factory.CanConvert(typeof(Colour)).Should().BeTrue();
		factory.CanConvert(typeof(string)).Should().BeFalse();
	}

	[Fact]
	public void Write_UsesEnumMemberValue()
		=> JsonSerializer.Serialize(Colour.Red, Options).Should().Be("\"bright-red\"");

	[Fact]
	public void Write_FallsBackToFieldName()
		=> JsonSerializer.Serialize(Colour.Green, Options).Should().Be("\"Green\"");

	[Fact]
	public void Write_UndefinedValue_FallsBackToToString()
		=> JsonSerializer.Serialize((Colour)999, Options).Should().Be("\"999\"");

	[Fact]
	public void Read_MapsEnumMemberValue()
		=> JsonSerializer.Deserialize<Colour>("\"bright-red\"", Options).Should().Be(Colour.Red);

	[Fact]
	public void Read_MapsFieldName()
		=> JsonSerializer.Deserialize<Colour>("\"Green\"", Options).Should().Be(Colour.Green);

	[Fact]
	public void Read_UnknownValue_Throws()
	{
		var act = () => JsonSerializer.Deserialize<Colour>("\"purple\"", Options);
		act.Should().Throw<JsonException>();
	}
}
