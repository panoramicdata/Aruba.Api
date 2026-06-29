using AwesomeAssertions;
using Aruba.Api.Data;
using Aruba.Api.Exceptions;
using System.Net;

namespace Aruba.Api.Test.Unit;

/// <summary>
/// Verifies that the client is read-only (safe) by default, and that the read-only gate blocks
/// every mutating HTTP verb before the request is sent.
/// </summary>
public sealed class ReadOnlyDefaultTests
{
	private static ArubaCentralClientOptions DefaultOptions() => new()
	{
		BaseAddress = ArubaCentralClusters.UsWest,
		ClientId = "test-id",
		ClientSecret = "test-secret",
		// NB: IsReadOnly intentionally NOT set — it must default to true.
	};

	[Fact]
	public void Options_IsReadOnly_DefaultsToTrue()
		=> DefaultOptions().IsReadOnly.Should().BeTrue("the client must be safe by default");

	[Fact]
	public void Builder_IsReadOnly_DefaultsToTrue()
	{
		var builder = new ArubaCentralClientOptionsBuilder
		{
			BaseAddress = ArubaCentralClusters.UsWest,
			ClientId = "id",
			ClientSecret = "secret",
		};

		builder.IsReadOnly.Should().BeTrue();
		builder.Build().IsReadOnly.Should().BeTrue();
	}

	[Fact]
	public async Task DefaultClient_BlocksPost()
	{
		using var client = NeverSendsHandlerClient(out var handler);

		await AssertBlockedAsync(
			() => client.Alerts.ClearAsync(new AlertActionRequest { AlertIds = ["1"] }, TestContext.Current.CancellationToken),
			handler);
	}

	[Fact]
	public async Task DefaultClient_BlocksPatch()
	{
		using var client = NeverSendsHandlerClient(out var handler);

		await AssertBlockedAsync(
			() => client.Devices.UpdateAsync("AP1", new DeviceUpdateRequest { DeviceName = "x" }, TestContext.Current.CancellationToken),
			handler);
	}

	[Fact]
	public async Task DefaultClient_BlocksPut()
	{
		using var client = NeverSendsHandlerClient(out var handler);

		await AssertBlockedAsync(
			() => client.Reports.UpdateAsync("r1", new Report(), TestContext.Current.CancellationToken),
			handler);
	}

	[Fact]
	public async Task DefaultClient_BlocksDelete()
	{
		using var client = NeverSendsHandlerClient(out var handler);

		await AssertBlockedAsync(
			() => client.Devices.DeleteAsync("AP1", TestContext.Current.CancellationToken),
			handler);
	}

	[Fact]
	public async Task DefaultClient_AllowsGet()
	{
		using var client = NeverSendsHandlerClient(out _, allowGet: true);

		var page = await client.Devices.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);
		page.Should().NotBeNull();
	}

	/// <summary>
	/// Asserts the call was blocked client-side: an <see cref="ArubaReadOnlyViolationException"/>
	/// appears in the exception chain (Refit wraps it for endpoints that deserialise a response),
	/// and nothing reached the transport.
	/// </summary>
	private static async Task AssertBlockedAsync(Func<Task> act, MockHttpMessageHandler handler)
	{
		var ex = await Record.ExceptionAsync(act);
		ex.Should().NotBeNull("a read-only client must reject mutations");
		ExceptionChain(ex!).Should().ContainItemsAssignableTo<ArubaReadOnlyViolationException>();
		handler.Requests.Should().BeEmpty("a blocked mutation must never reach the transport");
	}

	private static IEnumerable<Exception> ExceptionChain(Exception ex)
	{
		for (var current = ex; current is not null; current = current.InnerException)
		{
			yield return current;
		}
	}

	private static ArubaCentralClient NeverSendsHandlerClient(out MockHttpMessageHandler handler, bool allowGet = false)
	{
		handler = new MockHttpMessageHandler((request, _) =>
		{
			if (MockHttpMessageHandler.IsTokenRequest(request))
			{
				return MockHttpMessageHandler.TokenResponse();
			}

			// For GET-allowed tests, return an empty page; mutations should never get here.
			return allowGet
				? MockHttpMessageHandler.Json(HttpStatusCode.OK, """{"items":[],"count":0}""")
				: MockHttpMessageHandler.Json(HttpStatusCode.InternalServerError, "{}");
		});

		return new ArubaCentralClient(DefaultOptions(), handler);
	}
}
