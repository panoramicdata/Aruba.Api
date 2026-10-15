using AwesomeAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Aruba.Api.Test.Unit;

public sealed class ArubaCentralClientTests
{
	private static ArubaCentralClientOptions Options(bool readOnly = false) => new()
	{
		BaseAddress = ArubaCentralClusters.UsWest,
		ClientId = "test-id",
		ClientSecret = "test-secret",
		IsReadOnly = readOnly,
	};

	private static ArubaCentralClient CreateClient(MockHttpMessageHandler handler, bool readOnly = false)
		=> new(Options(readOnly), handler);

	[Fact]
	public void AllApiFacades_AreWiredUp()
	{
		using var client = CreateClient(CreateTokenOnlyHandler());

		object?[] facades =
		[
			client.AccessPoints, client.Devices, client.Clients, client.Switches, client.Gateways,
			client.SiteHealth, client.Topology, client.ApplicationVisibility, client.FirewallSessions,
			client.ClientOnboarding, client.Alerts, client.Insights, client.Reports, client.Webhooks,
			client.Firmware, client.AccessPointTroubleshooting, client.CxSwitchTroubleshooting,
			client.AosSwitchTroubleshooting, client.GatewayTroubleshooting, client.Events, client.MspTenants,
		];

		facades.Should().OnlyContain(f => f != null);
	}

	[Fact]
	public void Constructor_WithOptionsOnly_CreatesUsableClient_AndDisposeIsIdempotent()
	{
		var client = new ArubaCentralClient(Options());

		client.AccessPoints.Should().NotBeNull();

		// Dispose must be safe to call more than once.
		client.Dispose();
		var secondDispose = client.Dispose;
		secondDispose.Should().NotThrow();
	}

	[Fact]
	public async Task GetAllAsync_DeserialisesPagedResponse()
	{
		using var client = CreateClient(CreateJsonHandler(TestData.AccessPointsPage));

		var page = await client.AccessPoints.GetAllAsync(new Data.ListQuery { Limit = 100 }, TestContext.Current.CancellationToken);

		page.Count.Should().Be(1);
		page.Total.Should().Be(4);
		page.Next.Should().Be("2");
		page.Items.Should().ContainSingle();
		var ap = page.Items[0];
		ap.SerialNumber.Should().Be("AP00000001");
		ap.ClientCount.Should().Be(8);
		ap.PowerConsumption.Should().BeApproximately(11.891, 0.0001);
	}

	[Fact]
	public async Task GetAllAsync_AddsBearerTokenFromTokenEndpoint()
	{
		AuthenticationHeaderValue? auth = null;
		var handler = new MockHttpMessageHandler((request, _) =>
		{
			if (MockHttpMessageHandler.IsTokenRequest(request))
			{
				return MockHttpMessageHandler.TokenResponse("the-access-token");
			}

			auth = request.Headers.Authorization;
			return JsonResponse(TestData.AccessPointsPage);
		});

		using var client = CreateClient(handler);
		await client.AccessPoints.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		auth.Should().NotBeNull();
		auth!.Scheme.Should().Be("Bearer");
		auth.Parameter.Should().Be("the-access-token");
	}

	[Fact]
	public async Task Token_IsCachedAcrossCalls()
	{
		var tokenRequests = 0;
		var handler = CreateTokenCountingHandler((() => tokenRequests, (int v) => tokenRequests = v));

		using var client = CreateClient(handler);
		await client.AccessPoints.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);
		await client.Devices.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		tokenRequests.Should().Be(1);
	}

	[Fact]
	public async Task Unauthorized_RefreshesTokenAndRetries()
	{
		var tokenRequests = 0;
		var apiCalls = 0;
		var handler = CreateUnauthorizedHandler((() => tokenRequests, (int v) => tokenRequests = v), (() => apiCalls, (int v) => apiCalls = v));

		using var client = CreateClient(handler);
		var page = await client.AccessPoints.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		page.Items.Should().ContainSingle();
		tokenRequests.Should().Be(2, "the token should be refreshed once after a 401");
		apiCalls.Should().Be(2);
	}

	[Fact]
	public async Task ReadOnlyMode_BlocksMutatingRequests()
	{
		using var client = CreateClient(CreateTokenOnlyHandler(), readOnly: true);

		var act = async () => await client.Devices.DeleteAsync("AP00000001", TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task Alerts_EmptyUpdatedAt_DeserialisesAsNull()
	{
		using var client = CreateClient(CreateJsonHandler(TestData.AlertsPage));

		var page = await client.Alerts.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		var alert = page.Items.Should().ContainSingle().Subject;
		alert.UpdatedAt.Should().BeNull();
		alert.CreatedAt.Should().NotBeNull();
		alert.Severity.Should().Be("Critical");
	}

	[Fact]
	public async Task AlertAction_ReturnsAsyncAcknowledgement()
	{
		using var client = CreateClient(CreateJsonHandler(TestData.AsyncAccepted));

		var result = await client.Alerts.ClearAsync(
			new Data.AlertActionRequest { AlertIds = ["1", "2"] },
			TestContext.Current.CancellationToken);

		result.TaskId.Should().Be("abc123-def456-ghi789");
		result.Response.Should().Be("Request accepted");
	}

	private static HttpResponseMessage JsonResponse(string body)
		=> MockHttpMessageHandler.Json(HttpStatusCode.OK, body);

	private static HttpResponseMessage JsonResponse(HttpStatusCode statusCode, string body)
		=> MockHttpMessageHandler.Json(statusCode, body);

	private static HttpResponseMessage TokenResponse(string accessToken = "test-token")
		=> MockHttpMessageHandler.TokenResponse(accessToken);

	private static MockHttpMessageHandler CreateTokenOnlyHandler()
		=> new((request, _) =>
			MockHttpMessageHandler.IsTokenRequest(request)
				? TokenResponse()
				: JsonResponse("{}"));

	private static MockHttpMessageHandler CreateJsonHandler(string body)
		=> new((request, _) =>
			MockHttpMessageHandler.IsTokenRequest(request)
				? TokenResponse()
				: JsonResponse(body));

	private static MockHttpMessageHandler CreateTokenCountingHandler((Func<int>, Action<int>) tokenRequests)
		=> new((request, _) =>
		{
			if (MockHttpMessageHandler.IsTokenRequest(request))
			{
				tokenRequests.Item2(tokenRequests.Item1() + 1);
				return TokenResponse();
			}

			return JsonResponse(TestData.AccessPointsPage);
		});

	private static MockHttpMessageHandler CreateUnauthorizedHandler((Func<int>, Action<int>) tokenRequests, (Func<int>, Action<int>) apiCalls)
		=> new((request, _) =>
		{
			if (MockHttpMessageHandler.IsTokenRequest(request))
			{
				var nextToken = tokenRequests.Item1() + 1;
				tokenRequests.Item2(nextToken);
				return TokenResponse($"token-{nextToken}");
			}

			var nextApiCall = apiCalls.Item1() + 1;
			apiCalls.Item2(nextApiCall);
			return nextApiCall == 1
				? JsonResponse(HttpStatusCode.Unauthorized, "{}")
				: JsonResponse(TestData.AccessPointsPage);
		});
}
