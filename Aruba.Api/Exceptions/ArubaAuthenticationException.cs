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
	public ArubaAuthenticationException(string message)
		: base(message)
	{
	}

	/// <summary>
	/// Initialises a new instance of the <see cref="ArubaAuthenticationException"/> class with the
	/// HTTP status code returned by the token endpoint.
	/// </summary>
	public ArubaAuthenticationException(string message, HttpStatusCode? statusCode)
		: base(message)
	{
		StatusCode = statusCode;
	}
}
