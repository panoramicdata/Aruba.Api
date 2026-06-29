using AwesomeAssertions;
using System.Net;
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

	[Fact]
	public async Task GetAllAsync_DeserialisesPagedResponse()
	{
		var handler = new MockHttpMessageHandler((request, _) =>
			MockHttpMessageHandler.IsTokenRequest(request)
				? MockHttpMessageHandler.TokenResponse()
				: MockHttpMessageHandler.Json(HttpStatusCode.OK, TestData.AccessPointsPage));

		using var client = new ArubaCentralClient(Options(), handler);

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
			return MockHttpMessageHandler.Json(HttpStatusCode.OK, TestData.AccessPointsPage);
		});

		using var client = new ArubaCentralClient(Options(), handler);
		await client.AccessPoints.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		auth.Should().NotBeNull();
		auth!.Scheme.Should().Be("Bearer");
		auth.Parameter.Should().Be("the-access-token");
	}

	[Fact]
	public async Task Token_IsCachedAcrossCalls()
	{
		var tokenRequests = 0;
		var handler = new MockHttpMessageHandler((request, _) =>
		{
			if (MockHttpMessageHandler.IsTokenRequest(request))
			{
				tokenRequests++;
				return MockHttpMessageHandler.TokenResponse();
			}

			return MockHttpMessageHandler.Json(HttpStatusCode.OK, TestData.AccessPointsPage);
		});

		using var client = new ArubaCentralClient(Options(), handler);
		await client.AccessPoints.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);
		await client.Devices.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		tokenRequests.Should().Be(1);
	}

	[Fact]
	public async Task Unauthorized_RefreshesTokenAndRetries()
	{
		var tokenRequests = 0;
		var apiCalls = 0;
		var handler = new MockHttpMessageHandler((request, _) =>
		{
			if (MockHttpMessageHandler.IsTokenRequest(request))
			{
				tokenRequests++;
				return MockHttpMessageHandler.TokenResponse($"token-{tokenRequests}");
			}

			apiCalls++;
			return apiCalls == 1
				? MockHttpMessageHandler.Json(HttpStatusCode.Unauthorized, "{}")
				: MockHttpMessageHandler.Json(HttpStatusCode.OK, TestData.AccessPointsPage);
		});

		using var client = new ArubaCentralClient(Options(), handler);
		var page = await client.AccessPoints.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		page.Items.Should().ContainSingle();
		tokenRequests.Should().Be(2, "the token should be refreshed once after a 401");
		apiCalls.Should().Be(2);
	}

	[Fact]
	public async Task ReadOnlyMode_BlocksMutatingRequests()
	{
		var handler = new MockHttpMessageHandler((request, _) =>
			MockHttpMessageHandler.IsTokenRequest(request)
				? MockHttpMessageHandler.TokenResponse()
				: MockHttpMessageHandler.Json(HttpStatusCode.OK, "{}"));

		using var client = new ArubaCentralClient(Options(readOnly: true), handler);

		var act = async () => await client.Devices.DeleteAsync("AP00000001", TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task Alerts_EmptyUpdatedAt_DeserialisesAsNull()
	{
		var handler = new MockHttpMessageHandler((request, _) =>
			MockHttpMessageHandler.IsTokenRequest(request)
				? MockHttpMessageHandler.TokenResponse()
				: MockHttpMessageHandler.Json(HttpStatusCode.OK, TestData.AlertsPage));

		using var client = new ArubaCentralClient(Options(), handler);
		var page = await client.Alerts.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		var alert = page.Items.Should().ContainSingle().Subject;
		alert.UpdatedAt.Should().BeNull();
		alert.CreatedAt.Should().NotBeNull();
		alert.Severity.Should().Be("Critical");
	}

	[Fact]
	public async Task AlertAction_ReturnsAsyncAcknowledgement()
	{
		var handler = new MockHttpMessageHandler((request, _) =>
			MockHttpMessageHandler.IsTokenRequest(request)
				? MockHttpMessageHandler.TokenResponse()
				: MockHttpMessageHandler.Json(HttpStatusCode.OK, TestData.AsyncAccepted));

		using var client = new ArubaCentralClient(Options(), handler);
		var result = await client.Alerts.ClearAsync(
			new Data.AlertActionRequest { AlertIds = ["1", "2"] },
			TestContext.Current.CancellationToken);

		result.TaskId.Should().Be("abc123-def456-ghi789");
		result.Response.Should().Be("Request accepted");
	}
}
