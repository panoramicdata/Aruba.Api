using Microsoft.Extensions.DependencyInjection;

namespace Aruba.Api;

/// <summary>
/// Dependency-injection helpers for registering an <see cref="ArubaCentralClient"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Registers a singleton <see cref="ArubaCentralClient"/> configured via a builder callback.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="configure">A callback that populates the <see cref="ArubaCentralClientOptionsBuilder"/>.</param>
	public static IServiceCollection AddArubaCentralClient(
		this IServiceCollection services,
		Action<ArubaCentralClientOptionsBuilder> configure)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(configure);

		var builder = new ArubaCentralClientOptionsBuilder();
		configure(builder);
		var options = builder.Build();

		services.AddSingleton(_ => new ArubaCentralClient(options));
		return services;
	}

	/// <summary>
	/// Registers a singleton <see cref="ArubaCentralClient"/> using an already-constructed
	/// <see cref="ArubaCentralClientOptions"/> instance.
	/// </summary>
	public static IServiceCollection AddArubaCentralClient(
		this IServiceCollection services,
		ArubaCentralClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(options);

		services.AddSingleton(_ => new ArubaCentralClient(options));
		return services;
	}
}
