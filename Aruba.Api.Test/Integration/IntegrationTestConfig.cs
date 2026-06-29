using Microsoft.Extensions.Configuration;

namespace Aruba.Api.Test.Integration;

/// <summary>
/// Loads the credentials used by the read-only live integration tests.
/// </summary>
/// <remarks>
/// <para>
/// Credentials are read from .NET user-secrets (for local development) and environment variables
/// (for CI), with environment variables taking precedence. Configure them with either:
/// </para>
/// <code>
/// dotnet user-secrets set "Aruba:BaseAddress"  "https://us2.api.central.arubanetworks.com"
/// dotnet user-secrets set "Aruba:ClientId"     "..."
/// dotnet user-secrets set "Aruba:ClientSecret" "..."
/// </code>
/// <para>
/// or environment variables <c>Aruba__BaseAddress</c>, <c>Aruba__ClientId</c>,
/// <c>Aruba__ClientSecret</c>. See <c>usersecrets.example.json</c> for the expected shape.
/// </para>
/// <para>
/// By design these tests <b>fail hard</b> (rather than skip) when credentials are missing: a
/// silently-skipped integration test gives a false sense of security. The returned options always
/// have <see cref="ArubaCentralClientOptions.IsReadOnly"/> set, so the tests can never mutate the
/// tenant.
/// </para>
/// </remarks>
internal static class IntegrationTestConfig
{
	private static readonly Lazy<IConfigurationRoot> Configuration = new(() =>
		new ConfigurationBuilder()
			.AddUserSecrets(typeof(IntegrationTestConfig).Assembly, optional: true)
			.AddEnvironmentVariables()
			.Build());

	/// <summary>
	/// Builds read-only client options from the configured credentials.
	/// </summary>
	/// <exception cref="InvalidOperationException">
	/// Thrown when any credential is missing — integration tests must fail, not skip.
	/// </exception>
	public static ArubaCentralClientOptions LoadReadOnlyOptions()
	{
		var section = Configuration.Value.GetSection("Aruba");
		var baseAddress = section["BaseAddress"];
		var clientId = section["ClientId"];
		var clientSecret = section["ClientSecret"];

		var missing = new List<string>();
		if (string.IsNullOrWhiteSpace(baseAddress))
		{
			missing.Add("Aruba:BaseAddress");
		}

		if (string.IsNullOrWhiteSpace(clientId))
		{
			missing.Add("Aruba:ClientId");
		}

		if (string.IsNullOrWhiteSpace(clientSecret))
		{
			missing.Add("Aruba:ClientSecret");
		}

		if (missing.Count > 0)
		{
			throw new InvalidOperationException(
				$"Missing integration-test credentials: {string.Join(", ", missing)}. " +
				"Set them via `dotnet user-secrets set` (see usersecrets.example.json) or environment " +
				"variables (Aruba__BaseAddress, Aruba__ClientId, Aruba__ClientSecret). These tests fail " +
				"deliberately rather than skip.");
		}

		return new ArubaCentralClientOptions
		{
			BaseAddress = new Uri(baseAddress!),
			ClientId = clientId!,
			ClientSecret = clientSecret!,
			IsReadOnly = true,
		};
	}
}
