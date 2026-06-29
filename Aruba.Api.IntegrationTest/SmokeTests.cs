using AwesomeAssertions;

namespace Aruba.Api.IntegrationTest;

/// <summary>
/// Live smoke tests against a real New Central tenant. These are skipped automatically when no
/// credentials are configured — see <see cref="TestConfig"/>. This project sets
/// <c>failSkips: false</c> precisely so that absence of credentials does not fail the build.
/// </summary>
public sealed class SmokeTests
{
	[Fact]
	public async Task ListDevices_ReturnsData()
	{
		var options = TestConfig.TryLoadOptions();
		Assert.SkipWhen(options is null, "No Aruba credentials configured; skipping live integration test.");

		using var client = new ArubaCentralClient(options!);
		var page = await client.Devices.GetAllAsync(new Data.ListQuery { Limit = 10 }, TestContext.Current.CancellationToken);

		page.Should().NotBeNull();
		page.Items.Should().NotBeNull();
	}

	[Fact]
	public async Task ListSitesHealth_ReturnsData()
	{
		var options = TestConfig.TryLoadOptions();
		Assert.SkipWhen(options is null, "No Aruba credentials configured; skipping live integration test.");

		using var client = new ArubaCentralClient(options!);
		var page = await client.SiteHealth.GetAllSitesHealthAsync(cancellationToken: TestContext.Current.CancellationToken);

		page.Should().NotBeNull();
	}
}
