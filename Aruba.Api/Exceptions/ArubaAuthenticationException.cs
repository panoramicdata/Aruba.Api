using System.Net;

namespace Aruba.Api.Exceptions;

/// <summary>
/// Thrown when the OAuth 2.0 <c>client_credentials</c> token request to HPE GreenLake SSO fails.
/// </summary>
public sealed class ArubaAuthenticationException : ArubaApiException
{
	/// <summary>
	/// Initialises a new instance of the <see cref="ArubaAuthenticationException"/> class.
	/// </summary>
	public ArubaAuthenticationException(string message, HttpStatusCode? statusCode = null)
		: base(message)
	{
		StatusCode = statusCode;
	}
}
