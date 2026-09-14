using System.Net;
using System.Net.Http.Json;
using Account.Module.Features.SignIn;
using FluentAssertions;
using Xunit;

namespace Account.IntegrationTests.Features;

public class SignInEndpointTests : BaseIntegrationTest
{
    public SignInEndpointTests(AccountTestApp factory) : base(factory)
    {
    }

    [Fact]
    public async Task SignIn_ShouldReturnOkAndSetCookie_WhenCredentialsAreValid()
    {
        var email = "test@example.com";
        var password = "StrongPassword123!";
        await Client.PostAsJsonAsync("/api/account/sign-up", new { Email = email, Password = password });

        var signInCommand = new SignInCommand(email, password);

        var response = await Client.PostAsJsonAsync("/api/account/sign-in", signInCommand);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response.Headers.TryGetValues("Set-Cookie", out var cookies).Should().BeTrue();
        var accessTokenCookie = cookies!.FirstOrDefault(c => c.StartsWith("access_token="));
        
        accessTokenCookie.Should().NotBeNull();
        accessTokenCookie.Should().Contain("httponly");
    }
}