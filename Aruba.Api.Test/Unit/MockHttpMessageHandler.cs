using System.Net;
using System.Text;

namespace Aruba.Api.Test.Unit;

/// <summary>
/// A configurable test transport. A handler delegate decides the response for each request,
/// allowing a single instance to answer both the SSO token request and the API requests.
/// </summary>
internal sealed class MockHttpMessageHandler(Func<HttpRequestMessage, int, HttpResponseMessage> responder) : HttpMessageHandler
{
	private int _callCount;

	/// <summary>The requests that have been sent, in order.</summary>
	public List<HttpRequestMessage> Requests { get; } = [];

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		Requests.Add(request);
		var index = Interlocked.Increment(ref _callCount) - 1;
		return Task.FromResult(responder(request, index));
	}

	public static HttpResponseMessage Json(HttpStatusCode statusCode, string content)
		=> new(statusCode)
		{
			Content = new StringContent(content, Encoding.UTF8, "application/json"),
		};

	public static HttpResponseMessage TokenResponse(string accessToken = "test-token", int expiresIn = 3600)
		=> Json(HttpStatusCode.OK, $$"""{"access_token":"{{accessToken}}","token_type":"Bearer","expires_in":{{expiresIn}}}""");

	public static bool IsTokenRequest(HttpRequestMessage request)
		=> request.RequestUri!.Host.Contains("sso", StringComparison.OrdinalIgnoreCase);
}
