using System.IdentityModel.Tokens.Jwt;
using Account.Module.Entities;
using Account.Module.Services;
using Account.Module.Settings;
using Account.Module.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Account.UnitTests.Services;

public class JwtTokenManagerTests
{
    [Fact]
    public void GenerateAccessToken_ShouldCreateValidJwtWithCorrectClaims()
    {
        // Arrange
        var jwtOptions = new JwtOptions
        {
            SigningKey = "OF1VhKkvInetguEhjbOWCvLfUAbmxeUCTm0SHJBS0Ha",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpirationMinutes = 60
        };
        
        var optionsMock = Substitute.For<IOptions<JwtOptions>>();
        optionsMock.Value.Returns(jwtOptions);
        
        var manager = new JwtTokenManager(optionsMock);
        var user = User.Create(Guid.NewGuid(), "test@example.com", "hash", Role.Admin);

        // Act
        var tokenString = manager.GenerateAccessToken(user);

        // Assert
        tokenString.Should().NotBeNullOrWhiteSpace();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(tokenString);

        jwtToken.Issuer.Should().Be(jwtOptions.Issuer);
        jwtToken.Audiences.Should().Contain(jwtOptions.Audience);
        
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == "test@example.com");
        jwtToken.Claims.Should().Contain(c => c.Type == "role" && c.Value == Role.Admin);
        jwtToken.Claims.Should().Contain(c => c.Type == "nameid" && c.Value == user.Id.Value.ToString());
    }
}
