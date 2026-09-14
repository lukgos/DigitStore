using Account.Module.Exceptions;
using Account.Module.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Account.UnitTests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("admin.123@domain.com.pl")]
    public void Constructor_ShouldCreateEmail_WhenFormatIsValid(string validEmail)
    {
        // Act
        var email = new Email(validEmail);

        // Assert
        email.Value.Should().Be(validEmail);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("test@")]
    [InlineData("")]
    public void Constructor_ShouldThrowInvalidEmailException_WhenFormatIsInvalid(string invalidEmail)
    {
        // Act
        var action = () => new Email(invalidEmail);

        // Assert
        action.Should().Throw<InvalidEmailException>();
    }
}