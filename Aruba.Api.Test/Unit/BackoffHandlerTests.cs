using AwesomeAssertions;
using System.Net;
using System.Text;

namespace Aruba.Api.Test.Unit;

/// <summary>
/// Exercises the retry/back-off, header and logging behaviour of
/// <see cref="AuthenticatedBackingOffHandler"/> end-to-end through the client and Refit.
/// </summary>
public sealed class BackoffHandlerTests
{
	private static ArubaCentralClientOptions Options(
		int maxAttempts = 5,
		Microsoft.Extensions.Logging.ILogger? logger = null)
		=> new()
		{
			BaseAddress = ArubaCentralClusters.UsWest,
			ClientId = "id",
			ClientSecret = "secret",
			IsReadOnly = false,
			MaxAttemptCount = maxAttempts,
			// The first retry always waits at least 1s (factor^0 == 1); keep the factor tiny so later
			// retries do not add materially to the test duration.
			BackOffDelayFactor = 0.0001,
			Logger = logger ?? new Microsoft.Extensions.Logging.Abstractions.NullLogger<ArubaCentralClient>(),
		};

	private static HttpResponseMessage StatusJson(HttpStatusCode statusCode, string retryAfter = "")
	{
		var response = new HttpResponseMessage(statusCode)
		{
			Content = new StringContent("{}", Encoding.UTF8, "application/json"),
		};
		if (retryAfter.Length > 0)
		{
			response.Headers.Add("Retry-After", retryAfter);
		}

		return response;
	}

	[Fact]
	public async Task Retries_429_ThenSucceeds()
	{
		var apiCalls = 0;
		var handler = new MockHttpMessageHandler((request, _) =>
		{
			if (MockHttpMessageHandler.IsTokenRequest(request))
			{
				return MockHttpMessageHandler.TokenResponse();
			}

			apiCalls++;
			return apiCalls == 1
				? StatusJson(HttpStatusCode.TooManyRequests, retryAfter: "0")
				: MockHttpMessageHandler.Json(HttpStatusCode.OK, TestData.AccessPointsPage);
		});

		using var client = new ArubaCentralClient(Options(), handler);
		var page = await client.AccessPoints.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		page.Items.Should().ContainSingle();
		apiCalls.Should().Be(2, "the 429 should be retried once");
	}

	[Fact]
	public async Task Retries_503_ThenSucceeds()
	{
		var apiCalls = 0;
		var handler = new MockHttpMessageHandler((request, _) =>
		{
			if (MockHttpMessageHandler.IsTokenRequest(request))
			{
				return MockHttpMessageHandler.TokenResponse();
			}

			apiCalls++;
			return apiCalls == 1
				? StatusJson(HttpStatusCode.ServiceUnavailable)
				: MockHttpMessageHandler.Json(HttpStatusCode.OK, TestData.AccessPointsPage);
		});

		using var client = new ArubaCentralClient(Options(), handler);
		var page = await client.AccessPoints.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		page.Items.Should().ContainSingle();
		apiCalls.Should().Be(2, "a 5xx should be retried");
	}

	[Fact]
	public async Task GivesUp_After_MaxAttempts()
	{
		var apiCalls = 0;
		var handler = new MockHttpMessageHandler((request, _) =>
		{
			if (MockHttpMessageHandler.IsTokenRequest(request))
			{
				return MockHttpMessageHandler.TokenResponse();
			}

			apiCalls++;
			return StatusJson(HttpStatusCode.ServiceUnavailable);
		});

		using var client = new ArubaCentralClient(Options(maxAttempts: 2), handler);

		var ex = await Record.ExceptionAsync(
			() => client.AccessPoints.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken));

		ex.Should().NotBeNull("the request never succeeds");
		apiCalls.Should().Be(2, "it should stop after MaxAttemptCount attempts");
	}

	[Fact]
	public async Task Adds_UserAgent_Header()
	{
		string? userAgent = null;
		var handler = new MockHttpMessageHandler((request, _) =>
		{
			if (MockHttpMessageHandler.IsTokenRequest(request))
			{
				return MockHttpMessageHandler.TokenResponse();
			}

			userAgent = request.Headers.UserAgent.ToString();
			return MockHttpMessageHandler.Json(HttpStatusCode.OK, TestData.AccessPointsPage);
		});

		using var client = new ArubaCentralClient(Options(), handler);
		await client.AccessPoints.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		userAgent.Should().Be("Aruba.Api/1.0");
	}

	[Fact]
	public async Task Debug_Logging_Includes_Request_Body_For_Mutations()
	{
		var logger = new CapturingLogger();
		var handler = new MockHttpMessageHandler((request, _) =>
			MockHttpMessageHandler.IsTokenRequest(request)
				? MockHttpMessageHandler.TokenResponse()
				: MockHttpMessageHandler.Json(HttpStatusCode.OK, """{"id":"AP1","deviceName":"renamed"}"""));

		using var client = new ArubaCentralClient(Options(logger: logger), handler);
		await client.Devices.UpdateAsync(
			"AP1",
			new Data.DeviceUpdateRequest { DeviceName = "renamed" },
			TestContext.Current.CancellationToken);

		logger.AllText.Should().Contain("Request Body");
		logger.AllText.Should().Contain("renamed", "the PATCH body should be logged");
	}

	[Fact]
	public async Task Debug_Logging_Masks_Authorization_Header()
	{
		var logger = new CapturingLogger();
		var handler = new MockHttpMessageHandler((request, _) =>
			MockHttpMessageHandler.IsTokenRequest(request)
				? MockHttpMessageHandler.TokenResponse("super-secret-token")
				: MockHttpMessageHandler.Json(HttpStatusCode.OK, TestData.AccessPointsPage));

		using var client = new ArubaCentralClient(Options(logger: logger), handler);
		await client.AccessPoints.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

		logger.AllText.Should().Contain("***MASKED***");
		logger.AllText.Should().NotContain("super-secret-token", "the bearer token must never be logged");
	}
}
