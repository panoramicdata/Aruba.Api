using Aruba.Api.Authentication;
using Aruba.Api.Converters;
using Aruba.Api.Interfaces.Monitoring;
using Aruba.Api.Interfaces.Msp;
using Aruba.Api.Interfaces.Notifications;
using Aruba.Api.Interfaces.Reporting;
using Aruba.Api.Interfaces.Services;
using Aruba.Api.Interfaces.Troubleshooting;
using System.Text.Json;

namespace Aruba.Api;

/// <summary>
/// A strongly-typed client for the HPE Aruba Networking Central (New Central) REST API.
/// </summary>
/// <remarks>
/// The client authenticates using the OAuth 2.0 <c>client_credentials</c> grant against HPE
/// GreenLake SSO, caching and refreshing the access token automatically. Each API area is exposed
/// as a property returning a Refit-backed interface.
/// </remarks>
public sealed class ArubaCentralClient : IDisposable
{
	private readonly HttpClient _httpClient;
	private readonly OAuthTokenProvider _tokenProvider;
	private bool _disposed;

	/// <summary>
	/// Initialises a new <see cref="ArubaCentralClient"/> using the supplied options.
	/// </summary>
	public ArubaCentralClient(ArubaCentralClientOptions options)
		: this(options, new HttpClientHandler())
	{
	}

	/// <summary>
	/// Initialises a new <see cref="ArubaCentralClient"/> with an explicit terminal
	/// <see cref="HttpMessageHandler"/>. Intended for testing, where a mocked handler answers both
	/// the token request and the API requests.
	/// </summary>
	public ArubaCentralClient(ArubaCentralClientOptions options, HttpMessageHandler innerHandler)
	{
		ArgumentNullException.ThrowIfNull(options);
		ArgumentNullException.ThrowIfNull(innerHandler);
		options.Validate();

		_tokenProvider = new OAuthTokenProvider(options, new HttpClient(innerHandler, disposeHandler: false));

		var authHandler = new AuthenticatedBackingOffHandler(options, _tokenProvider)
		{
			InnerHandler = innerHandler,
		};

		_httpClient = new HttpClient(authHandler, disposeHandler: true)
		{
			BaseAddress = options.BaseAddress,
		};
		_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

		var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			PropertyNameCaseInsensitive = true,
		};
		jsonOptions.Converters.Add(new TolerantDateTimeOffsetConverter());

		var refitSettings = new RefitSettings
		{
			ContentSerializer = new SystemTextJsonContentSerializer(jsonOptions),
		};

		// Monitoring
		AccessPoints = RestService.For<IAccessPoints>(_httpClient, refitSettings);
		Devices = RestService.For<IDevices>(_httpClient, refitSettings);
		Clients = RestService.For<IClients>(_httpClient, refitSettings);
		Switches = RestService.For<ISwitches>(_httpClient, refitSettings);
		Gateways = RestService.For<IGateways>(_httpClient, refitSettings);
		SiteHealth = RestService.For<ISiteHealth>(_httpClient, refitSettings);
		Topology = RestService.For<ITopology>(_httpClient, refitSettings);
		ApplicationVisibility = RestService.For<IApplicationVisibility>(_httpClient, refitSettings);
		FirewallSessions = RestService.For<IFirewallSessions>(_httpClient, refitSettings);
		ClientOnboarding = RestService.For<IClientOnboarding>(_httpClient, refitSettings);

		// Notifications
		Alerts = RestService.For<IAlerts>(_httpClient, refitSettings);
		Insights = RestService.For<IInsights>(_httpClient, refitSettings);

		// Reporting
		Reports = RestService.For<IReports>(_httpClient, refitSettings);

		// Services
		Webhooks = RestService.For<IWebhooks>(_httpClient, refitSettings);
		Firmware = RestService.For<IFirmware>(_httpClient, refitSettings);

		// Troubleshooting
		AccessPointTroubleshooting = RestService.For<IAccessPointTroubleshooting>(_httpClient, refitSettings);
		CxSwitchTroubleshooting = RestService.For<ICxSwitchTroubleshooting>(_httpClient, refitSettings);
		AosSwitchTroubleshooting = RestService.For<IAosSwitchTroubleshooting>(_httpClient, refitSettings);
		GatewayTroubleshooting = RestService.For<IGatewayTroubleshooting>(_httpClient, refitSettings);
		Events = RestService.For<IEvents>(_httpClient, refitSettings);

		// MSP
		MspTenants = RestService.For<IMspTenants>(_httpClient, refitSettings);
	}

	/// <summary>Access point monitoring.</summary>
	public IAccessPoints AccessPoints { get; }

	/// <summary>Device monitoring (all device types).</summary>
	public IDevices Devices { get; }

	/// <summary>Client monitoring.</summary>
	public IClients Clients { get; }

	/// <summary>Switch monitoring.</summary>
	public ISwitches Switches { get; }

	/// <summary>Gateway monitoring.</summary>
	public IGateways Gateways { get; }

	/// <summary>Site- and tenant-level health.</summary>
	public ISiteHealth SiteHealth { get; }

	/// <summary>Network topology.</summary>
	public ITopology Topology { get; }

	/// <summary>Application visibility.</summary>
	public IApplicationVisibility ApplicationVisibility { get; }

	/// <summary>Firewall session monitoring.</summary>
	public IFirewallSessions FirewallSessions { get; }

	/// <summary>Client onboarding experience.</summary>
	public IClientOnboarding ClientOnboarding { get; }

	/// <summary>Notification alerts.</summary>
	public IAlerts Alerts { get; }

	/// <summary>Notification insights.</summary>
	public IInsights Insights { get; }

	/// <summary>Reporting.</summary>
	public IReports Reports { get; }

	/// <summary>Webhook management.</summary>
	public IWebhooks Webhooks { get; }

	/// <summary>Firmware information.</summary>
	public IFirmware Firmware { get; }

	/// <summary>Live troubleshooting commands for access points.</summary>
	public IAccessPointTroubleshooting AccessPointTroubleshooting { get; }

	/// <summary>Live troubleshooting commands for AOS-CX switches.</summary>
	public ICxSwitchTroubleshooting CxSwitchTroubleshooting { get; }

	/// <summary>Live troubleshooting commands for AOS-S switches.</summary>
	public IAosSwitchTroubleshooting AosSwitchTroubleshooting { get; }

	/// <summary>Live troubleshooting commands for gateways.</summary>
	public IGatewayTroubleshooting GatewayTroubleshooting { get; }

	/// <summary>Network events.</summary>
	public IEvents Events { get; }

	/// <summary>MSP tenant management.</summary>
	public IMspTenants MspTenants { get; }

	/// <inheritdoc />
	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_httpClient.Dispose();
		_tokenProvider.Dispose();
		_disposed = true;
	}
}
