using System.Net;
using System.Net.Http.Json;
using Account.Module.DTOs;
using Account.Module.Features.SignIn;
using FluentAssertions;
using Xunit;

namespace Account.IntegrationTests.Features;

public class GetMeEndpointTests : BaseIntegrationTest
{
    public GetMeEndpointTests(AccountTestApp factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetMe_ShouldReturnUserDetails_WhenUserIsAuthenticated()
    {
        var email = "test@example.com";
        var password = "StrongPassword123!";
        
        await Client.PostAsJsonAsync("/api/account/sign-up", new { Email = email, Password = password });
        
        var loginResponse = await Client.PostAsJsonAsync("/api/account/sign-in", new SignInCommand(email, password));
        
        var setCookieHeader = loginResponse.Headers.GetValues("Set-Cookie").First();
        var tokenValue = setCookieHeader.Split(';')[0];

        var request = new HttpRequestMessage(HttpMethod.Get, "/api/account/me");
        request.Headers.Add("Cookie", tokenValue);

        var response = await Client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var userDetails = await response.Content.ReadFromJsonAsync<UserDto>();
        
        userDetails.Should().NotBeNull();
        userDetails!.Email.Should().Be(email);
        userDetails.Role.Should().Be("user");
        userDetails.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetMe_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        var response = await Client.GetAsync("/api/account/me");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}