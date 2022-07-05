// -----------------------------------------------------------------------
//  <copyright file = "EmailValidatedControllerTests.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Prism.Picshare.Domain;
using Prism.Picshare.Services.Authentication.Commands;
using Prism.Picshare.Services.Authentication.Controllers.Events;

namespace Prism.Picshare.Services.Authentication.Tests.Controllers.Events;

public class EmailValidatedControllerTests
{
    [Fact]
    public async Task Validate_Ok()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            OrganisationId = Guid.NewGuid()
        };

        var mediator = new Mock<IMediator>();

        // Act
        var controller = new EmailValidatedController(mediator.Object);
        var result = await controller.Validate(user);

        // Assert
        result.Should().BeAssignableTo<OkResult>();
        mediator.Verify(x => x.Send(It.Is<EmailValidationRequest>(r => r.UserId == user.Id && r.OrganisationId == user.OrganisationId), It.IsAny<CancellationToken>()), Times.Once);
    }
}