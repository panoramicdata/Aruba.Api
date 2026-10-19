using Aruba.Api.Data;
using AwesomeAssertions;

namespace Aruba.Api.Test.Unit;

/// <summary>
/// The device types share their common properties through <see cref="DeviceBase"/> and <see cref="ManagedDevice"/>.
/// Moving a property to a base class must not change what is read from the wire.
/// </summary>
public sealed class DeviceBaseTests
{
	private const string Common = """
		"serialNumber": "SN1", "macAddress": "aa:bb", "deviceName": "name", "model": "M1",
		"firmwareVersion": "10.1", "status": "ONLINE", "deviceFunction": "fn", "siteId": "s1",
		"siteName": "site", "id": "id1", "type": "t1"
		""";

	private static void AssertDeviceBase(DeviceBase device)
	{
		device.SerialNumber.Should().Be("SN1");
		device.MacAddress.Should().Be("aa:bb");
		device.DeviceName.Should().Be("name");
		device.Model.Should().Be("M1");
		device.FirmwareVersion.Should().Be("10.1");
		device.Status.Should().Be("ONLINE");
		device.DeviceFunction.Should().Be("fn");
		device.SiteId.Should().Be("s1");
		device.SiteName.Should().Be("site");
		device.Id.Should().Be("id1");
		device.Type.Should().Be("t1");
	}

	[Fact]
	public void Gateway_ReadsTheSharedPropertiesAndItsOwn()
	{
		var gateway = TestSupport.Deserialize<Gateway>($$"""{ {{Common}}, "ipAddress": "10.0.0.1", "mode": "Cluster" }""");

		AssertDeviceBase(gateway);
		gateway.IpAddress.Should().Be("10.0.0.1");
		gateway.Mode.Should().Be("Cluster");
	}

	[Fact]
	public void ManagedDevices_ReadTheSharedPropertiesAndTheirOwn()
	{
		var managed = """
			"partNumber": "P1", "deployment": "Standalone", "ipv4": "10.0.0.2", "ipv6": "::1"
			""";

		var device = TestSupport.Deserialize<Device>($$"""{ {{Common}}, {{managed}}, "deviceType": "SWITCH", "notes": "n" }""");
		var accessPoint = TestSupport.Deserialize<AccessPoint>($$"""{ {{Common}}, {{managed}}, "clientCount": 8 }""");
		var switchModel = TestSupport.Deserialize<Switch>($$"""{ {{Common}}, {{managed}}, "stackId": "st1", "stackMemberId": 2 }""");

		foreach (ManagedDevice item in new ManagedDevice[] { device, accessPoint, switchModel })
		{
			AssertDeviceBase(item);
			item.PartNumber.Should().Be("P1");
			item.Deployment.Should().Be("Standalone");
			item.Ipv4.Should().Be("10.0.0.2");
			item.Ipv6.Should().Be("::1");
		}

		device.DeviceType.Should().Be("SWITCH");
		device.Notes.Should().Be("n");
		accessPoint.ClientCount.Should().Be(8);
		switchModel.StackId.Should().Be("st1");
		switchModel.StackMemberId.Should().Be(2);
	}
}
