using AwesomeAssertions;

namespace Aruba.Api.Test.Unit;

public sealed class OptionsAndBackoffTests
{
	[Fact]
	public void Validate_Throws_WhenClientIdMissing()
	{
		var options = new ArubaCentralClientOptions
		{
			BaseAddress = ArubaCentralClusters.UsWest,
			ClientId = "",
			ClientSecret = "secret",
		};

		var act = options.Validate;
		act.Should().Throw<ArgumentException>().WithParameterName("ClientId");
	}

	[Fact]
	public void Builder_Build_Throws_WhenBaseAddressMissing()
	{
		var builder = new ArubaCentralClientOptionsBuilder
		{
			ClientId = "id",
			ClientSecret = "secret",
		};

		var act = () => builder.Build();
		act.Should().Throw<ArgumentException>().WithParameterName("BaseAddress");
	}

	[Fact]
	public void Builder_Build_ProducesValidOptions()
	{
		var builder = new ArubaCentralClientOptionsBuilder
		{
			BaseAddress = ArubaCentralClusters.Europe,
			ClientId = "id",
			ClientSecret = "secret",
			IsReadOnly = true,
		};

		var options = builder.Build();

		options.BaseAddress.Should().Be(ArubaCentralClusters.Europe);
		options.IsReadOnly.Should().BeTrue();
		options.TokenEndpoint.Host.Should().Be("sso.common.cloud.hpe.com");
	}

	[Theory]
	[InlineData(1, 1, 1.5, 60, 1)]       // first attempt: max(1.5^0, 1) = 1
	[InlineData(3, 1, 2.0, 60, 4)]       // 2^2 = 4
	[InlineData(2, 10, 1.5, 60, 10)]     // retry-after dominates
	[InlineData(10, 1, 2.0, 30, 30)]     // capped at max
	public void CalculateBackoffDelay_RespectsFactorRetryAfterAndCap(
		int attempt, int retryAfter, double factor, int max, double expectedSeconds)
	{
		var delay = AuthenticatedBackingOffHandler.CalculateBackoffDelay(attempt, retryAfter, factor, max);
		delay.TotalSeconds.Should().BeApproximately(expectedSeconds, 0.001);
	}
}
