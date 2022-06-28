// -----------------------------------------------------------------------
//  <copyright file = "LoginControllerTests.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Prism.Picshare.Services.Authentication.Commands;
using Prism.Picshare.Services.Authentication.Configuration;
using Prism.Picshare.Services.Authentication.Controllers;

namespace Prism.Picshare.Services.Authentication.Tests.Controllers;

public class LoginControllerTests
{
    [Theory]
    [InlineData(ResponseCodes.Ok, typeof(NoContentResult))]
    [InlineData(ResponseCodes.ExistingOrganisation, typeof(ConflictObjectResult))]
    [InlineData(ResponseCodes.ExistingUsername, typeof(ConflictObjectResult))]
    public async Task Register(ResponseCodes code, Type responseType)
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        mediator.Setup(x => x.Send(It.IsAny<RegisterAccountRequest>(), default)).ReturnsAsync(code);

        // Act
        var controller = new LoginController(mediator.Object, Mock.Of<ILogger<LoginController>>());
        var result = await controller.Register(new RegisterAccountRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));

        // Assert
        result.Should().BeAssignableTo(responseType);
    }
}