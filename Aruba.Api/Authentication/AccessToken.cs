namespace Aruba.Api.Authentication;

/// <summary>
/// An OAuth 2.0 access token together with the (UTC) instant at which it should be considered expired.
/// </summary>
internal sealed record AccessToken(string Value, DateTimeOffset ExpiresAtUtc)
{
	/// <summary>
	/// Determines whether the token is still valid at the supplied instant.
	/// </summary>
	public bool IsValidAt(DateTimeOffset nowUtc) => nowUtc < ExpiresAtUtc;
}
