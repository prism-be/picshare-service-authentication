// -----------------------------------------------------------------------
//  <copyright file = "SubscribeRequestHandlerTests.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using Dapr.Client;
using FluentAssertions;
using Moq;
using Prism.Picshare.Domain;
using Prism.Picshare.Services.Authentication.Commands;
using Prism.Picshare.Services.Authentication.Configuration;

namespace Prism.Picshare.Services.Authentication.Tests.Commands;

public class SubscribeRequestHandlerTests
{

    [Fact]
    public async Task Handle_Ok()
    {
        // Arrange
        var login = Guid.NewGuid().ToString();
        var daprClient = new Mock<DaprClient>();

        // Act
        var handler = new SubscribeRequestHandler(daprClient.Object);
        var result = await handler.Handle(new SubscribeRequest(login, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()), CancellationToken.None);

        // Assert
        result.Should().Be(ResponseCodes.Ok);
        daprClient.VerifySaveState<Organisation>(Stores.Organisations);
        daprClient.VerifySaveState<User>(Stores.Users);
    }

    [Fact]
    public async Task Handle_Organisation_Exist()
    {
        // Arrange
        var daprClient = new Mock<DaprClient>();
        var items = new List<StateQueryItem<Organisation>>
        {
            new(Guid.NewGuid().ToString(), new Organisation(), Guid.NewGuid().ToString(), null)
        };
        daprClient.Setup(x => x.QueryStateAsync<Organisation>(Stores.Organisations, It.IsAny<string>(), It.IsAny<IReadOnlyDictionary<string, string>>(), default))
            .ReturnsAsync(new StateQueryResponse<Organisation>(new List<StateQueryItem<Organisation>>(items), null, null));

        // Act
        var handler = new SubscribeRequestHandler(daprClient.Object);
        var result = await handler.Handle(new SubscribeRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()), CancellationToken.None);

        // Assert
        result.Should().Be(ResponseCodes.ExistingOrganisation);
    }

    [Fact]
    public async Task Handle_User_Exist()
    {
        // Arrange
        var login = Guid.NewGuid().ToString();
        var daprClient = new Mock<DaprClient>();
        var items = new List<StateQueryItem<User>>
        {
            new(Guid.NewGuid().ToString(), new User
            {
                Login = login
            }, Guid.NewGuid().ToString(), null)
        };
        daprClient.Setup(x => x.QueryStateAsync<User>(Stores.Users, It.IsAny<string>(), It.IsAny<IReadOnlyDictionary<string, string>>(), default))
            .ReturnsAsync(new StateQueryResponse<User>(new List<StateQueryItem<User>>(items), null, null));

        // Act
        var handler = new SubscribeRequestHandler(daprClient.Object);
        var result = await handler.Handle(new SubscribeRequest(login, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()), CancellationToken.None);

        // Assert
        result.Should().Be(ResponseCodes.ExistingUsername);
    }
}