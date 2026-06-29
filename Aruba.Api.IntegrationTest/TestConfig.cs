using Microsoft.Extensions.Configuration;

namespace Aruba.Api.IntegrationTest;

/// <summary>
/// Loads integration-test credentials from a local <c>secrets.json</c> file or user-secrets.
/// Copy <c>secrets.example.json</c> to <c>secrets.json</c> to run locally.
/// </summary>
internal static class TestConfig
{
	public static ArubaCentralClientOptions? TryLoadOptions()
	{
		var configuration = new ConfigurationBuilder()
			.AddJsonFile("secrets.json", optional: true)
			.AddUserSecrets(typeof(TestConfig).Assembly, optional: true)
			.Build();

		var section = configuration.GetSection("Aruba");
		var baseAddress = section["BaseAddress"];
		var clientId = section["ClientId"];
		var clientSecret = section["ClientSecret"];

		if (string.IsNullOrWhiteSpace(baseAddress)
			|| string.IsNullOrWhiteSpace(clientId)
			|| string.IsNullOrWhiteSpace(clientSecret))
		{
			return null;
		}

		return new ArubaCentralClientOptions
		{
			BaseAddress = new Uri(baseAddress),
			ClientId = clientId,
			ClientSecret = clientSecret,
			IsReadOnly = true,
		};
	}
}
