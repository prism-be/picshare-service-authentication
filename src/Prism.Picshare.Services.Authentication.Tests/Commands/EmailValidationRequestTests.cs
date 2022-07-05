// -----------------------------------------------------------------------
//  <copyright file = "EmailValidationRequestTests.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using Dapr.Client;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Prism.Picshare.Domain;
using Prism.Picshare.Services.Authentication.Commands;
using Prism.Picshare.Services.Authentication.Configuration;

namespace Prism.Picshare.Services.Authentication.Tests.Commands;

public class EmailValidationRequestTests
{
    [Fact]
    public async Task Handle_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<EmailValidationRequestHandler>>();
        var daprClient = new Mock<DaprClient>();

        var request = new EmailValidationRequest(Guid.NewGuid(), Guid.NewGuid());
        daprClient.SetupGetStateAsync(Stores.Users, EntityReference.ComputeKey(request.OrganisationId, request.UserId), new User
        {
            OrganisationId = request.OrganisationId,
            Id = request.UserId
        });

        // Act
        var handler = new EmailValidationRequestHandler(logger.Object, daprClient.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(ResponseCodes.UserNotFound);
    }
    
    [Fact]
    public async Task Handle_Ok()
    {
        // Arrange
        var logger = new Mock<ILogger<EmailValidationRequestHandler>>();
        var daprClient = new Mock<DaprClient>();

        var request = new EmailValidationRequest(Guid.NewGuid(), Guid.NewGuid());
        daprClient.SetupGetStateAsync(Stores.Users, EntityReference.ComputeKey(request.OrganisationId, request.UserId), new User
        {
            OrganisationId = request.OrganisationId,
            Id = request.UserId
        });

        // Act
        var handler = new EmailValidationRequestHandler(logger.Object, daprClient.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(ResponseCodes.Ok);
        daprClient.VerifySaveState<User>(Stores.Users);
    }

    [Fact]
    public async Task Validator_Empty_Id()
    {
        // Arrange
        var request = new EmailValidationRequest(Guid.NewGuid(), Guid.Empty);

        // Act
        var validator = new EmailValidationRequestValidator();
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validator_Empty_Organisation()
    {
        // Arrange
        var request = new EmailValidationRequest(Guid.Empty, Guid.NewGuid());

        // Act
        var validator = new EmailValidationRequestValidator();
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validator_Ok()
    {
        // Arrange
        var request = new EmailValidationRequest(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var validator = new EmailValidationRequestValidator();
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}