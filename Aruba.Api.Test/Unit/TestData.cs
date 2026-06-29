namespace Aruba.Api.Test.Unit;

/// <summary>Canned API response bodies taken from the New Central API documentation examples.</summary>
internal static class TestData
{
	public const string AccessPointsPage = """
	{
	  "items": [
	    {
	      "serialNumber": "AP00000001",
	      "macAddress": "bb:5d:11:85:c0:00",
	      "deviceName": "ArubaAP",
	      "model": "AP-115",
	      "status": "ONLINE",
	      "lastSeenAt": null,
	      "siteId": "1120972196",
	      "siteName": "site1",
	      "cpuUtilization": 25,
	      "memoryUtilization": 11,
	      "powerConsumption": 11.891,
	      "clientCount": 8,
	      "id": "AP00000001",
	      "type": "network-monitoring/access-point-monitoring"
	    }
	  ],
	  "count": 1,
	  "total": 4,
	  "next": "2"
	}
	""";

	public const string AlertsPage = """
	{
	  "count": 1,
	  "next": "2",
	  "total": 85,
	  "items": [
	    {
	      "status": "Cleared",
	      "category": "System",
	      "severity": "Critical",
	      "deviceType": "Access Point",
	      "type": "network-notifications/alerts",
	      "priority": "Very High",
	      "id": "22071893000:47765082406",
	      "createdAt": "2025-12-10T07:04:33.352Z",
	      "updatedAt": "",
	      "name": "Insufficient PoE Received",
	      "summary": "AP did not receive the requested PoE power."
	    }
	  ]
	}
	""";

	public const string AsyncAccepted = """
	{
	  "response": "Request accepted",
	  "taskId": "abc123-def456-ghi789",
	  "location": "/network-notifications/v1/alerts/async-operations/abc123-def456-ghi789"
	}
	""";
}
