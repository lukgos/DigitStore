using Account.Module.Abstractions;
using Account.Module.Entities;
using Account.Module.Exceptions;
using Account.Module.ValueObjects;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Account.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void UpdatePassword_ShouldThrowInvalidPasswordException_WhenOldPasswordIsIncorrect()
    {
        // Arrange
        var user = User.Create(Guid.NewGuid(), "test@example.com", "old_hash", Role.User);
        var passwordManager = Substitute.For<IPasswordManager>();
        
        passwordManager.ValidatePassword("old_bad_password", "old_hash").Returns(false);

        // Act
        var action = () => user.UpdatePassword("NewPassword123", "old_bad_password", passwordManager);

        // Assert
        action.Should().Throw<InvalidPasswordException>();
    }

    [Fact]
    public void UpdatePassword_ShouldUpdatePasswordHash_WhenOldPasswordIsCorrect()
    {
        // Arrange
        var user = User.Create(Guid.NewGuid(), "test@example.com", "old_hash", Role.User);
        var passwordManager = Substitute.For<IPasswordManager>();
        
        passwordManager.ValidatePassword("old_good_password", "old_hash").Returns(true);
        passwordManager.HashPassword("NewPassword123").Returns("new_hash");

        // Act
        user.UpdatePassword("NewPassword123", "old_good_password", passwordManager);

        // Assert
        user.PasswordHash.Value.Should().Be("new_hash");
    }
}