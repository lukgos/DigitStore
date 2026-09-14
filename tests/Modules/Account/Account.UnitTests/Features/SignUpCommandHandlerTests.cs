using Account.Contracts.Events;
using Account.Module.Abstractions;
using Account.Module.DAL.Repositories;
using Account.Module.Entities;
using Account.Module.Exceptions;
using Account.Module.Features.SignUp;
using Account.Module.ValueObjects;
using FluentAssertions;
using MassTransit;
using NSubstitute;
using Xunit;

namespace Account.UnitTests.Features;

public class SignUpCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly SignUpCommandHandler _handler;

    public SignUpCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        
        _handler = new SignUpCommandHandler(_userRepository, _passwordManager, _publishEndpoint);
    }

    [Fact]
    public async Task SignUp_ShouldThrowEmailAlreadyInUseException_WhenEmailExists()
    {
        // Arrange
        var command = new SignUpCommand(Guid.NewGuid(), "test@example.com", "password123");
        
        _userRepository.ExistsByEmailAsync(Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var action = async () => await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<EmailAlreadyInUseException>();
    }

    [Fact]
    public async Task SignUp_ShouldCreateUserAndPublishEvent_WhenDataIsValid()
    {
        // Arrange
        var command = new SignUpCommand(Guid.NewGuid(), "test@example.com", "password123");
        
        _userRepository.ExistsByEmailAsync(Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(false);
            
        _passwordManager.HashPassword(command.Password).Returns("hashed_password");

        // Act
        await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await _userRepository.Received(1).AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await _publishEndpoint.Received(1).Publish(Arg.Any<AccountCreated>(), Arg.Any<CancellationToken>());
    }
}
