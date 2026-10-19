namespace Aruba.Api.Data;

/// <summary>
/// The names of the asynchronous commands that can be started with
/// <see cref="Interfaces.Troubleshooting.ITroubleshooting.CommandAsync"/>, and the same names to poll for the
/// result with <see cref="Interfaces.Troubleshooting.ITroubleshooting.GetCommandResultAsync"/>.
/// </summary>
/// <remarks>
/// Not every command exists for every <see cref="TroubleshootingTarget"/>; the service answers 404 for one that
/// does not. The groups below say where each applies.
/// </remarks>
public static class TroubleshootingCommands
{
	/// <summary>ICMP ping. All device kinds.</summary>
	public static string Ping => "ping";

	/// <summary>Traceroute. All device kinds.</summary>
	public static string Traceroute => "traceroute";

	/// <summary>Speed test. Access points.</summary>
	public static string Speedtest => "speedtest";

	/// <summary>DNS lookup. Access points.</summary>
	public static string NsLookup => "nslookup";

	/// <summary>Disconnects a wireless client by MAC address. Access points.</summary>
	public static string DisconnectUserByMacAddress => "disconnectUserByMacAddress";

	/// <summary>Bounces PoE on a port. Switches.</summary>
	public static string PoeBounce => "poeBounce";

	/// <summary>Bounces a port. Switches.</summary>
	public static string PortBounce => "portBounce";

	/// <summary>Runs a cable test on a port. Switches.</summary>
	public static string CableTest => "cableTest";

	/// <summary>Pings every address in a range. Gateways.</summary>
	public static string PingSweep => "pingSweep";

	/// <summary>Runs an iperf throughput test. Gateways.</summary>
	public static string Iperf => "iperf";

	/// <summary>Disconnects a client by MAC address. Gateways.</summary>
	public static string DisconnectClientByMacAddress => "disconnectClientByMacAddress";
}
