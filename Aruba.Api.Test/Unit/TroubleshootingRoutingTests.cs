using Aruba.Api.Data;
using AwesomeAssertions;
using System.Net;
using System.Net.Http;

namespace Aruba.Api.Test.Unit;

/// <summary>
/// The shared troubleshooting interface turns a <see cref="TroubleshootingTarget"/> into the route segment the
/// service expects. These tests read the URL that actually goes out, since the enum formatting is the whole risk.
/// </summary>
public sealed class TroubleshootingRoutingTests
{
	private static (ArubaCentralClient Client, MockHttpMessageHandler Handler) CreateClient()
	{
		var handler = new MockHttpMessageHandler((request, _) =>
			MockHttpMessageHandler.IsTokenRequest(request)
				? MockHttpMessageHandler.TokenResponse()
				: MockHttpMessageHandler.Json(HttpStatusCode.OK, "{}"));

		var options = new ArubaCentralClientOptions
		{
			BaseAddress = ArubaCentralClusters.UsWest,
			ClientId = "test-id",
			ClientSecret = "test-secret",
			IsReadOnly = false,
		};

		return (new ArubaCentralClient(options, handler), handler);
	}

	private static HttpRequestMessage Api(MockHttpMessageHandler handler)
		=> handler.Requests.Last(request => !MockHttpMessageHandler.IsTokenRequest(request));

	[Theory]
	[InlineData(TroubleshootingTarget.AccessPoint, "aps")]
	[InlineData(TroubleshootingTarget.CxSwitch, "cx")]
	[InlineData(TroubleshootingTarget.AosSwitch, "aos-s")]
	[InlineData(TroubleshootingTarget.Gateway, "gateways")]
	public async Task EveryOperation_UsesTheTargetsRouteSegment(TroubleshootingTarget target, string segment)
	{
		var (client, handler) = CreateClient();
		using var _ = client;
		var token = TestContext.Current.CancellationToken;
		var prefix = $"/network-troubleshooting/v1/{segment}/SN1";

		await client.Troubleshooting.RebootAsync(target, "SN1", token);
		Api(handler).Method.Should().Be(HttpMethod.Post);
		Api(handler).RequestUri!.AbsolutePath.Should().Be($"{prefix}/reboot");

		await client.Troubleshooting.LocateAsync(target, "SN1", token);
		Api(handler).RequestUri!.AbsolutePath.Should().Be($"{prefix}/locate");

		await client.Troubleshooting.ListTasksAsync(target, "SN1", token);
		Api(handler).Method.Should().Be(HttpMethod.Get);
		Api(handler).RequestUri!.AbsolutePath.Should().Be($"{prefix}/list-tasks");

		await client.Troubleshooting.GetSupportedShowCommandsAsync(target, "SN1", token);
		Api(handler).RequestUri!.AbsolutePath.Should().Be($"{prefix}/show-commands");

		await client.Troubleshooting.ShowCommandsAsync(target, "SN1", new ShowCommandsRequest { Commands = ["show version"] }, token);
		Api(handler).Method.Should().Be(HttpMethod.Post);
		Api(handler).RequestUri!.AbsolutePath.Should().Be($"{prefix}/showCommands");

		await client.Troubleshooting.CommandAsync(target, "SN1", TroubleshootingCommands.Ping, new TroubleshootingCommandRequest { Host = "probe.example" }, token);
		Api(handler).Method.Should().Be(HttpMethod.Post);
		Api(handler).RequestUri!.AbsolutePath.Should().Be($"{prefix}/ping");

		await client.Troubleshooting.GetCommandResultAsync(target, "SN1", TroubleshootingCommands.Ping, "task-9", token);
		Api(handler).Method.Should().Be(HttpMethod.Get);
		Api(handler).RequestUri!.AbsolutePath.Should().Be($"{prefix}/ping/async-operations/task-9");
	}

	[Fact]
	public void EveryTarget_HasADistinctRouteSegment()
	{
		var segments = Enum.GetValues<TroubleshootingTarget>()
			.Select(target => typeof(TroubleshootingTarget).GetField(target.ToString())!
				.GetCustomAttributes(typeof(System.Runtime.Serialization.EnumMemberAttribute), false)
				.Cast<System.Runtime.Serialization.EnumMemberAttribute>()
				.Single().Value)
			.ToList();

		segments.Should().OnlyHaveUniqueItems().And.NotContainNulls();
	}

	[Fact]
	public void CommandNames_AreDistinct()
	{
		var names = typeof(TroubleshootingCommands)
			.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
			.Select(property => (string)property.GetValue(null)!)
			.ToList();

		names.Should().OnlyHaveUniqueItems().And.HaveCount(11);
	}
}
