using AwesomeAssertions;
using Aruba.Api.Data;

namespace Aruba.Api.Test.Integration;

/// <summary>
/// Live, read-only integration tests against a real HPE Aruba Networking Central (New Central)
/// tenant. The client is created in read-only mode, so only <c>GET</c> requests are ever issued.
/// </summary>
/// <remarks>
/// These tests <b>do not skip</b>. If credentials are not configured they fail (see
/// <see cref="IntegrationTestConfig"/>) — for API work it is safer to fail loudly than to silently
/// not exercise the live surface. Tagged <c>Category=Integration</c> so they can be selected or
/// excluded with <c>dotnet test --filter Category=Integration</c> when desired.
/// </remarks>
[Trait("Category", "Integration")]
public sealed class ReadOnlyIntegrationTests : IDisposable
{
	private readonly ArubaCentralClient _client = new(IntegrationTestConfig.LoadReadOnlyOptions());

	[Fact]
	public async Task Devices_GetAll_ReturnsPagedResponse()
	{
		var page = await _client.Devices.GetAllAsync(new ListQuery { Limit = 10 }, TestContext.Current.CancellationToken);

		page.Should().NotBeNull();
		page.Items.Should().NotBeNull();
		page.Count.Should().BeGreaterThanOrEqualTo(0);
	}

	[Fact]
	public async Task AccessPoints_GetAll_ReturnsPagedResponse()
	{
		var page = await _client.AccessPoints.GetAllAsync(new ListQuery { Limit = 10 }, TestContext.Current.CancellationToken);

		page.Should().NotBeNull();
		page.Items.Should().NotBeNull();
	}

	[Fact]
	public async Task SiteHealth_GetAllSites_ReturnsPagedResponse()
	{
		var page = await _client.SiteHealth.GetAllSitesHealthAsync(cancellationToken: TestContext.Current.CancellationToken);

		page.Should().NotBeNull();
		page.Items.Should().NotBeNull();
	}

	[Fact]
	public async Task Alerts_GetAll_ReturnsPagedResponse()
	{
		var page = await _client.Alerts.GetAllAsync(new ListQuery { Limit = 10 }, TestContext.Current.CancellationToken);

		page.Should().NotBeNull();
		page.Items.Should().NotBeNull();
	}

	[Fact]
	public async Task ReadOnlyMode_PreventsMutation()
	{
		// Even against the live tenant, a mutating call must be blocked client-side before it is sent.
		var act = async () => await _client.Devices.DeleteAsync("THIS-SHOULD-NEVER-BE-SENT", TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<InvalidOperationException>();
	}

	public void Dispose() => _client.Dispose();
}
