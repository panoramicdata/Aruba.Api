using Aruba.Api.Exceptions;
using AwesomeAssertions;
using System.Net;

namespace Aruba.Api.Test.Unit;

public sealed class ExceptionTests
{
	[Fact]
	public void ArubaApiException_MessageOnly_HasNoStatusCode()
	{
		var ex = new ArubaApiException("boom");
		ex.Message.Should().Be("boom");
		ex.StatusCode.Should().BeNull();
		ex.InnerException.Should().BeNull();
	}

	[Fact]
	public void ArubaApiException_WithInner_PreservesInner()
	{
		var inner = new InvalidOperationException("cause");
		var ex = new ArubaApiException("boom", inner);
		ex.InnerException.Should().BeSameAs(inner);
	}

	[Fact]
	public void ArubaApiException_StatusCode_IsSettableViaInitialiser()
	{
		var ex = new ArubaApiException("boom") { StatusCode = HttpStatusCode.BadGateway };
		ex.StatusCode.Should().Be(HttpStatusCode.BadGateway);
	}

	[Fact]
	public void ArubaAuthenticationException_MessageOnly_HasNoStatusCode()
	{
		var ex = new ArubaAuthenticationException("no token");
		ex.Message.Should().Be("no token");
		ex.StatusCode.Should().BeNull();
		ex.Should().BeAssignableTo<ArubaApiException>();
	}

	[Fact]
	public void ArubaAuthenticationException_WithStatusCode_PropagatesStatusCode()
	{
		var ex = new ArubaAuthenticationException("unauthorized", HttpStatusCode.Unauthorized);
		ex.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
	}

	[Fact]
	public void ArubaReadOnlyViolationException_ExposesMethodAndUri()
	{
		var uri = new Uri("https://example.test/devices/AP1");
		var ex = new ArubaReadOnlyViolationException(HttpMethod.Delete, uri);

		ex.Method.Should().Be(HttpMethod.Delete);
		ex.RequestUri.Should().Be(uri);
		ex.Should().BeAssignableTo<InvalidOperationException>();
		ex.Message.Should().Contain("DELETE").And.Contain(uri.ToString());
	}

	[Fact]
	public void ArubaReadOnlyViolationException_NullUri_IsTolerated()
	{
		var ex = new ArubaReadOnlyViolationException(HttpMethod.Post, null);
		ex.RequestUri.Should().BeNull();
		ex.Method.Should().Be(HttpMethod.Post);
	}
}
