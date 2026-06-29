using System.Net;

namespace Aruba.Api.Exceptions;

/// <summary>
/// The base exception type for all errors raised by <see cref="ArubaCentralClient"/>.
/// </summary>
public class ArubaApiException : Exception
{
	/// <summary>
	/// Initialises a new instance of the <see cref="ArubaApiException"/> class.
	/// </summary>
	public ArubaApiException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initialises a new instance of the <see cref="ArubaApiException"/> class.
	/// </summary>
	public ArubaApiException(string message, Exception innerException) : base(message, innerException)
	{
	}

	/// <summary>
	/// The HTTP status code associated with the error, when one is available.
	/// </summary>
	public HttpStatusCode? StatusCode { get; init; }
}
