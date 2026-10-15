using Aruba.Api.Authentication;
using Aruba.Api.Exceptions;
using AwesomeAssertions;
using System.Net;
using System.Net.Http;

namespace Aruba.Api.Test.Unit;

public sealed class OAuthTokenProviderTests
{
	private static ArubaCentralClientOptions Options(int tolerance) => new()
	{
		BaseAddress = ArubaCentralClusters.UsWest,
		ClientId = "id",
		ClientSecret = "secret",
		TokenExpiryToleranceSeconds = tolerance,
	};

	private static OAuthTokenProvider CreateProvider(
		MockHttpMessageHandler handler,
		ArubaCentralClientOptions? options = null)
		=> new(options ?? Options(30), new HttpClient(handler));

	private static (MockHttpMessageHandler handler, OAuthTokenProvider provider) Setup(
		Func<HttpRequestMessage, int, HttpResponseMessage> responder,
		ArubaCentralClientOptions? options = null)
	{
		var handler = new MockHttpMessageHandler(responder);
		var provider = CreateProvider(handler, options);
		return (handler, provider);
	}

	[Fact]
	public async Task GetAccessToken_Success_ReturnsToken()
	{
		var provider = Setup((_, _) => MockHttpMessageHandler.TokenResponse("abc")).provider;
		using var _ = provider;

		var token = await provider.GetAccessTokenAsync(false, TestContext.Current.CancellationToken);

		token.Should().Be("abc");
	}

	[Fact]
	public async Task GetAccessToken_NonSuccess_ThrowsWithStatusCode()
	{
		var provider = Setup((_, _) =>
			MockHttpMessageHandler.Json(HttpStatusCode.BadRequest, """{"error":"invalid_client"}""")).provider;
		using var _ = provider;

		var act = async () => await provider.GetAccessTokenAsync(false, TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<ArubaAuthenticationException>())
			.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task GetAccessToken_MalformedJson_Throws()
	{
		var provider = Setup((_, _) =>
			MockHttpMessageHandler.Json(HttpStatusCode.OK, "this is not json")).provider;
		using var _ = provider;

		var act = async () => await provider.GetAccessTokenAsync(false, TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<ArubaAuthenticationException>();
	}

	[Fact]
	public async Task GetAccessToken_MissingAccessToken_Throws()
	{
		var provider = Setup((_, _) =>
			MockHttpMessageHandler.Json(HttpStatusCode.OK, """{"token_type":"Bearer","expires_in":3600}""")).provider;
		using var _ = provider;

		var act = async () => await provider.GetAccessTokenAsync(false, TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<ArubaAuthenticationException>();
	}

	[Fact]
	public async Task GetAccessToken_CachesTokenAcrossCalls()
	{
		var requests = 0;
		var (handler, provider) = Setup((_, _) =>
		{
			requests++;
			return MockHttpMessageHandler.TokenResponse();
		});
		using var _ = provider;

		await provider.GetAccessTokenAsync(false, TestContext.Current.CancellationToken);
		await provider.GetAccessTokenAsync(false, TestContext.Current.CancellationToken);

		requests.Should().Be(1);
	}

	[Fact]
	public async Task GetAccessToken_ForceRefresh_AlwaysRequestsNewToken()
	{
		var requests = 0;
		var (handler, provider) = Setup((_, _) =>
		{
			requests++;
			return MockHttpMessageHandler.TokenResponse($"token-{requests}");
		});
		using var _ = provider;

		await provider.GetAccessTokenAsync(false, TestContext.Current.CancellationToken);
		var second = await provider.GetAccessTokenAsync(true, TestContext.Current.CancellationToken);

		requests.Should().Be(2);
		second.Should().Be("token-2");
	}

	[Fact]
	public async Task GetAccessToken_ExpiryTolerance_ForcesEarlyRefresh()
	{
		var requests = 0;
		var (handler, provider) = Setup((_, _) =>
		{
			requests++;
			// expires_in (10s) is smaller than the tolerance (30s), so the token is treated as
			// already expired and the next call must request a fresh one.
			return MockHttpMessageHandler.TokenResponse(expiresIn: 10);
		}, Options(tolerance: 30));
		using var _ = provider;

		await provider.GetAccessTokenAsync(false, TestContext.Current.CancellationToken);
		await provider.GetAccessTokenAsync(false, TestContext.Current.CancellationToken);

		requests.Should().Be(2);
	}

	[Fact]
	public async Task GetAccessToken_ConcurrentCallers_ShareOneRefresh()
	{
		var requests = 0;
		var (handler, provider) = Setup((_, _) =>
		{
			Interlocked.Increment(ref requests);
			return MockHttpMessageHandler.TokenResponse();
		});
		using var _ = provider;

		var tasks = Enumerable.Range(0, 20)
			.Select(_ => provider.GetAccessTokenAsync(false, TestContext.Current.CancellationToken));
		await Task.WhenAll(tasks);

		requests.Should().Be(1, "concurrent callers must share a single in-flight refresh");
	}
}
