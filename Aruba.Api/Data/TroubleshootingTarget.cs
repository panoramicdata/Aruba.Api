using System.Runtime.Serialization;

namespace Aruba.Api.Data;

/// <summary>
/// The kind of device a troubleshooting call is aimed at. Each kind has its own route segment under
/// <c>/network-troubleshooting/v1</c>, and otherwise the same operations.
/// </summary>
public enum TroubleshootingTarget
{
	/// <summary>An access point (<c>aps</c>).</summary>
	[EnumMember(Value = "aps")]
	AccessPoint,

	/// <summary>An AOS-CX switch (<c>cx</c>).</summary>
	[EnumMember(Value = "cx")]
	CxSwitch,

	/// <summary>An AOS-S switch (<c>aos-s</c>).</summary>
	[EnumMember(Value = "aos-s")]
	AosSwitch,

	/// <summary>A gateway (<c>gateways</c>).</summary>
	[EnumMember(Value = "gateways")]
	Gateway,
}
