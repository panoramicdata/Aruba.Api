using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Aruba.Api.Test.Unit;

public sealed class ServiceCollectionExtensionsTests
{
	[Fact]
	public void AddArubaCentralClient_BuilderOverload_RegistersUsableSingleton()
	{
		var services = new ServiceCollection();

		services.AddArubaCentralClient(builder =>
		{
			builder.BaseAddress = ArubaCentralClusters.UsWest;
			builder.ClientId = "id";
			builder.ClientSecret = "secret";
		});

		using var provider = services.BuildServiceProvider();
		var first = provider.GetRequiredService<ArubaCentralClient>();
		var second = provider.GetRequiredService<ArubaCentralClient>();

		first.Should().NotBeNull();
		second.Should().BeSameAs(first, "the client is registered as a singleton");
	}

	[Fact]
	public void AddArubaCentralClient_OptionsOverload_RegistersUsableSingleton()
	{
		var services = new ServiceCollection();
		var options = new ArubaCentralClientOptions
		{
			BaseAddress = ArubaCentralClusters.Europe,
			ClientId = "id",
			ClientSecret = "secret",
		};

		services.AddArubaCentralClient(options);

		using var provider = services.BuildServiceProvider();
		provider.GetRequiredService<ArubaCentralClient>().Should().NotBeNull();
	}

	[Fact]
	public void AddArubaCentralClient_BuilderOverload_ValidatesOnBuild()
	{
		var services = new ServiceCollection();

		// Missing BaseAddress must surface as a build-time failure, not a runtime one.
		var act = () => services.AddArubaCentralClient(builder =>
		{
			builder.ClientId = "id";
			builder.ClientSecret = "secret";
		});

		act.Should().Throw<ArgumentException>().WithParameterName("BaseAddress");
	}

	[Fact]
	public void AddArubaCentralClient_NullServices_Throws()
	{
		IServiceCollection services = null!;
		var options = new ArubaCentralClientOptions
		{
			BaseAddress = ArubaCentralClusters.UsWest,
			ClientId = "id",
			ClientSecret = "secret",
		};

		var builderAct = () => services.AddArubaCentralClient(_ => { });
		var optionsAct = () => services.AddArubaCentralClient(options);

		builderAct.Should().Throw<ArgumentNullException>();
		optionsAct.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void AddArubaCentralClient_NullConfigureOrOptions_Throws()
	{
		var services = new ServiceCollection();

		var nullConfigure = () => services.AddArubaCentralClient((Action<ArubaCentralClientOptionsBuilder>)null!);
		var nullOptions = () => services.AddArubaCentralClient((ArubaCentralClientOptions)null!);

		nullConfigure.Should().Throw<ArgumentNullException>();
		nullOptions.Should().Throw<ArgumentNullException>();
	}
}
