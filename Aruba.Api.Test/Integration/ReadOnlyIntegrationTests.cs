using AwesomeAssertions;
using Aruba.Api.Data;

namespace Aruba.Api.Test.Integration;

/// <summary>
/// Live, read-only integration tests against a real HPE Aruba Networking Central (New Central)
/// tenant.
/// </summary>
/// <remarks>
/// <para>
/// <b>Every test here issues only HTTP <c>GET</c> requests.</b> The client is additionally created
/// in read-only mode (<see cref="ArubaCentralClientOptions.IsReadOnly"/> is <see langword="true"/>,
/// which is also the default), so the library blocks any non-<c>GET</c> request before it leaves
/// the process. There is no code path in these tests that creates, updates or deletes anything.
/// </para>
/// <para>
/// These tests <b>do not skip</b>. If credentials are not configured they fail (see
/// <see cref="IntegrationTestConfig"/>). Tagged <c>Category=Integration</c> so they can be selected
/// or excluded with <c>dotnet test --filter Category=Integration</c>.
/// </para>
/// </remarks>
[Trait("Category", "Integration")]
public sealed class ReadOnlyIntegrationTests : IDisposable
{
	private readonly ArubaCentralClient _client = new(IntegrationTestConfig.LoadReadOnlyOptions());

	[Fact]
	public void Client_IsConfiguredReadOnly()
	{
		// Guard: prove the integration client cannot mutate the tenant. No network call is made.
		IntegrationTestConfig.LoadReadOnlyOptions().IsReadOnly.Should().BeTrue();
	}

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

	public void Dispose() => _client.Dispose();
}
