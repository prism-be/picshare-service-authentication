// -----------------------------------------------------------------------
//  <copyright file = "SubscribeRequestTests.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using Dapr.Client;
using FluentAssertions;
using Moq;
using Prism.Picshare.Domain;
using Prism.Picshare.Events;
using Prism.Picshare.Services.Authentication.Commands;
using Prism.Picshare.Services.Authentication.Configuration;

namespace Prism.Picshare.Services.Authentication.Tests.Commands;

public class RegisterAccountRequestTests
{

    [Fact]
    public async Task Handle_Ok()
    {
        // Arrange
        var login = Guid.NewGuid().ToString();
        var daprClient = new Mock<DaprClient>();

        // Act
        var handler = new RegisterAccountRequestHandler(daprClient.Object);
        var result = await handler.Handle(new RegisterAccountRequest(login, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            CancellationToken.None);

        // Assert
        result.Should().Be(ResponseCodes.Ok);
        daprClient.VerifySaveState<Organisation>(Stores.Organisations);
        daprClient.VerifySaveState<User>(Stores.Users);
        daprClient.VerifyPublishEvent<User>(DaprConfiguration.PubSub, Topics.User.Register);
    }

    [Fact]
    public async Task Handle_Organisation_Exist()
    {
        // Arrange
        var daprClient = new Mock<DaprClient>();
        var organisationName = Guid.NewGuid().ToString();
        daprClient.SetupGetStateAsync(Stores.OrganisationsName, organisationName, Guid.NewGuid());

        // Act
        var handler = new RegisterAccountRequestHandler(daprClient.Object);
        var result = await handler.Handle(
            new RegisterAccountRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), organisationName),
            CancellationToken.None);

        // Assert
        result.Should().Be(ResponseCodes.ExistingOrganisation);
    }

    [Fact]
    public async Task Handle_User_Exist()
    {
        // Arrange
        var login = Guid.NewGuid().ToString();
        var daprClient = new Mock<DaprClient>();
        daprClient.SetupGetStateAsync(Stores.Credentials, login, new Credentials());

        // Act
        var handler = new RegisterAccountRequestHandler(daprClient.Object);
        var result = await handler.Handle(new RegisterAccountRequest(login, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            CancellationToken.None);

        // Assert
        result.Should().Be(ResponseCodes.ExistingUsername);
    }

    [Fact]
    public void Validator_Invalid_Email()
    {
        // Arrange
        var validator = new RegisterAccountRequestValidator();

        // Act
        var result = validator.Validate(new RegisterAccountRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()));

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_Missing_Email()
    {
        // Arrange
        var validator = new RegisterAccountRequestValidator();

        // Act
        var result = validator.Validate(new RegisterAccountRequest(Guid.NewGuid().ToString(), string.Empty, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_Missing_Login()
    {
        // Arrange
        var validator = new RegisterAccountRequestValidator();

        // Act
        var result = validator.Validate(new RegisterAccountRequest(string.Empty, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_Missing_Name()
    {
        // Arrange
        var validator = new RegisterAccountRequestValidator();

        // Act
        var result = validator.Validate(new RegisterAccountRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), string.Empty, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_Missing_Organisation()
    {
        // Arrange
        var validator = new RegisterAccountRequestValidator();

        // Act
        var result = validator.Validate(new RegisterAccountRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), string.Empty));

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_Missing_Password()
    {
        // Arrange
        var validator = new RegisterAccountRequestValidator();

        // Act
        var result = validator.Validate(new RegisterAccountRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), string.Empty, Guid.NewGuid().ToString()));

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_Ok()
    {
        // Arrange
        var validator = new RegisterAccountRequestValidator();

        // Act
        var result = validator.Validate(new RegisterAccountRequest(Guid.NewGuid().ToString(), "hello@pichsare.me", Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()));

        // Assert
        result.IsValid.Should().BeTrue();
    }
}