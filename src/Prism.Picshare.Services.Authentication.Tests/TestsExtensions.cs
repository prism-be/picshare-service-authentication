// -----------------------------------------------------------------------
//  <copyright file = "TestsExtensions.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using Dapr.Client;
using Moq;

namespace Prism.Picshare.Services.Authentication.Tests;

public static class TestsExtensions
{
    public static void SetupGetStateAsync<T>(this Mock<DaprClient> mock, string store, string key, T data)
    {
        mock.Setup(x => x.GetStateAsync<T>(store, key, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(data);
    }

    public static void VerifyPublishEvent<TExpected>(this Mock<DaprClient> mock, string expectedPubsub, string expectedTopic)
    {
        mock.Verify(x => x.PublishEventAsync(expectedPubsub, expectedTopic, It.IsAny<TExpected>(), default), Times.Once);
    }

    public static void VerifySaveState<TExpected>(this Mock<DaprClient> mock, string expectedStore)
    {
        mock.Verify(x => x.SaveStateAsync(expectedStore, It.IsAny<string>(), It.IsAny<TExpected>(), It.IsAny<StateOptions>(), It.IsAny<IReadOnlyDictionary<string, string>>(), default),
            Times.Once);
    }
}