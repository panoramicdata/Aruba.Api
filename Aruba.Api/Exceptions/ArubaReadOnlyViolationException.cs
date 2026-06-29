namespace Aruba.Api.Exceptions;

/// <summary>
/// Thrown when a mutating request (any verb other than <c>GET</c>) is attempted while the client is
/// in read-only mode (<see cref="ArubaCentralClientOptions.IsReadOnly"/> is <see langword="true"/>,
/// which is the default). The request is blocked before it is sent.
/// </summary>
/// <remarks>
/// Derives from <see cref="InvalidOperationException"/>. Note that Refit wraps exceptions thrown by
/// the HTTP pipeline for endpoints that deserialise a response, so a caught exception may be a
/// <c>Refit.ApiRequestException</c> whose <see cref="Exception.InnerException"/> is an instance of
/// this type. For bodyless endpoints (for example <c>DELETE</c>) it is thrown directly.
/// </remarks>
public sealed class ArubaReadOnlyViolationException(HttpMethod method, Uri? requestUri)
	: InvalidOperationException(
		$"The client is configured as read-only; the {method} request to {requestUri} was blocked. " +
		"Set ArubaCentralClientOptions.IsReadOnly = false to permit write operations.")
{
	/// <summary>The HTTP method that was blocked.</summary>
	public HttpMethod Method { get; } = method;

	/// <summary>The request URI that was blocked.</summary>
	public Uri? RequestUri { get; } = requestUri;
}
