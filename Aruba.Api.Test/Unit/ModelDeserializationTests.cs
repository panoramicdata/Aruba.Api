using Aruba.Api.Data;
using AwesomeAssertions;

namespace Aruba.Api.Test.Unit;

/// <summary>
/// Deserializes each <see cref="Aruba.Api.Data"/> model from a representative payload (using the same
/// serializer settings the client applies) and asserts key fields. Fixtures include every property so
/// that the whole model is exercised, which also guards the shapes as new endpoints are typed.
/// </summary>
public sealed class ModelDeserializationTests
{
	[Fact]
	public void AccessPoint_Deserializes()
	{
		const string json = """
		{
		  "serialNumber": "AP1", "macAddress": "aa:bb", "deviceName": "ap-1", "model": "AP-115",
		  "partNumber": "R1234A", "deployment": "Standalone", "role": "Conductor",
		  "deviceFunction": "Instant", "firmwareVersion": "10.4", "ipv4": "10.0.0.1",
		  "ipv6": "::1", "publicIpv4": "1.2.3.4", "status": "ONLINE", "lastSeenAt": "2025-01-01T00:00:00Z",
		  "siteId": "s1", "siteName": "Site 1", "clusterName": "c1", "clusterId": "cid1",
		  "lastRebootReason": "upgrade", "deviceGroupId": "g1", "deviceGroupName": "grp",
		  "cpuUtilization": 25.5, "memoryUtilization": 11.0, "powerConsumption": 11.891,
		  "clientCount": 8, "buildingId": "b1", "floorId": "f1", "id": "AP1",
		  "type": "network-monitoring/access-point-monitoring"
		}
		""";

		var ap = TestSupport.Deserialize<AccessPoint>(json);

		ap.SerialNumber.Should().Be("AP1");
		ap.ClientCount.Should().Be(8);
		ap.PowerConsumption.Should().BeApproximately(11.891, 1e-4);
		ap.LastSeenAt.Should().Be(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero));
	}

	[Fact]
	public void Device_Deserializes()
	{
		const string json = """
		{
		  "serialNumber": "D1", "macAddress": "aa:bb", "deviceName": "dev", "deviceType": "SWITCH",
		  "model": "6300", "partNumber": "JL658A", "deployment": "Standalone", "role": "member",
		  "deviceFunction": "switch", "firmwareVersion": "10.10", "ipv4": "10.0.0.2", "ipv6": "::2",
		  "status": "ONLINE", "uptimeInMillis": 123456789, "lastSeenAt": "2025-01-02T03:04:05Z",
		  "siteId": "s1", "siteName": "Site 1", "buildingId": "b1", "floorId": "f1",
		  "clusterName": "c1", "configStatus": "IN_SYNC", "configLastModifiedAt": "2025-01-01T00:00:00Z",
		  "notes": "note", "id": "D1", "type": "network-monitoring/device"
		}
		""";

		var device = TestSupport.Deserialize<Device>(json);

		device.DeviceType.Should().Be("SWITCH");
		device.UptimeInMillis.Should().Be(123456789);
	}

	[Fact]
	public void Switch_WithTrends_Deserializes()
	{
		const string json = """
		{
		  "serialNumber": "SW1", "macAddress": "aa:bb", "deviceName": "sw", "model": "6300",
		  "partNumber": "JL658A", "firmwareVersion": "10.10", "ipv4": "10.0.0.3", "ipv6": "::3",
		  "publicIp": "1.2.3.4", "status": "ONLINE", "switchRole": "Conductor", "deployment": "Stack",
		  "deviceFunction": "switch", "switchType": "cx", "uptimeInMillis": 999, "lastSeenAt": "2025-01-01T00:00:00Z",
		  "siteId": "s1", "siteName": "Site 1", "stackId": "st1", "stackMemberId": 2,
		  "switchTrends": [
		    { "cpuUtilization": 10.0, "memoryUtilization": 20.0, "poeAvailable": 100.0, "poeConsumption": 30.0,
		      "powerConsumption": 40.0, "serial": "SW1", "switchRole": "Conductor", "systemTemperature": 35.5,
		      "totalPowerConsumption": 50.0, "upLinkPorts": "1/8", "usage": 1234.0 }
		  ],
		  "id": "SW1", "type": "network-monitoring/switch"
		}
		""";

		var sw = TestSupport.Deserialize<Switch>(json);

		sw.StackMemberId.Should().Be(2);
		sw.SwitchTrends.Should().ContainSingle();
		sw.SwitchTrends[0].SystemTemperature.Should().BeApproximately(35.5, 1e-4);
	}

	[Fact]
	public void Gateway_Deserializes()
	{
		const string json = """
		{
		  "serialNumber": "GW1", "macAddress": "aa:bb", "deviceName": "gw", "cpuUtilization": 5.0,
		  "memoryUtilization": 6.0, "siteId": "s1", "siteName": "Site 1", "firmwareVersion": "10.4",
		  "ipAddress": "10.0.0.4", "uptimeInMillis": 42, "macRange": "aa:bb-aa:cc", "rebootReason": "power",
		  "deviceFunction": "VPNC", "mode": "Cluster", "status": "ONLINE", "role": "Isoleader",
		  "model": "9004", "clusterName": "c1", "id": "GW1", "type": "network-monitoring/gateway"
		}
		""";

		var gw = TestSupport.Deserialize<Gateway>(json);

		gw.DeviceFunction.Should().Be("VPNC");
		gw.Mode.Should().Be("Cluster");
	}

	[Fact]
	public void Client_Deserializes()
	{
		const string json = """
		{
		  "macAddress": "aa:bb", "id": "C1", "type": "client", "clientName": "laptop", "status": "Connected",
		  "connectedDeviceType": "AP", "clientConnectionType": "Wireless", "ipv4": "10.0.0.5", "ipv6": "::5",
		  "connectedDeviceSerial": "AP1", "connectedTo": "aa:cc", "lastSeenAt": "2025-01-01T00:00:00Z",
		  "wlanName": "corp", "port": "1/1", "role": "employee", "vlanId": "10", "vlanName": "data",
		  "connectedAt": "2025-01-01T00:00:00Z", "userName": "alice", "hostName": "alice-pc",
		  "wirelessSecurity": "WPA2-Enterprise", "clientManufacturer": "Dell", "clientFunction": "Mobile",
		  "clientOperatingSystem": "Windows", "siteId": "s1", "siteName": "Site 1", "wirelessBand": "5GHZ",
		  "wirelessChannel": "36", "bssid": "aa:dd", "radioMacAddress": "aa:ee", "authenticationType": "Captive Portal"
		}
		""";

		var client = TestSupport.Deserialize<Client>(json);

		client.ClientName.Should().Be("laptop");
		client.WirelessBand.Should().Be("5GHZ");
	}

	[Fact]
	public void Alert_Deserializes()
	{
		const string json = """
		{
		  "id": "a1", "type": "alert", "name": "PoE", "summary": "low power", "status": "Cleared",
		  "category": "System", "key": "poe.insufficient", "severity": "Critical", "priority": "Very High",
		  "deviceType": "Access Point", "clearedReason": "resolved", "updatedBy": "system",
		  "createdAt": "2025-01-01T00:00:00Z", "updatedAt": ""
		}
		""";

		var alert = TestSupport.Deserialize<Alert>(json);

		alert.Severity.Should().Be("Critical");
		alert.CreatedAt.Should().NotBeNull();
		alert.UpdatedAt.Should().BeNull("an empty timestamp string reads as null");
	}

	[Fact]
	public void AlertClassification_Deserializes()
	{
		const string json = """
		{
		  "type": "category",
		  "classificationData": [ { "name": "System", "count": 5, "typeId": "sys" } ]
		}
		""";

		var classification = TestSupport.Deserialize<AlertClassification>(json);

		classification.Type.Should().Be("category");
		classification.ClassificationData.Should().ContainSingle();
		classification.ClassificationData[0].Count.Should().Be(5);
		classification.ClassificationData[0].TypeId.Should().Be("sys");
	}

	[Fact]
	public void Insight_Deserializes()
	{
		const string json = """
		{ "id": "i1", "type": "insight", "name": "Coverage", "category": "RF", "severity": "Minor",
		  "description": "weak signal", "createdAt": "2025-01-01T00:00:00Z" }
		""";

		var insight = TestSupport.Deserialize<Insight>(json);

		insight.Category.Should().Be("RF");
		insight.Description.Should().Be("weak signal");
	}

	[Fact]
	public void NetworkEvent_Deserializes()
	{
		const string json = """
		{ "id": "e1", "type": "event", "name": "Reboot", "category": "System", "level": "info",
		  "description": "device rebooted", "serialNumber": "AP1", "siteId": "s1",
		  "occurredAt": "2025-01-01T00:00:00Z" }
		""";

		var networkEvent = TestSupport.Deserialize<NetworkEvent>(json);

		networkEvent.Level.Should().Be("info");
		networkEvent.SerialNumber.Should().Be("AP1");
	}

	[Fact]
	public void FirmwareDetails_Deserializes()
	{
		const string json = """
		{ "serialNumber": "AP1", "deviceType": "AP", "currentVersion": "10.3",
		  "recommendedVersion": "10.4", "upgradeAvailable": true }
		""";

		var firmware = TestSupport.Deserialize<FirmwareDetails>(json);

		firmware.UpgradeAvailable.Should().BeTrue();
		firmware.RecommendedVersion.Should().Be("10.4");
	}

	[Fact]
	public void HealthCounts_Deserializes()
	{
		const string json = """{ "total": 10, "good": 7, "fair": 2, "poor": 1 }""";

		var health = TestSupport.Deserialize<HealthCounts>(json);

		health.Total.Should().Be(10);
		health.Good.Should().Be(7);
		health.Poor.Should().Be(1);
	}

	[Fact]
	public void MspTenant_Deserializes()
	{
		const string json = """
		{
		  "tenantName": "Acme", "tenantId": "t1", "totalSites": 5, "degradedSites": 1,
		  "deviceOwnership": "MSP",
		  "deviceHealthStatus": { "total": 10, "good": 8, "fair": 1, "poor": 1 },
		  "alerts": { "total": 4, "critical": 1, "major": 2, "minor": 1 },
		  "lastUpdatedTime": 1609459200, "createdTime": 1600000000
		}
		""";

		var tenant = TestSupport.Deserialize<MspTenant>(json);

		tenant.TenantName.Should().Be("Acme");
		tenant.DeviceHealthStatus!.Good.Should().Be(8);
		tenant.Alerts!.Critical.Should().Be(1);
		tenant.LastUpdatedTime.Should().Be(1609459200);
	}

	[Fact]
	public void Report_Deserializes()
	{
		const string json = """
		{
		  "id": "r1", "type": "report", "name": "Weekly", "reportScope": { "scopeType": "TENANT" },
		  "reportType": "NETWORK", "createdBy": "alice", "createdAt": "2025-01-01T00:00:00Z",
		  "lastModifiedBy": "bob", "lastModifiedAt": "2025-01-02T00:00:00Z",
		  "reportSchedule": {
		    "scheduleType": "EVERY_YEAR", "lastRunTs": "2025-01-01T00:00:00Z", "lastRunStatus": "ACTIVE",
		    "nextRunTs": "2026-01-01T00:00:00Z", "completedRuns": 3, "totalRuns": 10,
		    "scheduleEnd": "2027-01-01T00:00:00Z"
		  }
		}
		""";

		var report = TestSupport.Deserialize<Report>(json);

		report.ReportScope!.ScopeType.Should().Be("TENANT");
		report.ReportSchedule!.ScheduleType.Should().Be("EVERY_YEAR");
		report.ReportSchedule.CompletedRuns.Should().Be(3);
	}

	[Fact]
	public void ReportRun_Deserializes()
	{
		const string json = """
		{ "id": "run1", "status": "COMPLETED", "startedAt": "2025-01-01T00:00:00Z",
		  "completedAt": "2025-01-01T01:00:00Z" }
		""";

		var run = TestSupport.Deserialize<ReportRun>(json);

		run.Status.Should().Be("COMPLETED");
		run.CompletedAt.Should().NotBeNull();
	}

	[Fact]
	public void SiteHealth_Deserializes()
	{
		const string json = """
		{
		  "id": "s1", "type": "site-health", "siteName": "Site 1",
		  "address": { "country": "GB", "address": "1 High St", "city": "London", "zipCode": "SW1", "state": "England" },
		  "alerts": { "totalCount": 3, "groups": [ { "name": "Critical", "count": 2 } ] },
		  "reasons": [ { "health": "Poor", "reason": "CLIENT_HEALTH_POOR", "data": { "name": "clients", "count": 5 } } ],
		  "health": { "groups": [ { "name": "Good", "value": 80.0 } ] },
		  "devices": { "count": 12, "health": { "groups": [ { "name": "Good", "value": 90.0 } ] } }
		}
		""";

		var siteHealth = TestSupport.Deserialize<SiteHealth>(json);

		siteHealth.Address!.City.Should().Be("London");
		siteHealth.Alerts!.TotalCount.Should().Be(3);
		siteHealth.Alerts.Groups[0].Count.Should().Be(2);
		siteHealth.Reasons.Should().ContainSingle();
		siteHealth.Reasons[0].Data!.Count.Should().Be(5);
		siteHealth.Health!.Groups[0].Value.Should().BeApproximately(80.0, 1e-4);
		siteHealth.Devices!.Count.Should().Be(12);
		siteHealth.Devices.Health!.Groups[0].Name.Should().Be("Good");
	}

	[Fact]
	public void TopNClient_Deserializes()
	{
		const string json = """
		{ "clientName": "laptop", "macAddress": "aa:bb", "usage": 1048576, "clientConnectionType": "Wireless",
		  "type": "top-n-client", "id": "C1", "siteName": "Site 1", "siteId": "s1" }
		""";

		var client = TestSupport.Deserialize<TopNClient>(json);

		client.Usage.Should().Be(1048576);
		client.ClientConnectionType.Should().Be("Wireless");
	}

	[Fact]
	public void TrendResponse_Deserializes()
	{
		const string json = """
		{
		  "interval": "5 mins", "keys": [ "rx", "tx" ],
		  "samples": [ { "data": [ 1.5, 2.5 ], "ts": "2025-01-01T00:00:00Z" } ],
		  "id": "t1", "type": "trend"
		}
		""";

		var trend = TestSupport.Deserialize<TrendResponse>(json);

		trend.Interval.Should().Be("5 mins");
		trend.Keys.Should().Equal("rx", "tx");
		trend.Samples.Should().ContainSingle();
		trend.Samples[0].Data.Should().Equal(1.5, 2.5);
		trend.Samples[0].Ts.Should().Be(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero));
	}

	[Fact]
	public void TroubleshootingCommandRequest_Deserializes()
	{
		const string json = """
		{ "host": "8.8.8.8", "count": 4, "packetSize": 64, "port": 443, "interface": "1/1/1",
		  "vlanId": "10", "arguments": { "flag": "on" } }
		""";

		var request = TestSupport.Deserialize<TroubleshootingCommandRequest>(json);

		request.Host.Should().Be("8.8.8.8");
		request.Count.Should().Be(4);
		request.Arguments.Should().ContainKey("flag");
	}

	[Fact]
	public void ShowCommandsRequest_Deserializes()
	{
		const string json = """{ "commands": [ "show version", "show clock" ] }""";

		var request = TestSupport.Deserialize<ShowCommandsRequest>(json);

		request.Commands.Should().Equal("show version", "show clock");
	}

	[Fact]
	public void TroubleshootingTask_Deserializes()
	{
		const string json = """
		{ "taskId": "task1", "status": "COMPLETED", "output": "pong",
		  "createdAt": "2025-01-01T00:00:00Z", "completedAt": "2025-01-01T00:00:05Z" }
		""";

		var task = TestSupport.Deserialize<TroubleshootingTask>(json);

		task.Status.Should().Be("COMPLETED");
		task.Output.Should().Be("pong");
	}

	[Fact]
	public void TroubleshootingTaskSummary_Deserializes()
	{
		const string json = """
		{ "taskId": "task1", "command": "ping", "status": "RUNNING", "createdAt": "2025-01-01T00:00:00Z" }
		""";

		var summary = TestSupport.Deserialize<TroubleshootingTaskSummary>(json);

		summary.Command.Should().Be("ping");
		summary.Status.Should().Be("RUNNING");
	}

	[Fact]
	public void Webhook_Deserializes()
	{
		const string json = """
		{ "id": "w1", "name": "hook", "type": "webhook", "endpoint": "https://example.test/hook",
		  "authMechanism": "API_KEY", "generation": 2, "hmacKey": "secret",
		  "createdAt": "2025-01-01T00:00:00Z", "updatedAt": "2025-01-02T00:00:00Z" }
		""";

		var webhook = TestSupport.Deserialize<Webhook>(json);

		webhook.Endpoint.Should().Be("https://example.test/hook");
		webhook.Generation.Should().Be(2);
	}

	[Fact]
	public void WebhookRequest_Deserializes()
	{
		const string json = """
		{ "input": { "name": "hook", "endpoint": "https://example.test/hook",
		  "authMechanism": "API_KEY", "apiKey": "k1" } }
		""";

		var request = TestSupport.Deserialize<WebhookRequest>(json);

		request.Input.Name.Should().Be("hook");
		request.Input.ApiKey.Should().Be("k1");
	}

	[Fact]
	public void DeviceUpdateRequest_Deserializes()
	{
		const string json = """{ "deviceName": "new-name", "notes": "some notes" }""";

		var request = TestSupport.Deserialize<DeviceUpdateRequest>(json);

		request.DeviceName.Should().Be("new-name");
		request.Notes.Should().Be("some notes");
	}

	[Fact]
	public void AlertActionRequest_Deserializes()
	{
		const string json = """
		{ "alertIds": [ "1", "2" ], "deferMinutes": 30, "priority": "Very High", "reason": "maintenance" }
		""";

		var request = TestSupport.Deserialize<AlertActionRequest>(json);

		request.AlertIds.Should().Equal("1", "2");
		request.DeferMinutes.Should().Be(30);
	}

	[Fact]
	public void AsyncOperationResponse_Deserializes()
	{
		const string json = """
		{ "response": "Request accepted", "taskId": "abc", "location": "/async-operations/abc" }
		""";

		var response = TestSupport.Deserialize<AsyncOperationResponse>(json);

		response.TaskId.Should().Be("abc");
		response.Location.Should().Be("/async-operations/abc");
	}

	[Fact]
	public void TimeRangeQuery_Deserializes()
	{
		const string json = """
		{ "limit": 100, "offset": 0, "next": "2", "sort": "name", "filter": "x", "siteId": "s1",
		  "startAt": "2025-01-01T00:00:00Z", "endAt": "2025-01-02T00:00:00Z" }
		""";

		var query = TestSupport.Deserialize<TimeRangeQuery>(json);

		query.Limit.Should().Be(100);
		query.StartAt.Should().Be("2025-01-01T00:00:00Z");
		query.EndAt.Should().Be("2025-01-02T00:00:00Z");
	}
}
