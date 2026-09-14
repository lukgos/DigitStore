using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace Account.IntegrationTests.Features;

public class SignUpEndpointTests : BaseIntegrationTest
{
    public SignUpEndpointTests(AccountTestApp factory) : base(factory)
    {
    }

    [Fact]
    public async Task SignUp_ShouldReturnCreated_WhenDataIsValid()
    {
        var command = new { Email = "test@example.com", Password = "StrongPassword123!" };

        var response = await Client.PostAsJsonAsync("/api/account/sign-up", command);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task SignUp_ShouldReturnBadRequest_WhenEmailAlreadyExists()
    {
        var command = new { Email = "taken@example.com", Password = "StrongPassword123!" };
        
        await Client.PostAsJsonAsync("/api/account/sign-up", command);
        var response = await Client.PostAsJsonAsync("/api/account/sign-up", command);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}