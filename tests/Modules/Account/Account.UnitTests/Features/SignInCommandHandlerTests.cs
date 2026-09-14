using Account.Module.Abstractions;
using Account.Module.DAL.Repositories;
using Account.Module.Entities;
using Account.Module.Exceptions;
using Account.Module.Features.SignIn;
using Account.Module.ValueObjects;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Account.UnitTests.Features;

public class SignInCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenManager _tokenManager;
    private readonly ITokenStorage _tokenStorage;
    private readonly SignInCommandHandler _handler;

    public SignInCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _tokenManager = Substitute.For<ITokenManager>();
        _tokenStorage = Substitute.For<ITokenStorage>();

        _handler = new SignInCommandHandler(_userRepository, _passwordManager, _tokenManager, _tokenStorage);
    }

    [Fact]
    public async Task SignIn_ShouldThrowInvalidCredentialsException_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new SignInCommand("test@example.com", "Password123");
        
        _userRepository.GetByEmailAsync(Arg.Any<Email>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<User?>(null));

        // Act
        var action = async () => await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async Task SignIn_ShouldThrowInvalidCredentialsException_WhenPasswordIsInvalid()
    {
        // Arrange
        var command = new SignInCommand("test@example.com", "BadPassword");
        var user = User.Create(Guid.NewGuid(), "test@example.com", "hashed_password", Role.User);

        _userRepository.GetByEmailAsync(Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(user);
            
        _passwordManager.ValidatePassword(command.Password, user.PasswordHash.Value).Returns(false);

        // Act
        var action = async () => await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async Task SignIn_ShouldGenerateAndStoreToken_WhenCredentialsAreValid()
    {
        // Arrange
        var command = new SignInCommand("test@example.com", "GoodPassword");
        var user = User.Create(Guid.NewGuid(), "test@example.com", "hashed_password", Role.Admin);
        var expectedToken = "jwt_secret_token";

        _userRepository.GetByEmailAsync(Arg.Any<Email>(), Arg.Any<CancellationToken>()).Returns(user);
        _passwordManager.ValidatePassword(command.Password, user.PasswordHash.Value).Returns(true);
        _tokenManager.GenerateAccessToken(user).Returns(expectedToken);

        // Act
        await _handler.HandleAsync(command, CancellationToken.None);

        _tokenStorage.Received(1).Set(expectedToken);
    }
}